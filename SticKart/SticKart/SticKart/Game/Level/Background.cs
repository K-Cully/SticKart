// -----------------------------------------------------------------------
// <copyright file="Background.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Level
{
    using System.Collections.ObjectModel;
    using Display;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A wrapper for the level background.
    /// </summary>
    public class Background
    {
        /// <summary>
        /// A sprite representing the scenery to display.
        /// </summary>
        private Sprite tileSprite;

        /// <summary>
        /// The dimensions of the game display area.
        /// </summary>
        private Vector2 screenDimensions;

        /// <summary>
        /// The depth of the sprite layer which determines the scroll rate relative to the main play layer.
        /// </summary>
        private float layerDepth;

        /// <summary>
        /// The number of rows of tiles.
        /// </summary>
        private int tiledRows;

        /// <summary>
        /// The number of columns of tiles.
        /// </summary>
        private int tiledColumns;

        /// <summary>
        /// A list of positions to display tiles at.
        /// </summary>
        private Collection<Vector2> tilePositions;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Background"/> class.
        /// </summary>
        /// <param name="screenDimensions">The dimensions of the display area.</param>
        /// <param name="scrollRate">The rate at which the background should scroll, relative to the game play layer.</param>
        public Background(Vector2 screenDimensions, float scrollRate)
        {
            this.screenDimensions = screenDimensions;
            this.tileSprite = new Sprite();
            this.tilePositions = new Collection<Vector2>();
            this.layerDepth = scrollRate > 0.0f ? 1.0f / scrollRate : 1.0f;
            this.tiledRows = 3;
            this.tiledColumns = 5;
        }

        /// <summary>
        /// Initializes and loads the sprite used by the background.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        /// <param name="textureName">The name of the texture asset to use.</param>
        public void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager, string textureName)
        {
            this.tileSprite.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.Scenery + textureName);
            for (float rowCount = 0; rowCount < this.tiledRows; rowCount++)
            {
                for (float colCount = 0; colCount < this.tiledColumns; colCount++)
                {
                    this.tilePositions.Add(new Vector2(this.tileSprite.Width * (colCount + 0.5f), this.tileSprite.Height * (rowCount + 0.5f)));
                }
            }
        }

        /// <summary>
        /// Resets the tiled area of the background.
        /// </summary>
        public void Reset()
        {
            for (float rowCount = 0; rowCount < this.tiledRows; rowCount++)
            {
                for (float colCount = 0; colCount < this.tiledColumns; colCount++)
                {
                    this.tilePositions[(int)((rowCount * this.tiledColumns) + colCount)] = new Vector2(this.tileSprite.Width * (colCount + 0.5f), this.tileSprite.Height * (rowCount + 0.5f));
                }
            }
        }

        /// <summary>
        /// Updates the tiled background area.
        /// </summary>
        public void Update()
        {
            for (int count = 0; count < this.tilePositions.Count; count++)
            {
                if (this.tilePositions[count].X + this.tileSprite.Width < (1.0f / this.layerDepth) * Camera2D.OffsetPosition.X)
                {
                    this.tilePositions[count] += new Vector2(this.tileSprite.Width * this.tiledColumns, 0.0f);
                }
                else if (this.tilePositions[count].X - this.tileSprite.Width > ((1.0f / this.layerDepth) * Camera2D.OffsetPosition.X) + this.screenDimensions.X)
                {
                    this.tilePositions[count] -= new Vector2(this.tileSprite.Width * this.tiledColumns, 0.0f);
                }

                if (this.tilePositions[count].Y + this.tileSprite.Height < (1.0f / this.layerDepth) * Camera2D.OffsetPosition.Y)
                {
                    this.tilePositions[count] += new Vector2(0.0f, this.tileSprite.Height * this.tiledRows);
                }
                else if (this.tilePositions[count].Y - this.tileSprite.Height > ((1.0f / this.layerDepth) * Camera2D.OffsetPosition.Y) + this.screenDimensions.Y)
                {
                    this.tilePositions[count] -= new Vector2(0.0f, this.tileSprite.Height * this.tiledRows);
                }
            }
        }

        /// <summary>
        /// Draws the background, tiled across the screen.
        /// </summary>
        public void Draw()
        {
            Vector2 cameraPos = Camera2D.OffsetPosition;
            foreach (Vector2 tilePosition in this.tilePositions)
            {
                Camera2D.Draw(this.tileSprite, tilePosition, 0.0f, Color.White, 1.0f, SpriteEffects.None, this.layerDepth);              
            }
        }
    }
}
