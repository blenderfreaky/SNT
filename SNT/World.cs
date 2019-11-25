namespace SNT
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading.Tasks;
    using System.Linq;

    public class World
    {
        public static float Gravity;
        public static float Damping;
        public static float Theta;

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

        public void Step()
        {
            var quadtree = Quadtree.Build(Particles, Min, Max);

            Parallel.ForEach(Particles, particle =>
            {
                var a = Particles.ToList();
                a.Remove(particle);
                a.Select(x => quadtree.GetForce(particle, x) / particle.Mass);
                particle.Velocity += quadtree.GetForce(particle, this) / particle.Mass;
                particle.Position += particle.Velocity;
            });
        }

        public static Vector2 ForceBetween(float distanceSquared, Vector2 p1, Vector2 p2, float m1, float m2) =>
             (p1 - p2) * (Gravity * m1 * m2 / MathF.Pow(distanceSquared + Damping, 3f / 2f));
    }
}