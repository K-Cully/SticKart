// -----------------------------------------------------------------------
// <copyright file="Editor.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.LevelEditor
{
    using System;
    using System.Collections.Generic;
    using Display;
    using Game.Entities;
    using Input;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a level editor for the game.
    /// </summary>
    public class Editor
    {
        #region constants

        /// <summary>
        /// The maximum angle change between two floor edges.
        /// </summary>
        private const float MaxFloorAngleDeviation = 0.1f;

        /// <summary>
        /// The maximum angle of any floor edge.
        /// </summary>
        private const float MaxFloorAngle = 0.85f;

        /// <summary>
        /// The delay to apply between placing two items.
        /// </summary>
        private const float TimeBetweenPlacments = 0.3f;

        /// <summary>
        /// The maximum length of a level.
        /// </summary>
        private const float MaxLength = 2000.0f;

        /// <summary>
        /// The length of a small platform, in pixels.
        /// </summary>
        private const float SmallPlatformLength = 128.0f;

        /// <summary>
        /// The length of the largest platform, in pixels.
        /// </summary>
        private const float MaxPlatformLength = SmallPlatformLength * 4.0f;

        #endregion

        #region sprites

        /// <summary>
        /// A switch sprite.
        /// </summary>
        private Sprite switchSprite;

        /// <summary>
        /// A cart sprite.
        /// </summary>
        private Sprite cartSprite;

        /// <summary>
        /// A platform sprite.
        /// </summary>
        private Sprite platformSprite;

        /// <summary>
        /// An edge sprite.
        /// </summary>
        private Sprite edgeSprite;

        /// <summary>
        /// The start position sprite.
        /// </summary>
        private Sprite startSprite;

        /// <summary>
        /// The exit sprite.
        /// </summary>
        private Sprite exitSprite;

        /// <summary>
        /// The jump power up sprite.
        /// </summary>
        private Sprite jumpSprite;

        /// <summary>
        /// The speed power up sprite.
        /// </summary>
        private Sprite speedSprite;

        /// <summary>
        /// The invincible power up sprite.
        /// </summary>
        private Sprite invincibleSprite;

        /// <summary>
        /// The health power up sprite.
        /// </summary>
        private Sprite healthSprite;

        /// <summary>
        /// The rock sprite.
        /// </summary>
        private Sprite rockSprite;

        /// <summary>
        /// The fire sprite.
        /// </summary>
        private Sprite fireSprite;

        /// <summary>
        /// The spike sprite.
        /// </summary>
        private Sprite spikeSprite;

        /// <summary>
        /// The coin sprite.
        /// </summary>
        private Sprite coinSprite;

        /// <summary>
        /// The ruby sprite.
        /// </summary>
        private Sprite rubySprite;

        /// <summary>
        /// The diamond sprite.
        /// </summary>
        private Sprite diamondSprite;

        #endregion

        #region private_variables

        /// <summary>
        /// The level being edited.
        /// </summary>
        private Level levelToEdit;

        /// <summary>
        /// The dimensions of the game's viewport.
        /// </summary>
        private Vector2 screenDimensions;

        /// <summary>
        /// The width of the next platform.
        /// </summary>
        private float platformWidth;

        /// <summary>
        /// The position of the last floor point.
        /// </summary>
        private Vector2 lastFloorPoint;

        /// <summary>
        /// The position of the current floor point.
        /// </summary>
        private Vector2 currentFloorPoint;

        /// <summary>
        /// The angle of the last floor edge.
        /// </summary>
        private float lastFloorAngle;

        /// <summary>
        /// The time since the last object was placed.
        /// </summary>
        private float timeSinceLastPlace;
        
        /// <summary>
        /// The current position of the user's cursor.
        /// </summary>
        private Vector2 cursorPosition;

        /// <summary>
        /// The current level number.
        /// </summary>
        private int currentLevelNumber;
        
        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Editor"/> class.
        /// </summary>
        /// <param name="screenDimensions">The dimensions of the game area.</param>
        public Editor(Vector2 screenDimensions)
        {
            this.screenDimensions = screenDimensions;
            this.LevelsCreated = 0;
            this.currentLevelNumber = 1;
            this.cursorPosition = Vector2.Zero;
            this.lastFloorPoint = Vector2.Zero;
            this.currentFloorPoint = Vector2.Zero;
            this.lastFloorAngle = 0.0f;
            this.EntitySelected = ModifiableEntity.Floor;
            this.levelToEdit = new Level(screenDimensions);
            this.platformSprite = new Sprite();
            this.edgeSprite = new Sprite();
            this.startSprite = new Sprite();
            this.exitSprite = new Sprite();
            this.invincibleSprite = new Sprite();
            this.jumpSprite = new Sprite();
            this.speedSprite = new Sprite();
            this.healthSprite = new Sprite();
            this.fireSprite = new Sprite();
            this.spikeSprite = new Sprite();
            this.rockSprite = new Sprite();
            this.coinSprite = new Sprite();
            this.diamondSprite = new Sprite();
            this.rubySprite = new Sprite();
            this.switchSprite = new Sprite();
            this.cartSprite = new Sprite();
            this.platformWidth = Editor.SmallPlatformLength;
            this.timeSinceLastPlace = Editor.TimeBetweenPlacments;
            this.CurrentPositionValid = true;
        }

        #endregion

        #region public_accessors

        /// <summary>
        /// Gets or sets the number of levels created.
        /// </summary>
        public int LevelsCreated { get; set; }

        /// <summary>
        /// Gets or sets the currently selected entity to modify. 
        /// </summary>
        public ModifiableEntity EntitySelected { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current item placement position is valid or not.
        /// </summary>
        public bool CurrentPositionValid { get; private set; }

        /// <summary>
        /// Gets or sets the platform width.
        /// </summary>
        public float PlatformWidth
        {
            get
            {
                return this.platformWidth;
            }

            set
            {
                if (value < this.platformSprite.Width)
                {
                    this.platformWidth = this.platformSprite.Width;
                }
                else
                {
                    this.platformWidth = value;
                }
            }
        }
        
        #endregion

        #region content_loading

        /// <summary>
        /// Loads any resources used by a level.
        /// </summary>
        /// <param name="spriteBatch">the game's sprite batch.</param>
        /// <param name="contentManager">The game's content manager.</param>
        public void LoadContent(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.levelToEdit.LoadContent(spriteBatch, contentManager);
            this.cartSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.CartFull);
            this.switchSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.SwitchStatic);
            this.platformSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Platform);
            this.platformWidth = this.platformSprite.Width;
            this.edgeSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Floor);
            this.startSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.StickManSubPath + EntityConstants.StickManStanding);
            this.exitSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Exit);
            this.invincibleSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.InvincibleName);
            this.jumpSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.JumpName);
            this.speedSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.SpeedName);
            this.healthSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.HealthName);
            this.fireSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.FireName);
            this.spikeSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.SpikeName);
            this.rockSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.RockName);
            this.coinSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.CoinName);
            this.diamondSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.DiamondName);
            this.rubySprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.RubyName);
        }

        #endregion
        
        #region insertion_and_deletion

        /// <summary>
        /// Adds the currently selected element to the level.
        /// </summary>
        public void AddSelectedElement()
        {
            switch (this.EntitySelected)
            {
                case ModifiableEntity.Floor:                    
                    this.levelToEdit.AddFloorPoint(this.currentFloorPoint);
                    Vector2 direction = this.currentFloorPoint - this.lastFloorPoint;
                    if (this.lastFloorPoint == Vector2.Zero)
                    {
                        direction = Vector2.UnitX;
                    }

                    direction.Normalize();
                    this.lastFloorAngle = (float)Math.Asin(direction.Y);
                    this.lastFloorPoint = this.currentFloorPoint;
                    break;
                case ModifiableEntity.StartPosition:
                    this.levelToEdit.StartPosition = this.cursorPosition;
                    break;
                case ModifiableEntity.ExitPosition:
                    this.levelToEdit.ExitPosition = this.cursorPosition;
                    break;
                case ModifiableEntity.Platform:
                    this.levelToEdit.AddPlatform(this.platformWidth, this.cursorPosition);
                    break;
                case ModifiableEntity.Invincible:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.InvincibleName, this.cursorPosition, new Vector2(this.invincibleSprite.Width, this.invincibleSprite.Height));
                    break;
                case ModifiableEntity.Speed:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.SpeedName, this.cursorPosition, new Vector2(this.speedSprite.Width, this.speedSprite.Height));
                    break;
                case ModifiableEntity.Jump:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.JumpName, this.cursorPosition, new Vector2(this.jumpSprite.Width, this.jumpSprite.Height));
                    break;
                case ModifiableEntity.Health:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.HealthName, this.cursorPosition, new Vector2(this.healthSprite.Width, this.healthSprite.Height));
                    break;
                case ModifiableEntity.Fire:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.FireName, this.cursorPosition, new Vector2(this.fireSprite.Width, this.fireSprite.Height));
                    break;
                case ModifiableEntity.Rock:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.RockName, this.cursorPosition, new Vector2(this.rockSprite.Width, this.rockSprite.Height));
                    break;
                case ModifiableEntity.Spike:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.SpikeName, this.cursorPosition, new Vector2(this.spikeSprite.Width, this.spikeSprite.Height));
                    break;
                case ModifiableEntity.Cart:
                    this.EntitySelected = ModifiableEntity.Switch;
                    this.levelToEdit.AddCart(this.cursorPosition, new Vector2(this.cartSprite.Width, this.cartSprite.Height));
                    break;
                case ModifiableEntity.Switch:
                    this.EntitySelected = ModifiableEntity.Cart;
                    this.levelToEdit.AddSwitch(this.cursorPosition, new Vector2(this.switchSprite.Width, this.switchSprite.Height));
                    break;
                case ModifiableEntity.Coin:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.CoinName, this.cursorPosition, new Vector2(this.coinSprite.Width, this.coinSprite.Height));
                    break;
                case ModifiableEntity.Ruby:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.RubyName, this.cursorPosition, new Vector2(this.rubySprite.Width, this.rubySprite.Height));
                    break;
                case ModifiableEntity.Diamond:
                    this.levelToEdit.AddInteractiveEntity(EntityConstants.DiamondName, this.cursorPosition, new Vector2(this.diamondSprite.Width, this.diamondSprite.Height));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Removes the last entity of the same type as the entity selected. 
        /// </summary>
        public void RemoveSelectedElement()
        {
            switch (this.EntitySelected)
            {
                case ModifiableEntity.Floor:
                    this.lastFloorPoint = this.levelToEdit.RemoveLastFloorPoint();
                    this.lastFloorAngle = this.levelToEdit.GetLastFloorEdgeAngle();
                    break;
                case ModifiableEntity.StartPosition:
                    this.levelToEdit.StartPosition = Vector2.Zero;
                    break;
                case ModifiableEntity.ExitPosition:
                    this.levelToEdit.ExitPosition = Vector2.Zero;
                    break;
                case ModifiableEntity.Platform:
                    this.levelToEdit.RemoveLastPlatform();
                    break;
                case ModifiableEntity.Cart:
                    this.levelToEdit.RemoveCartAndSwitch();
                    break;
                case ModifiableEntity.Switch:
                    this.levelToEdit.RemoveCartAndSwitch();
                    this.EntitySelected = ModifiableEntity.Cart;
                    break;
                default:
                    this.levelToEdit.RemoveLastInteractiveEntity();
                    break;
            }
        }

        /// <summary>
        /// Changes the main type of entity being placed.
        /// </summary>
        /// <param name="subTypeValue">The entity sub type value.</param>
        public void ChangeEntityType(int subTypeValue)
        {
            if (this.EntitySelected == ModifiableEntity.Switch)
            {
                this.levelToEdit.RemoveLastInteractiveEntity();
            }

            switch (subTypeValue)
            {
                case 0:
                    this.EntitySelected = ModifiableEntity.Floor;
                    break;
                case 1:
                    this.EntitySelected = ModifiableEntity.Platform;
                    this.platformWidth = Editor.SmallPlatformLength;
                    break;
                case 2:
                    this.EntitySelected = ModifiableEntity.StartPosition;
                    break;
                case 3:
                    this.EntitySelected = ModifiableEntity.Coin;
                    break;
                case 4:
                    this.EntitySelected = ModifiableEntity.Rock;
                    break;
                case 5:
                    this.EntitySelected = ModifiableEntity.Health;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Moves the selected entity to the next in the list.
        /// </summary>
        public void CycleSelection()
        {
            switch (this.EntitySelected)
            {
                case ModifiableEntity.Floor:
                    break;
                case ModifiableEntity.Platform:
                    if (this.platformWidth < Editor.MaxPlatformLength)
                    {
                        this.platformWidth += Editor.SmallPlatformLength;
                    }
                    else
                    {
                        this.platformWidth = Editor.SmallPlatformLength;
                    }

                    break;
                case ModifiableEntity.StartPosition:
                    this.EntitySelected = ModifiableEntity.ExitPosition;
                    break;
                case ModifiableEntity.ExitPosition:
                    this.EntitySelected = ModifiableEntity.Cart;
                    break;
                case ModifiableEntity.Cart:
                    this.EntitySelected = ModifiableEntity.StartPosition;
                    break;
                case ModifiableEntity.Invincible:
                    this.EntitySelected = ModifiableEntity.Speed;
                    break;
                case ModifiableEntity.Speed:
                    this.EntitySelected = ModifiableEntity.Jump;
                    break;
                case ModifiableEntity.Jump:
                    this.EntitySelected = ModifiableEntity.Health;
                    break;
                case ModifiableEntity.Health:
                    this.EntitySelected = ModifiableEntity.Invincible;
                    break;
                case ModifiableEntity.Coin:
                    this.EntitySelected = ModifiableEntity.Ruby;
                    break;
                case ModifiableEntity.Ruby:
                    this.EntitySelected = ModifiableEntity.Diamond;
                    break;
                case ModifiableEntity.Diamond:
                    this.EntitySelected = ModifiableEntity.Coin;
                    break;
                case ModifiableEntity.Fire:
                    this.EntitySelected = ModifiableEntity.Rock;
                    break;
                case ModifiableEntity.Rock:
                    this.EntitySelected = ModifiableEntity.Spike;
                    break;
                case ModifiableEntity.Spike:
                    this.EntitySelected = ModifiableEntity.Fire;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region loading_and_saving

        /// <summary>
        /// Loads a previously saved level.
        /// </summary>
        /// <param name="number">The level number.</param>
        public void LoadLevel(int number)
        {
            this.lastFloorAngle = 0.0f;
            this.currentFloorPoint = Vector2.Zero;
            this.EntitySelected = ModifiableEntity.Floor;
            this.levelToEdit.Load(number);
            this.lastFloorAngle = this.levelToEdit.GetLastFloorEdgeAngle();
            this.currentLevelNumber = number;
            this.lastFloorPoint = this.levelToEdit.LastFloorPoint;
        }

        /// <summary>
        /// Saves the current level.
        /// </summary>
        /// <param name="formatForContentManager">Whether to format the data for the content manager or the xml serializer.</param>
        public void SaveLevel(bool formatForContentManager)
        {
            this.levelToEdit.Save(this.currentLevelNumber, formatForContentManager);
            if (this.currentLevelNumber > this.LevelsCreated)
            {
                this.LevelsCreated++;
            }
        }

        /// <summary>
        /// Starts a new level.
        /// </summary>
        public void CreateNewLevel()
        {
            this.levelToEdit.Clear();
            this.currentLevelNumber = this.LevelsCreated + 1;
            this.lastFloorAngle = 0.0f;
            this.lastFloorPoint = Vector2.Zero;
            this.currentFloorPoint = Vector2.Zero;
            this.EntitySelected = ModifiableEntity.Floor;
        }

        #endregion

        /// <summary>
        /// Updates the level editor.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="cursorScreenPosition">The on-screen cursor position.</param>
        /// <param name="commands">The list of input commands.</param>
        public void Update(GameTime gameTime, Vector2 cursorScreenPosition, List<InputCommand> commands)
        {
            Vector2 cursorPosition = Camera2D.OffsetPosition + cursorScreenPosition;
            if (this.timeSinceLastPlace < Editor.TimeBetweenPlacments)
            {
                this.timeSinceLastPlace += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            this.UpdateCameraPosition(cursorScreenPosition, gameTime);
            this.cursorPosition.X = 8.0f * ((int)cursorPosition.X / 8);
            this.cursorPosition.Y = 8.0f * ((int)cursorPosition.Y / 8);
            if (this.EntitySelected == ModifiableEntity.Floor)
            {
                this.SetFloorAngle(cursorPosition);
            }

            this.CheckCursorPositionIsValid();
            this.ProcessCommands(commands);
        }

        #region drawing

        /// <summary>
        /// Draws the level editor's world.
        /// </summary>
        public void Draw()
        {
            this.levelToEdit.Draw();
            switch (this.EntitySelected)
            {
                case ModifiableEntity.Floor:
                    this.DrawFloorEdge();
                    break;
                case ModifiableEntity.StartPosition:
                    Camera2D.Draw(this.startSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.ExitPosition:
                    Camera2D.Draw(this.exitSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Platform:
                    this.DrawPlatform(this.cursorPosition);
                    break;
                case ModifiableEntity.Invincible:
                    Camera2D.Draw(this.invincibleSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Speed:
                    Camera2D.Draw(this.speedSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Jump:
                    Camera2D.Draw(this.jumpSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Health:
                    Camera2D.Draw(this.healthSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Fire:
                    Camera2D.Draw(this.fireSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Rock:
                    Camera2D.Draw(this.rockSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Spike:
                    Camera2D.Draw(this.spikeSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Cart:
                    Camera2D.Draw(this.cartSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Switch:
                    Camera2D.Draw(this.switchSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Coin:
                    Camera2D.Draw(this.coinSprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Ruby:
                    Camera2D.Draw(this.rubySprite, this.cursorPosition, 0.0f);
                    break;
                case ModifiableEntity.Diamond:
                    Camera2D.Draw(this.diamondSprite, this.cursorPosition, 0.0f);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Draws a floor edge.
        /// </summary>
        private void DrawFloorEdge()
        {
            Vector2 position = Vector2.Zero;
            float angle = 0.0f;
            if (this.lastFloorPoint == Vector2.Zero)
            {
                position = new Vector2(this.edgeSprite.Width / 2.0f, this.cursorPosition.Y);
            }
            else if (this.currentFloorPoint != this.lastFloorPoint)
            {
                position = (this.lastFloorPoint + this.currentFloorPoint) / 2.0f;
                Vector2 direction = this.currentFloorPoint - this.lastFloorPoint;
                direction.Normalize();
                angle = (float)Math.Asin(direction.Y);
            }

            Camera2D.Draw(this.edgeSprite, position, angle);
        }

        /// <summary>
        /// Draws a platform.
        /// </summary>
        /// <param name="position">The centre position of the platform.</param>
        private void DrawPlatform(Vector2 position)
        {
            Camera2D.Draw(this.platformSprite, position, 0.0f);
            if (this.platformWidth > this.platformSprite.Width)
            {
                int count = 0;

                // Useable area on middle sprites (removing rounded ends)
                float offset = this.platformSprite.Width - this.platformSprite.Height;
                float halfLeftOver = (this.platformWidth - offset) * 0.5f;

                // Leftover greater than useable area on end sprites
                while (halfLeftOver > this.platformSprite.Width - (this.platformSprite.Height / 2.0f))
                {
                    count++;
                    Camera2D.Draw(this.platformSprite, new Vector2(position.X + (offset * count), position.Y), 0.0f);
                    Camera2D.Draw(this.platformSprite, new Vector2(position.X - (offset * count), position.Y), 0.0f);
                    halfLeftOver -= offset;
                }

                // Fill in ends
                if (halfLeftOver > 0.0f)
                {
                    Camera2D.Draw(this.platformSprite, position + new Vector2((this.platformWidth / 2.0f) - (this.platformSprite.Width / 2.0f), 0.0f), 0.0f);
                    Camera2D.Draw(this.platformSprite, position + new Vector2(-(this.platformWidth / 2.0f) + (this.platformSprite.Width / 2.0f), 0.0f), 0.0f);
                }
            }
        }

        #endregion

        /// <summary>
        /// Processes any relevant input commands into actions.
        /// </summary>
        /// <param name="commands">The list of input commands.</param>
        private void ProcessCommands(List<InputCommand> commands)
        {
            if (this.timeSinceLastPlace >= Editor.TimeBetweenPlacments)
            {
                foreach (InputCommand command in commands)
                {
                    switch (command)
                    {
                        case InputCommand.Place:
                            if (this.CurrentPositionValid)
                            {
                                this.AddSelectedElement();
                                this.timeSinceLastPlace = 0.0f;
                            }
                            break;
                        case InputCommand.Swap:
                            this.CycleSelection();
                            this.timeSinceLastPlace = 0.0f;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the angle of the next floor element to place.
        /// </summary>
        /// <param name="cursorPosition">The current cursor position.</param>
        private void SetFloorAngle(Vector2 cursorPosition)
        {
            this.cursorPosition = cursorPosition;
            if (this.lastFloorPoint == Vector2.Zero)
            {
                this.currentFloorPoint = new Vector2(0.0f, this.cursorPosition.Y);
                this.lastFloorAngle = 0.0f;
            }
            else
            {
                Vector2 direction = this.cursorPosition - this.lastFloorPoint;
                if (direction.X <= 0.0f)
                {
                    direction = Vector2.UnitX;
                }
                else
                {
                    direction.Normalize();
                    float edgeAngle = (float)Math.Asin(direction.Y);
                    float difference = edgeAngle - this.lastFloorAngle;
                    if (difference < -Editor.MaxFloorAngleDeviation)
                    {
                        float newAngle = this.lastFloorAngle - Editor.MaxFloorAngleDeviation;
                        direction = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
                    }
                    else if (difference > Editor.MaxFloorAngleDeviation)
                    {
                        float newAngle = this.lastFloorAngle + Editor.MaxFloorAngleDeviation;
                        direction = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
                    }

                    edgeAngle = (float)Math.Asin(direction.Y);
                    if (edgeAngle > Editor.MaxFloorAngle)
                    {
                        direction = new Vector2((float)Math.Cos(Editor.MaxFloorAngle), (float)Math.Sin(Editor.MaxFloorAngle));
                    }
                    else if (edgeAngle < -Editor.MaxFloorAngle)
                    {
                        direction = new Vector2((float)Math.Cos(-Editor.MaxFloorAngle), (float)Math.Sin(-Editor.MaxFloorAngle));
                    }
                }

                this.currentFloorPoint = this.lastFloorPoint + (direction * this.edgeSprite.Width);
            }
        }

        /// <summary>
        /// Updates the camera position.
        /// </summary>
        /// <param name="screenCursorPosition">The on-screen cursor position.</param>
        /// <param name="gameTime">The game time.</param>
        private void UpdateCameraPosition(Vector2 screenCursorPosition, GameTime gameTime)
        {
            if (screenCursorPosition.X > this.screenDimensions.X * 0.75f)
            {
                float speed = (screenCursorPosition.X - (this.screenDimensions.X * 0.75f)) / (this.screenDimensions.X * 0.00025f);
                if (Camera2D.OffsetPosition.X + (speed * (float)gameTime.ElapsedGameTime.TotalSeconds) < Editor.MaxLength - (this.screenDimensions.X * 0.75f))
                {
                    Camera2D.Update(new Vector2(speed, 0.0f), gameTime);
                }
            }
            else if (screenCursorPosition.X < this.screenDimensions.X * 0.25f)
            {
                float speed = (screenCursorPosition.X - (this.screenDimensions.X * 0.25f)) / (this.screenDimensions.X * 0.00025f);
                if (Camera2D.OffsetPosition.X > -speed * (float)gameTime.ElapsedGameTime.TotalSeconds)
                {
                    Camera2D.Update(new Vector2(speed, 0.0f), gameTime);
                }
                else
                {
                    Camera2D.Update(new Vector2(-Camera2D.OffsetPosition.X, 0.0f), gameTime);
                }
            }
        }

        /// <summary>
        /// Checks if the current cursor position is valid for the current object type.
        /// </summary>
        private void CheckCursorPositionIsValid()
        {
            this.CurrentPositionValid = true;
            if (this.EntitySelected == ModifiableEntity.Floor)
            {
                this.CurrentPositionValid = this.cursorPosition.X > this.levelToEdit.LastFloorPoint.X && this.levelToEdit.LastFloorPoint.X < Editor.MaxLength;
            }
            else
            {
                this.CurrentPositionValid = this.cursorPosition.X < this.levelToEdit.LastFloorPoint.X && this.cursorPosition.X > 0.0f;
            }
        }
    }
}
