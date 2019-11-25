namespace SNT
{
    using System.Numerics;

    public class Particle
    {
        public Vector2 Position;

        public Vector2 Velocity;

        public Vector2 Accel;

        public float Mass;

        public World World;

        public Vector2[] Trace;
        public int TraceIndex;

        public Particle(Vector2 position, Vector2 velocity, float mass, World world)
        {
            Velocity = velocity;
            Mass = mass;
            World = world;

            //Trace = new Vector2[5];
            //for (int i = 0; i < Trace.Length; i++) Trace[i] = position;

            Position = position;
        }
    }
}
