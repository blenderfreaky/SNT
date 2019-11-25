namespace SNT
{
    using System.Numerics;

    public class Particle
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                //Trace[TraceIndex] = value;
                //TraceIndex = (TraceIndex + 1) % Trace.Length;
            }
        }

        public Vector2 Velocity;
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
