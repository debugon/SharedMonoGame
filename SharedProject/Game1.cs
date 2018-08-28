using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SharedProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D texture;
        private SpriteFont font;

        private Camera camera;

        private Mech mechObject;
        private Object missileObject;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if __MOBILE__
            //フルスクリーンにしないとステータスバーが表示されてしまうので注意
            graphics.IsFullScreen = true;
#else
            graphics.IsFullScreen = false;
#endif

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            //WindowsPhoneの横画面処理
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
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
            missileObject = new Object();

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
            texture = Content.Load<Texture2D>("Images/chicken");
            font = Content.Load<SpriteFont>("SpriteFonts/SpriteFont");

            mechObject = new Mech
            {
                Model = Content.Load<Model>("Models/Mech/mech"),
                Texture = Content.Load<Texture2D>("Models/Mech/Mech5_desert"),
                Scale = 1.0f,
                Rotation = new Vector3(-90, 0, 0)
            };

            missileObject = new Object
            {
                Model = Content.Load<Model>("Models/Missile/missile"),
                Texture = Content.Load<Texture2D>("Models/Missile/uvmap"),
                Scale = 0.05f
            };
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
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
            mechObject.DrawModel(camera.View, camera.Projection);
            missileObject.DrawModel(camera.View, camera.Projection);

            base.Draw(gameTime);
        }
        
    }

   
    public partial class Object
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

        public Object() { }

        public Object(Microsoft.Xna.Framework.Content.ContentManager contentManager,string filePath, float modelScale)
        {
            Model = contentManager.Load<Model>(filePath);
            Scale = modelScale;
        }
        
    }

    public partial class Object
    {
        //モデル描画処理
        public virtual void DrawModel(Matrix view, Matrix projection)
        {
            Matrix world = this.World;

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

    public class Mech : Object
    {
        public override void DrawModel(Matrix view, Matrix projection)
        {
            Matrix world = this.World;

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
