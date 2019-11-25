namespace SNT
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading.Tasks;
    using System.Linq;

    public static class World
    {
        public static float Gravity;
        public static float Damping;
        public static float Theta;

        public static Vector2 Min, Max;

        public static List<Particle> Particles = new List<Particle>();

        public static void SetWorld(float gravity, float damping, float theta, Vector2 min, Vector2 max)
        {
            Gravity = gravity;
            Damping = damping;
            Theta = theta;
            Min = min;
            Max = max;
        }

        public static void AddParticle(Particle p)
        {
            Particles.Add(p);
        }

        public static void AddParticle(Vector2 position, Vector2 velocity, float mass)
        {
            Particles.Add(new Particle(position, velocity, mass));
        }

        public static void Step()
        {
            var quadtree = Quadtree.Build(Particles, Min, Max);

            Parallel.ForEach(Particles, particle =>
            {
                var a = Particles.ToList();
                a.Remove(particle);
                particle.Velocity += AddThem(a.Select(x => quadtree.GetForce(particle, x) / particle.Mass).ToList());
                
                particle.Position += particle.Velocity;
            });
        }

        public static Vector2 ForceBetween(float distanceSquared, Vector2 p1, Vector2 p2, float m1, float m2) =>
             (p1 - p2) * (Gravity * m1 * m2 / MathF.Pow(distanceSquared + Damping, 3f / 2f));

        public static Vector2 AddThem(this List<Vector2> vecs)
        {
            var aggregate = Vector2.Zero;

            for (int i = 0; i < vecs.Count; i++) aggregate += vecs[i];
            return aggregate;
        }
    }
}