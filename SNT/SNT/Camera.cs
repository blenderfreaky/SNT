namespace SNT
{
    using System.Numerics;

    public class Camera
    {
        public Vector4 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public float FocalLength { get; set; }

        public Camera(Vector4 position, Quaternion rotation, float focalLength)
        {
            Position = position;
            Rotation = rotation;
            FocalLength = focalLength;
        }

        public Vector4 Project(Vector4 point)
        {
            return point;
        }
    }
}