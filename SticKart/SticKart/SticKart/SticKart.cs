namespace SticKart
{
    using System;
    using System.Collections.Generic;
    using Display;
    using Game;
    using Game.Entities;
    using Game.Level;
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
        
        #region game

        /// <summary>
        /// The game's input manager.
        /// </summary>
        private InputManager inputManager;

        /// <summary>
        /// The game's settings.
        /// </summary>
        private GameSettings gameSettings;

        /// <summary>
        /// The current game state.
        /// </summary>
        private GameState gameState;

        /// <summary>
        /// The game's menu system manager.
        /// </summary>
        private MenuManager menuManager;

        /// <summary>
        /// The game's level manager.
        /// </summary>
        private LevelManager levelManager;

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
            this.graphics.IsFullScreen = false; // TODO: set to true for release 
            this.Content.RootDirectory = "Content";
            this.inputManager = new InputManager(this.screenDimensions, ControlDevice.Kinect);
            this.levelManager = new LevelManager(this.screenDimensions, SticKart.FrameTime);

            this.menuManager = new MenuManager(this.screenDimensions);
            this.menuManager.OnBeginLevelDetected += this.BeginLevel;
            this.menuManager.OnQuitGameDetected += this.QuitGame;
            // TODO: add other menu event handlers.

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
                
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.gameSettings = GameSettings.Load();

            this.menuManager.InitializeAndLoad(this.spriteBatch, this.Content, this.gameSettings);
            this.handSprite.InitializeAndLoad(this.spriteBatch, this.Content, ContentLocations.HandIcon);

            EntitySettingsLoader.LoadEntitySettings(this.Content);
            this.levelManager.LoadContent(this.Content, this.spriteBatch);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.gameSettings.Save();
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

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the game world while in play.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void UpdateGame(GameTime gameTime)
        {
            this.levelManager.Update(gameTime, this.inputManager.Commands);

            if (this.inputManager.Update()) // Commands are available.
            {
                foreach (InputCommand command in this.inputManager.Commands)
                {
                    switch (command)
                    {
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
            this.menuManager.ActiveMenu = MenuType.Main; // TODO: Add a resume button or pause menu.
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
                            this.menuManager.Update(this.menuManager.HighlightedPosition, null, this.gameSettings);
                            break;
                        case InputCommand.SelectAt:
                            this.menuManager.Update(this.inputManager.SelectedPosition, null, this.gameSettings);
                            break;
                        case InputCommand.NextPage:
                            this.menuManager.FlipPage(true);
                            break;
                        case InputCommand.PreviousPage:
                            this.menuManager.FlipPage(false);
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
                this.menuManager.Update(Vector2.Zero, this.inputManager.LastVoiceCommand, this.gameSettings);
            }                
        }

        /// <summary>
        /// Event handler for a begin level event.
        /// </summary>
        /// <param name="value">The value passed from the sender.</param>
        protected void BeginLevel(int value)
        {
            // TODO: refine.
            this.gameState = GameState.InGame;
            this.levelManager.BeginLevel(value, false);
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
            this.spriteBatch.Begin();
            switch (this.gameState)
            {
                case GameState.InMenu:
                    this.GraphicsDevice.Clear(Color.CornflowerBlue);
                    this.menuManager.Draw();
                    if (this.inputManager.HandPosition == Vector2.Zero)
                    {
                        Sprite.Draw(this.handSprite, this.menuManager.HighlightedDrawingPosition, 0.0f);
                    }
                    else
                    {
                        Sprite.Draw(this.handSprite, this.inputManager.HandPosition, 0.0f);
                    }

                    break;
                case GameState.InGame:
                    this.GraphicsDevice.Clear(Color.GhostWhite);
                    this.levelManager.Draw();
                    break;
                default:
                    break;
            }

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
