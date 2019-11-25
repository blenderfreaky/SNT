namespace SNT
{
    using System.Collections.Generic;
    using System.Numerics;

    public class Quadtree
    {
        public Quadtree[]? Nodes;
        public Particle? Particle;

        public Vector2 Min, Max;

        public Vector2 CenterOfMass;
        public float TotalMass;

        public Quadtree(Vector2 min, Vector2 max)
        {
            Nodes = null;
            Particle = null;
            Min = min;
            Max = max;
        }

        public Quadtree(Particle? particle, Vector2 min, Vector2 max)
        {
            Nodes = null;
            Particle = particle;
            Min = min;
            Max = max;
        }

        public static Quadtree Build(List<Particle> particles, Vector2 min, Vector2 max)
        {
            if (particles.Count == 0) return new Quadtree(min, max);

            Quadtree quadtree = new Quadtree(particles[0], min, max);

            for (int i = 1; i < particles.Count; i++)
            {
                if (!quadtree.Insert(particles[i]))
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }

            quadtree.Compute();

            return quadtree;
        }

        public Vector2 GetAccel(Particle particle, World world)
        {
            if (TotalMass == 0) return Vector2.Zero;

            float distanceSquared = (particle.Position - CenterOfMass).LengthSquared();

            if (distanceSquared == 0) return Vector2.Zero;

            if ((Max.X - Min.X) / distanceSquared < world.Theta || Particle != null)
            {
                //return (particle.Position - CenterOfMass) / -distanceSquared * TotalMass * 0.0001f;
                return world.AccelFor(
                    distanceSquared, particle,
                    CenterOfMass, TotalMass);
            }

            Vector2 accel = Vector2.Zero;

            for (int i = 0; i < 4; i++)
            {
                accel += Nodes![i].GetAccel(particle, world);
            }

            return accel;
        }

        public void Compute()
        {
            if (Particle != null)
            {
                CenterOfMass = Particle.Position;
                TotalMass = Particle.Mass;

                return;
            }

            if (Nodes == null) return;

            for (int i = 0; i < 4; i++)
            {
                Nodes![i].Compute();

                var mass = Nodes![i].TotalMass;

                CenterOfMass += Nodes![i].CenterOfMass * mass;
                TotalMass += mass;
            }

            if (TotalMass != 0)
            {
                CenterOfMass /= TotalMass;
            }
            else
            {
                CenterOfMass = Vector2.Zero;
            }
        }

        public void Subdivide()
        {
            Nodes = new Quadtree[4];

            var halfsize = (Max - Min) / 2;
            var halfsizeX = new Vector2(halfsize.X, 0);
            var halfsizeY = new Vector2(0, halfsize.Y);

            Nodes[0] = new Quadtree(Min, Max - halfsize);
            Nodes[1] = new Quadtree(Min + halfsizeX, Max - halfsizeY);
            Nodes[2] = new Quadtree(Min + halfsizeY, Max - halfsizeX);
            Nodes[3] = new Quadtree(Min + halfsize, Max);
        }

        public bool Insert(Particle particle)
        {
            var lowest = GetLowest(particle.Position);

            if (lowest == null) return true;

            if (lowest.Particle != null)
            {
                if ((lowest.Particle.Position - particle.Position).LengthSquared() < 4)
                {
                    lowest.Particle.Mass += particle.Mass;
                    return false;
                }

                lowest.Subdivide();
                lowest.Insert(lowest.Particle);
                lowest.Particle = null;

                lowest.Insert(particle);
            }
            else
            {
                lowest.Particle = particle;
            }

            return true;
        }

        public bool Contains(Vector2 position) =>
            position.X >= Min.X && position.Y >= Min.Y
            && position.X <= Max.X && position.Y <= Max.Y;

        public Quadtree? GetLowest(Vector2 position)
        {
            if (Nodes == null) return this;

            for (int i = 0; i < 4; i++)
            {
                //if (Nodes[i] == null) throw new System.Exception();

                if (Nodes[i].Contains(position))
                {
                    return Nodes[i].GetLowest(position);
                }
            }

            return null;
        }

        public override string ToString() => $"Quadtree ({Particle}) ({Nodes})";
    }
}
