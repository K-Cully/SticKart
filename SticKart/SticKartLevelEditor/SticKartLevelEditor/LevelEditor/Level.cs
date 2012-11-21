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

    /// <summary>
    /// Defines a level which the level editor can modify.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// A platform sprite.
        /// </summary>
        private Sprite platformSprite; 
        
        /// <summary>
        /// A list of platform descriptions.
        /// </summary>
        private List<PlatformDescription> platformDescriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        public Level()
        {
            this.platformDescriptions = new List<PlatformDescription>();
            this.platformSprite = new Sprite();
        }

        /// <summary>
        /// Loads any resources used by a level.
        /// </summary>
        /// <param name="spriteBatch">the game's sprite batch.</param>
        /// <param name="contentManager">The game's content manager.</param>
        public void LoadContent(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.platformSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Platform);
        }

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
        /// Draws the level.
        /// </summary>
        public void Draw()
        {
            // TODO: Implement rest of objects

            // Draw platforms
            foreach (PlatformDescription platformDescription in this.platformDescriptions)
            {
                this.DrawPlatform(platformDescription);
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
    }
}
