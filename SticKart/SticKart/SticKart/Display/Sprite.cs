namespace SticKart.Display
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A wrapper for drawing sprites centred on a specific position.
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// The origin to use for the sprite.
        /// </summary>
        private Vector2 origin;

        /// <summary>
        /// The texture to use for the sprite.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// The sprite batch to use for drawing the sprite.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        public Sprite()
        {
            this.Colour = Color.White;
        }

        /// <summary>
        /// Gets or sets the colour of the sprite.
        /// </summary>
        public Color Colour { get; set; }

        /// <summary>
        /// Gets the width of the sprite without scaling.
        /// </summary>
        public float Width
        {
            get
            {
                return this.texture.Width;
            }
        }

        /// <summary>
        /// Gets the height of the sprite without scaling.
        /// </summary>
        public float Height
        {
            get
            {
                return this.texture.Height;
            }
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="sprite">The sprite to draw.</param>
        /// <param name="position">The position to draw the sprite at.</param>
        /// <param name="rotation">The rotation of the sprite.</param>
        /// <param name="colour">The colour to draw the sprite in.</param>
        /// <param name="scale">The scale to draw the sprite at.</param>
        /// <param name="effect">The effect to apply to the sprite before drawing.</param>
        /// <param name="layerDepth">The layer depth of the sprite.</param>
        public static void Draw(Sprite sprite, Vector2 position, float rotation, Color colour, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None, float layerDepth = 1.0f)
        {
            sprite.spriteBatch.Draw(sprite.texture, position, null, colour, rotation, sprite.origin, scale, effect, layerDepth);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="sprite">The sprite to draw.</param>
        /// <param name="position">The position to draw the sprite at.</param>
        /// <param name="rotation">The rotation of the sprite.</param>
        public static void Draw(Sprite sprite, Vector2 position, float rotation)
        {
            sprite.spriteBatch.Draw(sprite.texture, position, null, sprite.Colour, rotation, sprite.origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        /// <summary>
        /// Initializes the sprite and loads the associated texture.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use when drawing.</param>
        /// <param name="contentManager">The content manager to load the content with.</param>
        /// <param name="pathToImage">The relative path to the texture asset.</param>
        public void InitalizeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager, string pathToImage)
        {
            this.spriteBatch = spriteBatch;
            this.texture = contentManager.Load<Texture2D>(pathToImage);
            this.origin = new Vector2(this.texture.Width / 2.0f, this.texture.Height / 2.0f);
        }
        
        /// <summary>
        /// Initializes the sprite and loads the associated texture.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use when drawing.</param>
        /// <param name="contentManager">The content manager to load the content with.</param>
        /// <param name="pathToImage">The relative path to the texture asset.</param>
        /// <param name="origin">The origin to use for the sprite.</param>
        public void InitalizeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager, string pathToImage, Vector2 origin)
        {
            this.spriteBatch = spriteBatch;
            this.texture = contentManager.Load<Texture2D>(pathToImage);
            this.origin = origin;
        }
    }
}
