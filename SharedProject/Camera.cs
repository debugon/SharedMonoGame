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
        /// Y軸を中心に回転（向いている方向に移動しないので未完成）
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public void RotationTarget(float degrees)
        {
            float radian = MathHelper.ToRadians(degrees);
            float cos = (float)Math.Cos(radian);
            float sin = (float)Math.Sin(radian);
            
            Position = new Vector3(Position.X * cos - Position.Z * sin, Position.Y, -(Position.X * sin + Position.Z * cos));
            Target = new Vector3(-(Target.X * cos - Target.Z * sin), Target.Y, Target.X * sin + Target.Z * cos);

        }

    }
}
