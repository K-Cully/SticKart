using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using System.Collections.Generic;
using FarseerPhysics.SamplesFramework;
using SticKart.Display;

namespace SticKart
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SticKart : Microsoft.Xna.Framework.Game
    {
        #region enums

        /// <summary>
        /// An enumeration of the possible game states.
        /// </summary>
        enum GameState { InMenu, InGame };

        #endregion

        #region graphics

        Vector2 screenDimensions;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        const float FrameTime = 1.0f / 60.0f;

        Sprite playerSprite; // 64px == 1.8m => 35.56px == 1m

        #endregion

        #region physics

        World physicsWorld;
        Body boundry;
        Body playerBody;
        
        #endregion

        #region misc

        InputManager inputManager;
        GameState gameState;

        #endregion

        /// <summary>
        /// Initalizes an instance of the <see cref="SticKart"/> class.
        /// </summary>
        public SticKart()
        {
            this.gameState = GameState.InGame;
            this.TargetElapsedTime = TimeSpan.FromSeconds(SticKart.FrameTime); 
            this.screenDimensions = new Vector2(1280.0f, 720.0f);
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            this.graphics.PreferredBackBufferWidth = (int)this.screenDimensions.X;
            this.graphics.PreferredBackBufferHeight = (int)this.screenDimensions.Y;
            this.graphics.IsFullScreen = false;
            this.Content.RootDirectory = "Content";
            this.inputManager = new InputManager(this.screenDimensions, InputManager.ControlDevice.Kinect);
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

        // TODO: Remove this boundry or scroll.
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
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            Texture2D playerTexture = this.Content.Load<Texture2D>("Sprites/StickMan__base");
            this.playerSprite = new Sprite(playerTexture);

            this.physicsWorld = new World(ConvertUnits.ToSimUnits(new Vector2(0.0f, 348.8f)));
            this.playerBody = BodyFactory.CreateBody(this.physicsWorld);

            Vertices playerBox = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(playerTexture.Width / 2.0f), ConvertUnits.ToSimUnits(playerTexture.Height / 2.0f));
            PolygonShape playerShape = new PolygonShape(playerBox, 1.25f);
            Fixture playerFixture = playerBody.CreateFixture(playerShape);

            this.playerBody.BodyType = BodyType.Dynamic;
            this.playerBody.Position = ConvertUnits.ToSimUnits(this.screenDimensions / 2.0f);
            this.playerBody.Restitution = 0.125f;

            this.boundry = BodyFactory.CreateLoopShape(this.physicsWorld, this.GetBounds());
            this.boundry.CollisionCategories = Category.All;
            this.boundry.CollidesWith = Category.All;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.inputManager.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (this.gameState)
            {
                case GameState.InMenu:
                    this.UpdateMenu(gameTime);
                    break;
                case GameState.InGame:
                    this.UpdateGame(gameTime);
                    break;
                default:
                    break;
            }            

            this.physicsWorld.Step(MathHelper.Min((float)gameTime.ElapsedGameTime.TotalSeconds, SticKart.FrameTime));
            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the game world while in play.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateGame(GameTime gameTime)
        {
            if (this.inputManager.Update()) // Commands are available.
            {
                foreach (InputManager.Command command in this.inputManager.Commands)
                {
                    switch (command)
                    {
                        case InputManager.Command.Left: // TODO: remove
                            this.playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(-5000.0f, 0.0f)));
                            break;
                        case InputManager.Command.Jump:
                            this.playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(0.0f, -400.0f)));
                            break;
                        case InputManager.Command.Crouch:
                            this.playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(0.0f, 400.0f)));
                            break;
                        case InputManager.Command.Run:
                            this.playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(5000.0f, 0.0f)));
                            break;
                        case InputManager.Command.Pause:
                            this.gameState = GameState.InMenu;
                            break;
                        case InputManager.Command.Exit:
                            this.Exit();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the menu system while the game is not in play.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateMenu(GameTime gameTime)
        {
            if (this.inputManager.Update()) // Commands are available.
            {
                foreach (InputManager.Command command in this.inputManager.Commands)
                {
                    switch (command)
                    {
                        case InputManager.Command.Up:
                            break;
                        case InputManager.Command.Down:
                            break;
                        case InputManager.Command.Left:
                            break;
                        case InputManager.Command.Right:
                            break;                        
                        case InputManager.Command.Select:
                            break;
                        case InputManager.Command.SelectAt:
                            break;
                        case InputManager.Command.Pause:
                            break;
                        case InputManager.Command.Exit:
                            this.Exit();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();
            Sprite.Draw(this.spriteBatch, this.playerSprite, ConvertUnits.ToDisplayUnits(this.playerBody.Position), this.playerBody.Rotation);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
