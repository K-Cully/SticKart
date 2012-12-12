// -----------------------------------------------------------------------
// <copyright file="ColourStreamRenderer.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display
{
    using Microsoft.Kinect;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Renders the Kinect's colour stream data to the screen.
    /// </summary>
    public class ColourStreamRenderer
    {
        /// <summary>
        /// The game's graphics device.
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// The texture set from the colour stream data.
        /// </summary>
        private Texture2D colourTexture;
        
        /// <summary>
        /// The effect to swap red and blue bytes of the colour stream.
        /// </summary>
        private Effect spriteEffect;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ColourStreamRenderer"/> class.
        /// </summary>
        /// <param name="contentManager">The game's content manager.</param>
        /// <param name="graphicsDevice">The game's graphics device.</param>
        public ColourStreamRenderer(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.colourTexture = null;
            this.spriteEffect = contentManager.Load<Effect>(ContentLocations.KinectColourEffect);
            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Updates the colour image displayed.
        /// </summary>
        /// <param name="colourData">The colour frame data as a byte array.</param>
        /// <param name="colourFrameSize">The size of the colour frame.</param>
        public void Update(byte[] colourData, Vector2 colourFrameSize)
        {
            if (colourData != null)
            {
                if (this.colourTexture == null || this.colourTexture.Height * this.colourTexture.Width != colourData.Length)
                {
                    this.colourTexture = new Texture2D(this.graphicsDevice, (int)colourFrameSize.X, (int)colourFrameSize.Y, false, SurfaceFormat.Color);
                }

                this.colourTexture.SetData<byte>(colourData);
            }
        }

        /// <summary>
        /// Draws the colour stream.
        /// This should only be called after the sprite batch end call as it utilizes its own effect for rendering.
        /// </summary>
        /// <param name="spriteBatch">The sprite</param>
        /// <param name="targetRectangle">The area to draw the colour stream to.</param>
        /// <param name="percentWidthToDraw">The percentage of the width of the frame to draw.</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle targetRectangle, float percentWidthToDraw)
        {
            if (this.colourTexture != null)
            {
                percentWidthToDraw = percentWidthToDraw > 10.0f ? percentWidthToDraw < 100.0f ? percentWidthToDraw : 100.0f : 10.0f;
                Rectangle sourceRectangle = new Rectangle((int)((this.colourTexture.Width * 0.5f) * (percentWidthToDraw / 100.0f)), 0, (int)(this.colourTexture.Width * (percentWidthToDraw / 100.0f)), this.colourTexture.Height);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, this.spriteEffect);
                spriteBatch.Draw(this.colourTexture, targetRectangle, sourceRectangle, Color.White);
                spriteBatch.End();
            }
        }
    }
}
