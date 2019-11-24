namespace SNT
{
    using System.Numerics;

    public class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Mass;

        public World World;

        public Particle(Vector2 position, Vector2 velocity, float mass, World world)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
            World = world;
        }
    }
}
