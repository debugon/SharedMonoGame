using System;
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

        public Camera() { }

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(Position, Target, Vector3.UnitZ);
            }
        }

        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlaneDistance, FarPlaneDistance);
            }
        }

        /// <summary>
        /// Z軸を中心に回転（原点Oを基点にするので注意）
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public void RotationTarget(float degrees)
        {
            float radian = MathHelper.ToRadians(degrees);
            float cos = (float)Math.Cos(radian);
            float sin = (float)Math.Sin(radian);
                
            Position = new Vector3(Position.X * cos - Position.Y * sin, Position.X * sin + Position.Y * cos, Position.Z);
            Target = new Vector3(Target.X * cos - Target.Y * sin, Target.X * sin + Target.Y * cos, Target.Z);

        }

    }
}
