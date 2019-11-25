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

        public Quadtree Quadtree;

        public void Step()
        {
            var quadtree = Quadtree = Quadtree.Build(Particles, Min, Max);

            Parallel.ForEach(Particles, particle =>
            {
                particle.Velocity += quadtree.GetAccel(particle, this);
                particle.Position += particle.Velocity;
            });
        }

        public Vector2 AccelFor(float distanceSquared, Particle particle, Vector2 p2, float m2) =>
            (p2 - particle.Position)
            * (Gravity * m2 / particle.Mass
                / MathF.Pow(distanceSquared + Damping, 3f / 2f));
    }
}