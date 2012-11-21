// -----------------------------------------------------------------------
// <copyright file="Level.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.LevelEditor
{
    using System.Collections.Generic;
    using Display;
    using Game.Entities;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using System;

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

        #endregion

        #region private_entities
        
        /// <summary>
        /// A list of platform descriptions.
        /// </summary>
        private List<PlatformDescription> platformDescriptions;

        /// <summary>
        /// A list of points which define the ends of the floor edges.
        /// </summary>
        private List<Vector2> floorEdgePoints;

        #endregion

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
        }

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
        }

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
