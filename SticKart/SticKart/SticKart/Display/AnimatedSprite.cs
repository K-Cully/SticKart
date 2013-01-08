// -----------------------------------------------------------------------
// <copyright file="AnimatedSprite.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A wrapper for drawing animated sprites, centred on a specific position.
    /// </summary>
    public class AnimatedSprite : Sprite
    {
        /// <summary>
        /// A value indicating if the animation should loop continuously.
        /// </summary>
        private bool loop;

        /// <summary>
        /// The current frame number.
        /// </summary>
        private int frameNumber;

        /// <summary>
        /// The number of frames.
        /// </summary>
        private int numberOfFrames;

        /// <summary>
        /// The time, in seconds, between frames.
        /// </summary>
        private float timeBetweenFrames;

        /// <summary>
        /// The time, in seconds, since the last frame.
        /// </summary>
        private float frameTimer;

        /// <summary>
        /// The current area/frame of the image to display.
        /// </summary>
        private Rectangle sourceFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedSprite"/> class.
        /// </summary>
        public AnimatedSprite()
            : base()
        {
            this.loop = false;
            this.frameNumber = 0;
            this.numberOfFrames = 0;
            this.timeBetweenFrames = 0.0f;
            this.sourceFrame = new Rectangle();
            this.frameTimer = 0.0f;
        }

        /// <summary>
        /// Gets a value indicating if the animation is finished playing or not.
        /// </summary>
        public bool Finished
        {
            get
            {
                return !this.loop && this.frameNumber == (this.numberOfFrames - 1);
            }
        }

        /// <summary>
        /// Gets the width of an individual frame.
        /// </summary>
        public float FrameWidth
        {
            get
            {
                return this.Width / this.numberOfFrames;
            }
        }

        /// <summary>
        /// Gets the height of an individual frame.
        /// </summary>
        public float FrameHeight
        {
            get
            {
                return this.Height;
            }
        }

        /// <summary>
        /// Draws an animated sprite.
        /// </summary>
        /// <param name="sprite">The animated sprite to draw.</param>
        /// <param name="position">The position to draw the sprite at.</param>
        /// <param name="rotation">The rotation of the sprite.</param>
        public static void Draw(AnimatedSprite sprite, Vector2 position, float rotation)
        {
            sprite.SpriteBatch.Draw(sprite.Texture, position, sprite.sourceFrame, sprite.Colour, rotation, sprite.Origin, 1.0f, SpriteEffects.None, 1.0f);
        }

        /// <summary>
        /// Draws an animated sprite.
        /// </summary>
        /// <param name="sprite">The animated sprite to draw.</param>
        /// <param name="position">The position to draw the sprite at.</param>
        /// <param name="rotation">The rotation of the sprite.</param>
        /// <param name="colour">The colour to draw the sprite in.</param>
        /// <param name="scale">The scale to draw the sprite at.</param>
        /// <param name="effect">The effect to apply to the sprite before drawing.</param>
        /// <param name="layerDepth">The layer depth of the sprite.</param>
        public static void Draw(AnimatedSprite sprite, Vector2 position, float rotation, Color colour, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None, float layerDepth = 1.0f)
        {
            sprite.SpriteBatch.Draw(sprite.Texture, position, sprite.sourceFrame, colour, rotation, sprite.Origin, scale, effect, layerDepth);
        }

        /// <summary>
        /// Resets the current frame number to 0.
        /// </summary>
        public void Reset()
        {
            this.frameNumber = 0;
        }

        /// <summary>
        /// Updates the animation frame.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            if (this.frameTimer < this.timeBetweenFrames)
            {
                this.frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.frameTimer = 0.0f;
                this.frameNumber++;
                if (this.frameNumber == this.numberOfFrames)
                {
                    this.frameNumber = this.loop ? 0 : this.numberOfFrames - 1;
                }

                this.sourceFrame = new Rectangle((int)(this.FrameWidth * (float)this.frameNumber), 0, (int)this.FrameWidth, (int)this.Height);
            }
        }

        /// <summary>
        /// Initializes the sprite and loads the associated texture.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use when drawing.</param>
        /// <param name="contentManager">The content manager to load the content with.</param>
        /// <param name="pathToImage">The relative path to the texture asset.</param>
        /// <param name="numberOfFrames">The number of frames to split the image into.</param>
        /// <param name="timeBetweenFrames">The time between frame changes.</param>
        /// <param name="loop">A value indicating if the animation should loop or not.</param>
        public void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager, string pathToImage, int numberOfFrames, float timeBetweenFrames, bool loop)
        {
            base.InitializeAndLoad(spriteBatch, contentManager, pathToImage);
            this.numberOfFrames = numberOfFrames;
            this.Origin = this.Origin - new Vector2((this.Width - this.FrameWidth) / 2.0f, 0.0f);
            this.sourceFrame = new Rectangle(0, 0, (int)(this.Width / this.numberOfFrames), (int)this.Height);
            this.timeBetweenFrames = timeBetweenFrames;
            this.loop = loop;
            this.frameNumber = 0;
            this.frameTimer = 0.0f;
        }

        /// <summary>
        /// Initializes the sprite and loads the associated texture.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use when drawing.</param>
        /// <param name="contentManager">The content manager to load the content with.</param>
        /// <param name="pathToImage">The relative path to the texture asset.</param>
        /// <param name="origin">The origin to use for the sprite.</param>
        /// <param name="numberOfFrames">The number of frames to split the image into.</param>
        /// <param name="timeBetweenFrames">The time between frame changes.</param>
        /// <param name="loop">A value indicating if the animation should loop or not.</param>
        public void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager, string pathToImage, Vector2 origin, int numberOfFrames, float timeBetweenFrames, bool loop)
        {
            base.InitializeAndLoad(spriteBatch, contentManager, pathToImage, origin);
            this.numberOfFrames = numberOfFrames;
            this.Origin = this.Origin - new Vector2((this.Width - this.FrameWidth) / 2.0f, 0.0f);
            this.sourceFrame = new Rectangle(0, 0, (int)(this.Width / this.numberOfFrames), (int)this.Height);
            this.timeBetweenFrames = timeBetweenFrames;
            this.loop = loop;
            this.frameNumber = 0;
            this.frameTimer = 0.0f;
        }
    }
}
