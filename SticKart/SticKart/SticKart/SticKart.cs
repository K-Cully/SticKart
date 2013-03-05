// -----------------------------------------------------------------------
// <copyright file="SticKart.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart
{
    using System;
    using System.Collections.Generic;
    using Audio;
    using Display;
    using Display.Notification;
    using Game;
    using Game.Entities;
    using Game.Level;
    using Input;
    using LevelEditor;
    using Menu;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
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
        /// A value indicating whether to display the colour stream on screen or not. 
        /// </summary>
        private const bool DisplayColourStream = true;

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

        /// <summary>
        /// The game's heads up display.
        /// </summary>
        private HeadsUpDisplay headsUpDisplay;

        /// <summary>
        /// The sprite to display the user's hand position.
        /// </summary>
        private Sprite handSprite;

        /// <summary>
        /// The colour stream renderer.
        /// </summary>
        private ColourStreamRenderer colourStreamRenderer;

        /// <summary>
        /// The rectangle to display the colour stream in.
        /// </summary>
        private Rectangle colourStreamDisplayArea;

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

        /// <summary>
        /// The game's notification manager.
        /// </summary>
        private NotificationManager notificationManager;

        /// <summary>
        /// The game's level editor.
        /// </summary>
        private Editor levelEditor; // TODO: add placement timer to editor

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SticKart"/> class.
        /// </summary>
        public SticKart()
        {
            this.notificationManager = null;
            this.gameState = GameState.InEditor;    // TODO: change back to InMenu
            this.TargetElapsedTime = TimeSpan.FromSeconds(SticKart.FrameTime); 
            this.screenDimensions = new Vector2(1360.0f, 768.0f);
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            this.graphics.PreferredBackBufferWidth = (int)this.screenDimensions.X;
            this.graphics.PreferredBackBufferHeight = (int)this.screenDimensions.Y;
            this.graphics.IsFullScreen = false; // TODO: set to true for release 

            Camera2D.Initialize(this.screenDimensions);
            this.headsUpDisplay = new HeadsUpDisplay(this.screenDimensions);
            this.Content.RootDirectory = "Content";
            this.inputManager = new InputManager(this.screenDimensions, ControlDevice.Kinect, SticKart.DisplayColourStream);
            this.levelManager = new LevelManager(this.screenDimensions, SticKart.FrameTime);

            this.levelEditor = new Editor(this.screenDimensions);

            // TODO: add other menu event handlers.
            this.menuManager = new MenuManager(this.screenDimensions);
            this.menuManager.OnBeginLevelDetected += this.BeginLevel;
            this.menuManager.OnQuitGameDetected += this.QuitGame;
            
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
            this.notificationManager = NotificationManager.Initialize(this.Content, this.spriteBatch, this.screenDimensions);            
            AudioManager.InitializeAndLoad(this.Content);
            PositionInformer.Initialize(this.Content, new Vector2(this.screenDimensions.X / 2.0f, this.screenDimensions.Y / 2.5f), new Vector2(this.screenDimensions.X / 6.0f), 75, true);
            this.menuManager.InitializeAndLoad(this.spriteBatch, this.Content, this.gameSettings);
            this.inputManager.InitializeSpeechEngine(this.menuManager.GetAllSelectableNames());
            this.handSprite.InitializeAndLoad(this.spriteBatch, this.Content, ContentLocations.HandIcon);
            this.headsUpDisplay.InitializeAndLoad(this.spriteBatch, this.Content);
            EntitySettingsLoader.LoadEntitySettings(this.Content);
            this.levelManager.LoadContent(this.Content, this.spriteBatch);
            this.levelEditor.LoadContent(this.spriteBatch, this.Content);
            if (SticKart.DisplayColourStream)
            {
                this.colourStreamRenderer = new ColourStreamRenderer(this.Content, this.GraphicsDevice);
                this.colourStreamDisplayArea = new Rectangle(5 * ((int)this.screenDimensions.X / 6), 2 * ((int)this.screenDimensions.Y / 3), (int)this.screenDimensions.X / 6, (int)this.screenDimensions.Y / 3);
            }

            AudioManager.PlayBackgroundMusic(this.gameState == GameState.InGame);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.gameSettings.Save();
            this.notificationManager.Save();
            this.inputManager.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (this.notificationManager.NotificationsActive)
            {
                this.notificationManager.Update(gameTime, this.inputManager.Update(gameTime, false));
            }
            else
            {
                switch (this.gameState)
                {
                    case GameState.InMenu:
                        this.UpdateMenu(gameTime);
                        break;
                    case GameState.InGame:
                        this.UpdateGame(gameTime);
                        break;
                    case GameState.InEditor:
                        //this.UpdateMenu(gameTime); // TODO: will probably need this
                        this.UpdateEditor(gameTime);
                        break;
                    default:
                        break;
                }
            }

            if (SticKart.DisplayColourStream)
            {
                this.colourStreamRenderer.Update(this.inputManager.ColourData, this.inputManager.ColourFrameSize);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the level editor.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void UpdateEditor(GameTime gameTime)
        {
            Vector2 handPosition = this.inputManager.HandPosition;
            this.levelEditor.Update(handPosition + Camera2D.OffsetPosition);
            if (handPosition.X > this.screenDimensions.X * 0.75f)
            {
                float speed = (handPosition.X - (this.screenDimensions.X * 0.75f)) / (this.screenDimensions.X * 0.00025f);
                //if (Camera2D.OffsetPosition.X + speed < this.levelEditor.MaxLength)
                //{
                    Camera2D.Update(new Vector2(speed, 0.0f), gameTime);
                //}
                //else
                //{

                //}
            }
            else if (handPosition.X < this.screenDimensions.X * 0.25f)
            {
                float speed = (handPosition.X - (this.screenDimensions.X * 0.25f)) / (this.screenDimensions.X * 0.00025f);
                if (Camera2D.OffsetPosition.X > -speed)
                {
                    Camera2D.Update(new Vector2(speed, 0.0f), gameTime);
                }
                else
                {
                    Camera2D.Update(new Vector2(-Camera2D.OffsetPosition.X, 0.0f), gameTime);
                }
            }

            //this.levelEditor.CycleSelection();
            //this.levelEditor.PlatformWidth = this.levelEditor.PlatformWidth + 16.0f;
            //this.levelEditor.PlatformWidth = this.levelEditor.PlatformWidth + -16.0f;
            //this.levelEditor.SaveLevel(true); // TODO: Remove in release
            //this.levelEditor.SaveLevel(false);
            //this.levelEditor.LoadLevel(1);
            //this.levelEditor.CreateNewLevel();
        }

        /// <summary>
        /// Updates the game world while in play.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void UpdateGame(GameTime gameTime)
        {
            this.levelManager.Update(gameTime, this.inputManager.Commands);
            if (this.levelManager.Complete)
            {
                if (this.levelManager.CurrentLevel < this.gameSettings.TotalLevels && this.levelManager.CurrentLevel == this.gameSettings.LevelsUnlocked)
                {
                    this.gameSettings.LevelsUnlocked += 1;
                    this.menuManager.UpdateLevelsUnlocked(this.gameSettings.LevelsUnlocked);
                }

                this.gameState = GameState.InMenu;
                this.menuManager.ActiveMenu = MenuType.LevelComplete;
                this.menuManager.SetLevelCompleteMenuText(this.gameSettings.AddScore(this.levelManager.CurrentLevel, this.levelManager.PlayerScore), this.levelManager.PlayerScore, this.levelManager.GetRating());
                this.levelManager.EndLevel();
                AudioManager.PlayBackgroundMusic(this.gameState == GameState.InGame);
            }
            else
            {
                this.headsUpDisplay.HealthPercentage = this.levelManager.PlayerHealthPercentage;
                this.headsUpDisplay.Score = this.levelManager.PlayerScore;
                this.headsUpDisplay.ActivePowerUp = this.levelManager.PlayerPowerUp;
                if (this.inputManager.Update(gameTime, false))
                {
                    // Commands are available.
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
                    if (this.inputManager.LastVoiceCommand.ToUpperInvariant() == MenuConstants.PauseCommandName)
                    {
                        this.PauseGame();
                    }
                }
            }
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        protected void PauseGame()
        {
            this.levelManager.EndLevel(); // TODO: change to only on exit
            AudioManager.PauseBackgroundMusic();
            this.gameState = GameState.InMenu;
            this.menuManager.ActiveMenu = MenuType.Main; // TODO: Add a resume button or pause menu.
            AudioManager.PlayBackgroundMusic(this.gameState == GameState.InGame);
        }

        /// <summary>
        /// Updates the menu system while the game is not in play.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void UpdateMenu(GameTime gameTime)
        {
            if (this.inputManager.Update(gameTime, true))
            {
                // Commands are available.
                foreach (InputCommand command in this.inputManager.Commands)
                {
                    switch (command)
                    {
                        case InputCommand.Select:
                            this.menuManager.Update(this.menuManager.HighlightedPosition, null, ref this.gameSettings);
                            break;
                        case InputCommand.SelectAt:
                            this.menuManager.Update(this.inputManager.SelectedPosition, null, ref this.gameSettings);
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
                this.menuManager.Update(Vector2.Zero, this.inputManager.LastVoiceCommand, ref this.gameSettings);
            }                
        }

        /// <summary>
        /// Event handler for a begin level event.
        /// </summary>
        /// <param name="value">The value passed from the sender.</param>
        protected void BeginLevel(int value)
        {
            Camera2D.Reset();
            this.inputManager.Reset();
            if (value == 0)
            {
                value = this.levelManager.CurrentLevel + 1;
            }
            else if (value == int.MaxValue)
            {
                value = this.levelManager.CurrentLevel;
            }

            if (value <= this.gameSettings.TotalLevels)
            {
                this.gameState = GameState.InGame;
                this.levelManager.BeginLevel(value, false);
            }
            else
            {
                // TODO: Game complete (roll credits)
            }
        }

        /// <summary>
        /// Event handler for a quit game event.
        /// </summary>
        /// <param name="value">The value passed from the sender.</param>
        protected void QuitGame(bool value)
        {
            AudioManager.StopBackgroundMusic();
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
                    this.headsUpDisplay.Draw();
                    break;
                case GameState.InEditor:
                    this.GraphicsDevice.Clear(Color.GhostWhite);
                    this.levelEditor.Draw();
                    break;
                default:
                    break;
            }
            
            this.notificationManager.Draw();
            if (this.inputManager.PlayerFloorPosition != Vector2.Zero)
            {
                PositionInformer.Draw(this.spriteBatch, this.inputManager.PlayerFloorPosition.Y, -1.25f + (this.inputManager.PlayerFloorPosition.X * 0.75f));
            }

            this.spriteBatch.End();
            if (SticKart.DisplayColourStream)
            {
                this.colourStreamRenderer.Draw(this.spriteBatch, this.colourStreamDisplayArea, 50.0f);
            }

            base.Draw(gameTime);
        }
    }
}
