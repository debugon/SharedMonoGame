using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharedProject
{
    public partial class GameObject
    {
        public Model Model { set; get; }
        public Texture2D Texture { set; get; }

        public float Scale { set; get; }
        public Vector3 Rotation { set; get; }
        public Vector3 Position { set; get; }

        public Matrix World
        {
            get
            {
                return Matrix.CreateScale(Scale)
                    * Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(Rotation.X))
                    * Matrix.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(Rotation.Y))
                    * Matrix.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(Rotation.Z))
                    * Matrix.CreateTranslation(Position);
            }
        }

        public GameObject() { }

        public GameObject(Microsoft.Xna.Framework.Content.ContentManager contentManager, string filePath, float modelScale)
        {
            Model = contentManager.Load<Model>(filePath);
            Scale = modelScale;
        }

    }

    public partial class GameObject
    {
        //モデル描画処理
        public virtual void DrawModel(Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    //テクスチャが読み込まれていたら張り付ける
                    if (Texture != null)
                    {
                        effect.Texture = Texture;
                    }

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}
