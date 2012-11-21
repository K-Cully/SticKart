// -----------------------------------------------------------------------
// <copyright file="LevelEditor.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.LevelEditor
{
    using System;
    using System.Collections.Generic;
    using Display;
    using Game.Entities;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a level editor for the game.
    /// </summary>
    public class LevelEditor
    {        
        #region sprites

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

        /// <summary>
        /// The level being edited.
        /// </summary>
        private Level levelToEdit;


        // TODO: Modify this
        /// <summary>
        /// The width of the next platform.
        /// </summary>
        private float platformWidth;

        /// <summary>
        /// The position of the last floor point.
        /// </summary>
        private Vector2 lastFloorPoint;

        /// <summary>
        /// The position ot he current floor point.
        /// </summary>
        private Vector2 currentFloorPoint;

        /// <summary>
        /// The current position of the user's cursor.
        /// </summary>
        private Vector2 cursorPosition;
                
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelEditor"/> class.
        /// </summary>
        public LevelEditor()
        {
            this.cursorPosition = Vector2.Zero;
            this.lastFloorPoint = Vector2.Zero;
            this.currentFloorPoint = Vector2.Zero;
            this.EntitySelected = ModifiableEntity.Floor;
            this.levelToEdit = new Level();
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
            this.platformWidth = 128.0f;
        }

        #region public_accessors
        
        /// <summary>
        /// Gets or sets the currently selected entity to modify. 
        /// </summary>
        public ModifiableEntity EntitySelected { get; set; }

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
            this.platformSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Platform);
            this.platformWidth = platformSprite.Width;
            this.edgeSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Floor);
            this.startSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.StickManStanding);
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

        /// <summary>
        /// Updates the level editor.
        /// </summary>
        /// <param name="cursorPosition">The user's cursor position.</param>
        public void Update(Vector2 cursorPosition)
        {
            this.cursorPosition = cursorPosition;

            if (this.EntitySelected == ModifiableEntity.Floor)
            {
                if (this.lastFloorPoint == Vector2.Zero)
                {
                    this.currentFloorPoint = new Vector2(0.0f, this.cursorPosition.Y);
                }
                else
                {
                    Vector2 direction = this.cursorPosition - this.lastFloorPoint;
                    direction.Normalize();
                    
                    // TODO: limit the angle of this vector.
                    this.currentFloorPoint = direction * this.edgeSprite.Width; 
                }
            }
        }

        /// <summary>
        /// Adds the currently selected element to the level.
        /// </summary>
        public void AddSelectedElement()
        {
            switch (this.EntitySelected)
            {
                case ModifiableEntity.Floor:
                    this.levelToEdit.AddFloorPoint(this.currentFloorPoint);
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
                    this.EntitySelected = ModifiableEntity.StartPosition;
                    break;
                case ModifiableEntity.StartPosition:
                    this.EntitySelected = ModifiableEntity.ExitPosition;
                    break;
                case ModifiableEntity.ExitPosition:
                    this.EntitySelected = ModifiableEntity.Platform;
                    break;
                case ModifiableEntity.Platform:
                    this.EntitySelected = ModifiableEntity.Invincible;
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
                    this.EntitySelected = ModifiableEntity.Fire;
                    break;
                case ModifiableEntity.Fire:
                    this.EntitySelected = ModifiableEntity.Rock;
                    break;
                case ModifiableEntity.Rock:
                    this.EntitySelected = ModifiableEntity.Spike;
                    break;
                case ModifiableEntity.Spike:
                    this.EntitySelected = ModifiableEntity.Floor;
                    break;
                default:
                    break;
            }
        }

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
            else
            {
                position = (this.lastFloorPoint + this.currentFloorPoint) / 2.0f;
                Vector2 direction = this.currentFloorPoint - this.lastFloorPoint;
                direction.Normalize();
                angle = (float)Math.Acos(direction.X);
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
    }
}
