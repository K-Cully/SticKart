namespace SticKart
{
    using System;
    using System.Collections.Generic;
    using Display;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Game.Entities;
    using Input;
    using Menu;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// An enumeration of the possible game states.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// The menu is active.
        /// </summary>
        InMenu,

        /// <summary>
        /// The game is in play.
        /// </summary>
        InGame
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SticKart : Microsoft.Xna.Framework.Game
    {
        // TODO: Note scaling factor: 64px == 1.8m => 35.56px == 1m
        
        #region graphics

        /// <summary>
        /// The time between frames.
        /// </summary>
        private const float FrameTime = 1.0f / 60.0f;

        /// <summary>
        /// The dimensions of the viewport.
        /// </summary>
        private Vector2 screenDimensions;

        /// <summary>
        /// The graphics device manager used by the game.
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// The sprite batch used by the game.
        /// </summary>
        private SpriteBatch spriteBatch;
        
        // TODO: Place in menu?
        private Sprite handSprite;

        #endregion

        #region physics

        /// <summary>
        /// The physics game world.
        /// </summary>
        private World physicsWorld;

        // TODO: remove once level implemented.
        private Body boundry;
                
        #endregion

        #region entities

        private StickMan stickman;

        #endregion

        #region misc

        /// <summary>
        /// The game's input manager.
        /// </summary>
        private InputManager inputManager;

        /// <summary>
        /// The current game state.
        /// </summary>
        private GameState gameState;

        /// <summary>
        /// The game's menu system manager.
        /// </summary>
        private MenuManager menuManager;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SticKart"/> class.
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
            this.inputManager = new InputManager(this.screenDimensions, ControlDevice.Kinect);

            this.menuManager = new MenuManager(this.screenDimensions);
            this.menuManager.OnBeginLevelDetected += this.BeginLevel;
            this.menuManager.OnQuitGameDetected += this.QuitGame;
            // TODO: add other menu event handlers.

            this.stickman = null;
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
        protected Vertices GetBounds()
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

            this.menuManager.InitializeAndLoad(this.spriteBatch, this.Content);

            this.handSprite.InitializeAndLoad(this.spriteBatch, this.Content, ContentLocations.HandIcon);
            
            this.physicsWorld = new World(ConvertUnits.ToSimUnits(new Vector2(0.0f, 348.8f)));

            this.stickman = new StickMan(ref this.physicsWorld, 100.0f, 100, -1000.0f, this.spriteBatch, this.Content);
            this.stickman.Position = this.screenDimensions / 2.0f;

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
            this.stickman.Update(gameTime);
            if (this.inputManager.Update()) // Commands are available.
            {
                foreach (InputCommand command in this.inputManager.Commands)
                {
                    switch (command)
                    {
                        case InputCommand.Jump:
                            this.stickman.Jump();
                            break;
                        case InputCommand.Crouch:
                            this.stickman.Crouch();
                            break;
                        case InputCommand.Run:
                            this.stickman.Run();
                            break;
                        case InputCommand.Pause:
                            this.PauseGame();
                            break;
                        case InputCommand.Exit:
                            this.PauseGame();
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
        protected void PauseGame()
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
                foreach (InputCommand command in this.inputManager.Commands)
                {
                    switch (command)
                    {                 
                        case InputCommand.Select:
                            this.menuManager.Update(this.menuManager.HighlightedPosition, null);
                            break;
                        case InputCommand.SelectAt:
                            this.menuManager.Update(this.inputManager.SelectedPosition, null);
                            break;
                        case InputCommand.Up:
                            this.menuManager.MoveSelectionUp();
                            break;
                        case InputCommand.Down:
                            this.menuManager.MoveSelectionDown();
                            break;
                        case InputCommand.Left:
                            this.menuManager.MoveSelectionLeft();
                            break;
                        case InputCommand.Right:
                            this.menuManager.MoveSelectionRight();
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
                    if (this.inputManager.HandPosition == Vector2.Zero)
                    {
                        Sprite.Draw(this.handSprite, this.menuManager.HighlightedPosition, 0.0f);
                    }
                    else
                    {
                        Sprite.Draw(this.handSprite, this.inputManager.HandPosition, 0.0f);
                    }

                    break;
                case GameState.InGame:
                    this.stickman.Draw();
                    break;
                default:
                    break;
            }

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
