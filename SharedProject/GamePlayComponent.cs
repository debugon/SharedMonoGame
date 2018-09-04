using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SharedProject
{
    public class GamePlayComponent : DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        private Texture2D texture;
        private SpriteFont font;

        private Camera camera;

        private Mech mechObject;
        private GameObject missileObject;

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
                Rotation = new Vector3(-90, 0, 0)
            };

            missileObject = new GameObject
            {
                Model = Game.Content.Load<Model>("Models/Missile/Missile"),
                Texture = Game.Content.Load<Texture2D>("Models/Missile/MissileTex"),
                Scale = 1.2f,
                Position = new Vector3(5, 10, 10)
            };

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
            mechObject.Rotation += new Vector3(0.0f, 0.2f, 0.0f);
            
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

            base.Draw(gameTime);
        }
    }

    public class Mech : GameObject
    {
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
