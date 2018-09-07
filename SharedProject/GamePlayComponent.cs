﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace SharedProject
{
    public class GamePlayComponent : DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        private Texture2D texture;
        private SpriteFont font;

        private Camera camera;
        private Vector3 offset;

        private Mech mechObject;
        private GameObject missileObject;

        private BasicEffect effect;
        private VertexPositionTexture[] floorVerts;

        public GamePlayComponent(Game game) : base(game)
        {

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization logic here
            Input.Initialize();

            camera = new Camera()
            {
                Position = new Vector3(0, 10, 20),
                Target = new Vector3(0, 5, 0),
                FieldOfView = MathHelper.ToRadians(45),
                AspectRatio = GraphicsDevice.Viewport.AspectRatio,
                NearPlaneDistance = 0.1f,
                FarPlaneDistance = 500f
            };

            mechObject = new Mech();
            missileObject = new GameObject();
            
            offset = camera.Position - mechObject.Position;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            texture = Game.Content.Load<Texture2D>("Images/chicken");
            font = Game.Content.Load<SpriteFont>("SpriteFonts/SpriteFont");

            mechObject = new Mech
            {
                Model = Game.Content.Load<Model>("Models/Mech/Mech"),
                Texture = Game.Content.Load<Texture2D>("Models/Mech/Mech5_desert"),
                Scale = 1.0f,
                Spped = 0.05f,
                Position = new Vector3(0.0f, 0.0f, 0.0f),
                Rotation = new Vector3(0.0f, 0.0f, 180.0f),
                FrontDirection = Vector3.TransformNormal(Vector3.UnitY, Matrix.CreateRotationZ(mechObject.Rotation.Z))
                
            };

            missileObject = new GameObject
            {
                Model = Game.Content.Load<Model>("Models/Missile/Missile"),
                Texture = Game.Content.Load<Texture2D>("Models/Missile/MissileTex"),
                Scale = 1.2f,
                Position = new Vector3(5, 10, 10)
            };

            floorVerts = new VertexPositionTexture[6];

            floorVerts[0].Position = new Vector3(-20, -20, 0);
            floorVerts[1].Position = new Vector3(-20, 20, 0);
            floorVerts[2].Position = new Vector3(20, -20, 0);

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 20, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            effect = new BasicEffect(GraphicsDevice);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Game.Content.Unload();
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Input.Update();

            if (Input.IsJustKeyUp(Keys.Enter))
            {
                Game.Components.Remove(this);
            }

            // TODO: Add your update logic here
            missileObject.Rotation += new Vector3(0.0f, 0.2f, 0.0f);

            //正面の単位ベクトルを取得（MathHelper使ってるから重いかも）
            mechObject.FrontDirection = Vector3.TransformNormal(Vector3.UnitY, Matrix.CreateRotationZ(MathHelper.ToRadians(mechObject.Rotation.Z - 180)));

#if __MOBILE__
            //TouchCollection touches = TouchPanel.GetState();
            
            //if(touches.Count != 0)
            //{
            //    if(GraphicsDevice.Viewport.Width / 2 < touches[0].Position.X && touches[0].State == TouchLocationState.Moved)
            //        mechObject.Rotation -= new Vector3(0.0f, 0.5f, 0.0f);

            //    if(GraphicsDevice.Viewport.Width / 2 > touches[0].Position.X && touches[0].State == TouchLocationState.Moved)
            //        mechObject.Rotation += new Vector3(0.0f, 0.5f, 0.0f);
            //}

            if(Input.IsTap())
            {
                mechObject.Rotation -= new Vector3(0.0f, 0.5f, 0.0f);
            }
            
#else
            //向いている方向（正面）に移動
            if (Input.IsKeyDown(Keys.Up)) mechObject.Position += new Vector3(mechObject.Spped, mechObject.Spped, 0.0f) * mechObject.FrontDirection;
            if (Input.IsKeyDown(Keys.Down)) mechObject.Position -= new Vector3(mechObject.Spped, mechObject.Spped, 0.0f) * mechObject.FrontDirection;

            //Mechの回転
            if (Input.IsKeyDown(Keys.Left)) mechObject.Rotation += new Vector3(0.0f, 0.0f, 0.5f);
            if (Input.IsKeyDown(Keys.Right)) mechObject.Rotation -= new Vector3(0.0f, 0.0f, 0.5f);
#endif

            camera.Position = new Vector3(0.0f, -30.0f, 20.0f);
            camera.Target = new Vector3(0.0f, 20.0f, 10.0f);

            camera.RotationTarget(mechObject.Rotation.Z - 180);

            camera.Position += mechObject.Position;
            camera.Target += mechObject.Position;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(0, 0, 256, 512), Color.White);

#if __MOBILE__
            spriteBatch.DrawString(font, "Android Build!!!", new Vector2(256, 512), Color.White);

#else
            spriteBatch.DrawString(font, "Windows Build!!!", new Vector2(256, 512), Color.White);
            spriteBatch.DrawString(font, "Camera.Position:" + camera.Position.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "Camera.Target:" + camera.Target.ToString(), new Vector2(0, 15), Color.White);
            spriteBatch.DrawString(font, "Mech.Rotation:" + mechObject.Rotation.ToString(), new Vector2(0, 30), Color.White);
            spriteBatch.DrawString(font, "Mech.Position:" + mechObject.Position.ToString(), new Vector2(0, 45), Color.White);
            spriteBatch.DrawString(font, "Mech.Front:" + mechObject.FrontDirection.ToString(), new Vector2(0, 60), Color.White);

#endif
            spriteBatch.End();

#region SpriteBatchで変更されたレンダーステートを元に戻す
            //合成方法をアルファブレンドに指定（アルファ合成を行いたいのでOpaqueではない）
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            //深度ステンシルステートを通常に変更
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //規定値をLinearWrapに設定
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
#endregion

            //モデル描画
            missileObject.DrawModel(missileObject.World, camera.View, camera.Projection);
            mechObject.DrawModel(mechObject.World, camera.View, camera.Projection);

            effect.View = camera.View;
            effect.Projection = camera.Projection;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                                                        floorVerts,
                                                        0,
                                                        2);
            }

            base.Draw(gameTime);
        }
    }

    public class Mech : GameObject
    {
        public float Spped { get; set; }
        public Vector3 FrontDirection { get; set; }

        public override void DrawModel(Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    //メッシュ名が"Plane"のメッシュを透過
                    if (mesh.Name == "Plane" && effect.Alpha != 0.0f)
                    {
                        effect.Alpha = 0.0f;
                    }

                    //ライティングの有効化
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    //Androidだとテクスチャが表示されないので読み込む
                    effect.Texture = Texture;

                    //反射とかいろいろ
                    effect.DiffuseColor = Color.Gray.ToVector3();
                    effect.SpecularColor = Color.White.ToVector3();
                    effect.SpecularPower = 50.0f;

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}
