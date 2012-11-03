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
using SticKart.Menu;

namespace SticKart
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SticKart : Microsoft.Xna.Framework.Game
    {
        //TODO: Note scaling factor: 64px == 1.8m => 35.56px == 1m

        #region enums

        /// <summary>
        /// An enumeration of the possible game states.
        /// </summary>
        enum GameState { InMenu, InGame };

        #endregion

        #region graphics

        private Vector2 screenDimensions;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private const float FrameTime = 1.0f / 60.0f;

        private Sprite playerSprite;
        private Sprite handSprite;

        #endregion

        #region physics

        private World physicsWorld;
        private Body boundry;
        private Body playerBody;
        
        #endregion

        #region misc

        private InputManager inputManager;
        private GameState gameState;
        private MenuManager menuManager;

        #endregion

        /// <summary>
        /// Initalizes an instance of the <see cref="SticKart"/> class.
        /// </summary>
        public SticKart()
        {
            this.gameState = GameState.InMenu;
            this.TargetElapsedTime = TimeSpan.FromSeconds(SticKart.FrameTime); 
            this.screenDimensions = new Vector2(1280.0f, 720.0f);
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            this.graphics.PreferredBackBufferWidth = (int)this.screenDimensions.X;
            this.graphics.PreferredBackBufferHeight = (int)this.screenDimensions.Y;
            this.graphics.IsFullScreen = false;
            this.Content.RootDirectory = "Content";
            this.inputManager = new InputManager(this.screenDimensions, InputManager.ControlDevice.Kinect);

            this.menuManager = new MenuManager(this.screenDimensions);
            this.menuManager.OnBeginLevelDetected += this.BeginLevel;
            this.menuManager.OnQuitGameDetected += this.QuitGame;
            // TODO: add other menu event handlers.

            this.playerSprite = new Sprite();
            this.handSprite = new Sprite();
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

            this.menuManager.InitalizeAndLoad(this.spriteBatch, this.Content);

            this.handSprite.InitalizeAndLoad(this.spriteBatch, this.Content, ContentLocations.HandIcon);
            this.playerSprite.InitalizeAndLoad(this.spriteBatch, this.Content, ContentLocations.StickManStanding);

            this.physicsWorld = new World(ConvertUnits.ToSimUnits(new Vector2(0.0f, 348.8f)));
            this.playerBody = BodyFactory.CreateBody(this.physicsWorld);

            Vertices playerBox = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(this.playerSprite.Width / 2.0f), ConvertUnits.ToSimUnits(this.playerSprite.Height / 2.0f));
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
                    this.physicsWorld.Step(MathHelper.Min((float)gameTime.ElapsedGameTime.TotalSeconds, SticKart.FrameTime));
                    break;
                default:
                    break;
            }            

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the game world while in play.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void UpdateGame(GameTime gameTime)
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
                            this.playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(0.0f, -5000.0f)));
                            break;
                        case InputManager.Command.Crouch:
                            this.playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(0.0f, 5000.0f)));
                            break;
                        case InputManager.Command.Run:
                            this.playerBody.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(5000.0f, 0.0f)));
                            break;
                        case InputManager.Command.Pause:
                            this.PauseGame();
                            break;
                        case InputManager.Command.Exit:
                            this.Exit();
                            break;
                        default:
                            break;
                    }
                }
            }

            if (this.inputManager.VoiceCommandAvailable)
            {
                if (this.inputManager.LastVoiceCommand.ToUpperInvariant() == SelectableNames.PauseCommandName)
                {
                    this.PauseGame();
                }
            } 
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        private void PauseGame()
        {
            this.gameState = GameState.InMenu;
            this.menuManager.ActiveMenu = MenuType.Main; // TODO add a resume button.
        }

        /// <summary>
        /// Updates the menu system while the game is not in play.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void UpdateMenu(GameTime gameTime)
        {
            if (this.inputManager.Update()) // Commands are available.
            {
                foreach (InputManager.Command command in this.inputManager.Commands)
                {
                    switch (command)
                    {                 
                        case InputManager.Command.Select:
                            break;
                        case InputManager.Command.SelectAt:
                            this.menuManager.Update(this.inputManager.SelectedPosition, null);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (this.inputManager.VoiceCommandAvailable)
            {
                this.menuManager.Update(Vector2.Zero, this.inputManager.LastVoiceCommand);
            }                
        }

        /// <summary>
        /// Event handler for a begin level event.
        /// </summary>
        /// <param name="value">The value passed from the sender.</param>
        protected void BeginLevel(bool value)
        {
            // TODO: refine.
            this.gameState = GameState.InGame;
        }

        /// <summary>
        /// Event handler for a quit game event.
        /// </summary>
        /// <param name="value">The value passed from the sender.</param>
        protected void QuitGame(bool value)
        {
            // TODO: refine.
            this.Exit();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();
            switch (this.gameState)
            {
                case GameState.InMenu:
                    this.menuManager.Draw();
                    Sprite.Draw(this.handSprite, this.inputManager.HandPosition, 0.0f);
                    break;
                case GameState.InGame:
                    Sprite.Draw(this.playerSprite, ConvertUnits.ToDisplayUnits(this.playerBody.Position), this.playerBody.Rotation);
                    break;
                default:
                    break;
            }
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
