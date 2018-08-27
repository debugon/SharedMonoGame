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

        private Model attinyModel;
        private Model atmegaModel;

        private Matrix[] transforms;

        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 10, 20), new Vector3(0, 5, 0), Vector3.UnitY);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 500f);

        private Character mechCharacter;

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
            mechCharacter = new Character(Content, "Models/Mech/mech", 1.0f);
            mechCharacter.Rotation = new Vector3(-90, 0, 0);

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

            attinyModel = Content.Load<Model>("Models/Microcomputer/attiny85");
            atmegaModel = Content.Load<Model>("Models/Microcomputer/atmega328");
            transforms = new Matrix[atmegaModel.Bones.Count];
            atmegaModel.CopyAbsoluteBoneTransformsTo(transforms);

            
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
            world = mechCharacter.CreateWorldMatrix();


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

            foreach (ModelMesh mesh in mechCharacter.Model.Meshes)
            {
                System.Diagnostics.Debug.WriteLine(mesh.Name);
                
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //Hide Plane Mesh
                    if(mesh.Name == "Plane")
                    {
                        effect.Alpha = 0.0f;
                    }

                    effect.EnableDefaultLighting();
                    effect.DiffuseColor = Color.Gray.ToVector3();
                    effect.SpecularColor = Color.White.ToVector3();
                    effect.SpecularPower = 50.0f;

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;

                }
                mesh.Draw();

            }


            base.Draw(gameTime);
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
        
        
    }

   
    public class Character
    {
        public Model Model { set; get; }

        public float Scale { set; get; }
        public Vector3 Rotation { set; get; }
        public Vector3 Position { set; get; }

        public Character() { }

        public Character(Microsoft.Xna.Framework.Content.ContentManager contentManager,string filePath, float modelScale)
        {
            Model = contentManager.Load<Model>(filePath);
            Scale = modelScale;
        }

        public Matrix CreateWorldMatrix()
        {
            return Matrix.CreateScale(Scale)
                * Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(Rotation.X))
                * Matrix.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(Rotation.Y))
                * Matrix.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(Rotation.Z))
                * Matrix.CreateTranslation(Position);
        }
        
    }
}
