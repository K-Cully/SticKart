namespace SticKart.Display
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A wrapper for rendering text centered on a specific position.
    /// </summary>
    public class RenderableText
    {
         /// <summary>
        /// The origin to use for the text.
        /// </summary>
        private Vector2 origin;

        /// <summary>
        /// The font to use for the text.
        /// </summary>
        private SpriteFont font;

        /// <summary>
        /// The text to render.
        /// </summary>
        private string text;

        /// <summary>
        /// The sprite batch to use for drawing the text.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The size of the text without scaling. 
        /// </summary>
        private Vector2 size;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderableText"/> class.
        /// </summary>
        public RenderableText()
        {
            this.Colour = Color.White;
        }

        /// <summary>
        /// Gets or sets the colour of the text.
        /// </summary>
        public Color Colour { get; set; }
        
        /// <summary>
        /// Gets the width of the text without scaling.
        /// </summary>
        public float Width
        {
            get
            {
                return this.size.X;
            }
        }

        /// <summary>
        /// Gets the height of the text without scaling.
        /// </summary>
        public float Height
        {
            get
            {
                return this.size.Y;
            }
        }
        
        /// <summary>
        /// Draws text to the screen.
        /// </summary>
        /// <param name="renderableText">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="rotation">The rotation of the text.</param>
        /// <param name="colour">The colour to draw the text in.</param>
        /// <param name="scale">The scale to draw the text at.</param>
        /// <param name="effect">The effect to apply to the text before drawing.</param>
        /// <param name="layerDepth">The layer depth of the text.</param>
        public static void Draw(RenderableText renderableText, Vector2 position, float rotation, Color colour, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None, float layerDepth = 1.0f)
        {
            renderableText.spriteBatch.DrawString(renderableText.font, renderableText.text, position, colour, rotation, renderableText.origin, scale, effect, layerDepth);
        }

        /// <summary>
        /// Draws text to the screen.
        /// </summary>
        /// <param name="renderableText">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="rotation">The rotation of the text.</param>
        public static void Draw(RenderableText renderableText, Vector2 position, float rotation)
        {
            renderableText.spriteBatch.DrawString(renderableText.font, renderableText.text, position, renderableText.Colour, rotation, renderableText.origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        /// <summary>
        /// Initializes the text and loads the associated font.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use when drawing.</param>
        /// <param name="contentManager">The content manager to load the content with.</param>
        /// <param name="pathToFont">The relative path to the font asset.</param>
        /// <param name="text">The text to render.</param>
        public void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager, string pathToFont, string text)
        {
            this.spriteBatch = spriteBatch;
            this.font = contentManager.Load<SpriteFont>(pathToFont);
            this.text = text;
            this.size = this.font.MeasureString(this.text);
            this.origin = this.size / 2.0f;
        }
        
        /// <summary>
        /// Initializes the text and loads the associated font.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use when drawing.</param>
        /// <param name="contentManager">The content manager to load the content with.</param>
        /// <param name="pathToFont">The relative path to the font asset.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="origin">The origin to use for the text.</param>
        public void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager, string pathToFont, string text, Vector2 origin)
        {
            this.spriteBatch = spriteBatch;
            this.font = contentManager.Load<SpriteFont>(pathToFont);
            this.text = text;
            this.size = this.font.MeasureString(this.text);
            this.origin = origin;
        }

        /// <summary>
        /// Sets the text to render.
        /// </summary>
        /// <param name="text">The new text.</param>
        public void SetText(string text)
        {
            this.text = text;
            this.size = this.font.MeasureString(this.text);
            this.origin = this.size / 2.0f;
        }
    }
}
