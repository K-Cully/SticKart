// -----------------------------------------------------------------------
// <copyright file="Level.cs" company="None">
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
    /// Defines a level which the level editor can modify.
    /// </summary>
    public class Level
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

        #region private_entities
        
        /// <summary>
        /// A list of platform descriptions.
        /// </summary>
        private List<PlatformDescription> platformDescriptions;

        /// <summary>
        /// A list of interactive entity descriptions.
        /// </summary>
        private List<InteractiveEntityDescription> interactiveEntityDescriptions;

        /// <summary>
        /// A list of points which define the ends of the floor edges.
        /// </summary>
        private List<Vector2> floorEdgePoints;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        public Level()
        {
            this.StartPosition = Vector2.Zero;
            this.ExitPosition = Vector2.Zero;
            this.platformDescriptions = new List<PlatformDescription>();
            this.floorEdgePoints = new List<Vector2>();
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
        }

        #endregion

        #region public_entities

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        public Vector2 StartPosition { get; set; }

        /// <summary>
        /// Gets or sets the exit position.
        /// </summary>
        public Vector2 ExitPosition { get; set; }

        #endregion

        #region content_loading

        /// <summary>
        /// Loads any resources used by a level.
        /// </summary>
        /// <param name="spriteBatch">the game's sprite batch.</param>
        /// <param name="contentManager">The game's content manager.</param>
        public void LoadContent(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.platformSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Platform);
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

        #region insertion

        /// <summary>
        /// Adds a platform to the level.
        /// </summary>
        /// <param name="length">The length of the platform.</param>
        /// <param name="position">The position of the platform.</param>
        public void AddPlatform(float length, Vector2 position)
        {
            PlatformDescription platformDescription = new PlatformDescription();
            platformDescription.Length = length;
            platformDescription.Position = position;
            this.platformDescriptions.Add(platformDescription);
        }

        /// <summary>
        /// Adds an interactive entity to the level.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <param name="position">The position of the entity.</param>
        /// <param name="size">The size of the entity in display coordinates.</param>
        public void AddInteractiveEntity(string name, Vector2 position, Vector2 size)
        {
            InteractiveEntityDescription entityDescription = new InteractiveEntityDescription();
            entityDescription.Position = position;
            entityDescription.Name = name;
            entityDescription.Dimensions = size;
            this.interactiveEntityDescriptions.Add(entityDescription);
        }
        
        /// <summary>
        /// Adds a point to the floor edges.
        /// </summary>
        /// <param name="point">The point to add.</param>
        public void AddFloorPoint(Vector2 point)
        {
            this.floorEdgePoints.Add(point);
        }

        #endregion

        #region removal

        /// <summary>
        /// Removes the last platform added.
        /// </summary>
        public void RemoveLastPlatform()
        {
            if (this.platformDescriptions.Count > 0)
            {
                this.platformDescriptions.RemoveAt(this.platformDescriptions.Count - 1);
            }
        }
        
        /// <summary>
        /// Removes the last interactive entity added.
        /// </summary>
        public void RemoveLastInteractiveEntity()
        {
            if (this.interactiveEntityDescriptions.Count > 0)
            {
                this.interactiveEntityDescriptions.RemoveAt(this.interactiveEntityDescriptions.Count - 1);
            }
        }

        /// <summary>
        /// Removes the last floor point added.
        /// </summary>
        public void RemoveLastFloorPoint()
        {
            if (this.floorEdgePoints.Count > 0)
            {
                this.floorEdgePoints.RemoveAt(this.floorEdgePoints.Count - 1);
            }
        }

        #endregion

        #region drawing

        /// <summary>
        /// Draws the level.
        /// </summary>
        public void Draw()
        {
            // TODO: Implement rest of objects
            if (this.StartPosition != Vector2.Zero)
            {
                Camera2D.Draw(this.startSprite, this.StartPosition, 0.0f);
            }

            if (this.ExitPosition != Vector2.Zero)
            {
                Camera2D.Draw(this.exitSprite, this.ExitPosition, 0.0f);
            }

            this.DrawFloor();
            foreach (PlatformDescription platformDescription in this.platformDescriptions)
            {
                this.DrawPlatform(platformDescription);
            }

            foreach (InteractiveEntityDescription entityDescription in this.interactiveEntityDescriptions)
            {
                this.DrawInteractiveEntity(entityDescription);
            }
        }

        /// <summary>
        /// Draws the floor to the screen.
        /// </summary>
        private void DrawFloor()
        {
            Vector2 startPoint = Vector2.Zero;
            foreach (Vector2 point in this.floorEdgePoints)
            {
                if (startPoint != Vector2.Zero)
                {
                    Vector2 direction = point - startPoint;
                    direction.Normalize();
                    Camera2D.Draw(this.edgeSprite, (startPoint + point) / 2.0f, (float)Math.Acos(direction.X));
                }

                startPoint = point;
            }
        }

        /// <summary>
        /// Draws an interactive entity.
        /// </summary>
        /// <param name="entityDescription">The entity's description.</param>
        private void DrawInteractiveEntity(InteractiveEntityDescription entityDescription)
        {
            switch (entityDescription.Name)
            {
                case EntityConstants.FireName:
                    Camera2D.Draw(this.fireSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.SpikeName:
                    Camera2D.Draw(this.spikeSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.RockName:
                    Camera2D.Draw(this.rockSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.InvincibleName:
                    Camera2D.Draw(this.invincibleSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.HealthName:
                    Camera2D.Draw(this.healthSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.SpeedName:
                    Camera2D.Draw(this.speedSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.JumpName:
                    Camera2D.Draw(this.jumpSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.CoinName:
                    Camera2D.Draw(this.coinSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.DiamondName:
                    Camera2D.Draw(this.diamondSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.RubyName:
                    Camera2D.Draw(this.rubySprite, entityDescription.Position, 0.0f);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Draws a platform to the screen.
        /// </summary>
        /// <param name="platformDescription">The platform description to use draw.</param>
        private void DrawPlatform(PlatformDescription platformDescription)
        {
            Camera2D.Draw(this.platformSprite, platformDescription.Position, 0.0f);
            if (platformDescription.Length > this.platformSprite.Width)
            {
                int count = 0;

                // Useable area on middle sprites (removing rounded ends)
                float offset = this.platformSprite.Width - this.platformSprite.Height;
                float halfLeftOver = (platformDescription.Length - offset) * 0.5f;

                // Leftover greater than useable area on end sprites
                while (halfLeftOver > this.platformSprite.Width - (this.platformSprite.Height / 2.0f))
                {
                    count++;
                    Camera2D.Draw(this.platformSprite, new Vector2(platformDescription.Position.X + (offset * count), platformDescription.Position.Y), 0.0f);
                    Camera2D.Draw(this.platformSprite, new Vector2(platformDescription.Position.X - (offset * count), platformDescription.Position.Y), 0.0f);
                    halfLeftOver -= offset;
                }

                // Fill in ends
                if (halfLeftOver > 0.0f)
                {
                    Camera2D.Draw(this.platformSprite, platformDescription.Position + new Vector2((platformDescription.Length / 2.0f) - (this.platformSprite.Width / 2.0f), 0.0f), 0.0f);
                    Camera2D.Draw(this.platformSprite, platformDescription.Position + new Vector2(-(platformDescription.Length / 2.0f) + (this.platformSprite.Width / 2.0f), 0.0f), 0.0f);
                }
            }
        }

        #endregion
    }
}
