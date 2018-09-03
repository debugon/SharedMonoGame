using Microsoft.Xna.Framework;

namespace SharedProject
{
    public class Camera
    {
        public Vector3 Position { set; get; }
        public Vector3 Target { set; get; }

        public float FieldOfView { set; get; }
        public float AspectRatio { set; get; }
        public float NearPlaneDistance { set; get; }
        public float FarPlaneDistance { set; get; }

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(Position, Target, Vector3.UnitY);
            }
        }

        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlaneDistance, FarPlaneDistance);
            }
        }

        public Camera() { }
    }
}
