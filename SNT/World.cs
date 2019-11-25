namespace SNT
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading.Tasks;

    public class World
    {
        public float Gravity;
        public float Damping;
        public float Theta;

        public Vector2 Min, Max;

        public List<Particle> Particles;

        public World(Vector2 min, Vector2 max, float gravity = 10f, float damping = 0.1f, float theta = 1f)
            : this(gravity, damping, theta, min, max, new List<Particle>())
        { }

        public World(float gravity, float damping, float theta, Vector2 min, Vector2 max, List<Particle> particles)
        {
            Gravity = gravity;
            Damping = damping;
            Theta = theta;
            Min = min;
            Max = max;
            Particles = particles;
        }

        public void AddParticle(Particle p)
        {
            p.World = this;
            Particles.Add(p);
        }

        public void AddParticle(Vector2 position, Vector2 velocity, float mass)
        {
            Particles.Add(new Particle(position, velocity, mass, this));
        }

        public Quadtree? Quadtree;

        public void Step()
        {
            //Parallel.ForEach(Particles, particle =>
            //{
            //    Parallel.ForEach(Particles, other =>
            //    {
            //        var diff = (other.Position - particle.Position);

            //        particle.Velocity += diff * Gravity * other.Mass / MathF.Pow(diff.LengthSquared() + Damping, 3f / 2f);
            //    });
            //    particle.Position += particle.Velocity;
            //});

            //return;

            float deltaT = 1;

            var quadtree = Quadtree = Quadtree.Build(Particles, Min, Max);

            var avg = Vector2.Zero;

            for (int i = 0; i < Particles.Count; i++)
            {
                avg += Particles[i].Position;
            }

            avg /= Particles.Count;

            Parallel.ForEach(Particles, particle =>
                {
                    // Leapfrog integration

                    particle.Position += (deltaT * particle.Velocity) + (particle.Accel / 2) - avg;
                    var prevAccel = particle.Accel;
                    particle.Accel = quadtree.GetAccel(particle, this);
                    particle.Velocity += deltaT * (prevAccel + particle.Accel) / 2;
                });
        }

        public Vector2 AccelFor(float distanceSquared, Particle particle, Vector2 p2, float m2) =>
            (p2 - particle.Position)
            * (Gravity * m2
                / MathF.Pow(distanceSquared + Damping, 3f / 2f));
    }
}