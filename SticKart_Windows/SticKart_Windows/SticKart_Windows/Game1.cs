using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using System.Collections.Generic;
using FarseerPhysics.SamplesFramework;

namespace SticKart_Windows
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region graphics

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite playerSprite;

        #endregion

        #region physics

        World physicsWorld;
        Body boundry;
        Body playerBody;
        
        #endregion

        #region input

        // TODO: Put Kinect interface here
        KeyboardState keyboardState;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        // TODO: Remove this boundry.
        private Vertices GetBounds()
        {
            float height = ConvertUnits.ToSimUnits(this.GraphicsDevice.Viewport.Height);
            float width = ConvertUnits.ToSimUnits(this.GraphicsDevice.Viewport.Width);

            Vertices bounds = new Vertices(4);
            bounds.Add(new Vector2(0.0f, 0.0f));
            bounds.Add(new Vector2(width, 0.0f));
            bounds.Add(new Vector2(width, height));
            bounds.Add(new Vector2(0.0f, height));

            return bounds;
        }
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Viewport viewport = GraphicsDevice.Viewport;
            Texture2D playerTexture = Content.Load<Texture2D>("Sprites/StickMan__base");
            playerSprite = new Sprite(playerTexture);

            physicsWorld = new World(Vector2.Zero);
            playerBody = BodyFactory.CreateBody(physicsWorld);

            Vertices playerBox = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(playerTexture.Width / 2.0f), ConvertUnits.ToSimUnits(playerTexture.Height / 2.0f));
            PolygonShape playerShape = new PolygonShape( playerBox, 5.0f);
            Fixture playerFixture = playerBody.CreateFixture(playerShape);

            playerBody.BodyType = BodyType.Dynamic;
            playerBody.Position = ConvertUnits.ToSimUnits(new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f));
            playerBody.Restitution = 0.6f;

            boundry = BodyFactory.CreateLoopShape(physicsWorld, GetBounds());
            boundry.CollisionCategories = Category.All;
            boundry.CollidesWith = Category.All;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: put in interface class.
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W))
            {
                playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(0.0f, -50.0f)));
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(0.0f, 50.0f)));
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(-50.0f, 0.0f)));
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(50.0f, 0.0f)));
            }

            physicsWorld.Step(MathHelper.Min((float)gameTime.ElapsedGameTime.TotalSeconds, 1.0f / 30.0f)); //TODO: Set and store framerate.

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            Sprite.Draw(spriteBatch, playerSprite, ConvertUnits.ToDisplayUnits(playerBody.Position), playerBody.Rotation);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
