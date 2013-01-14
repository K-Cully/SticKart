// -----------------------------------------------------------------------
// <copyright file="Notification.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display.Notification
{
    using Audio;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A wrapper for displaying  a notification message. 
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// The time, in seconds, which the notification should be active for.
        /// </summary>
        private float timeToLive;

        /// <summary>
        /// The timer to keep track of how long the notification has been active.
        /// </summary>
        private float lifeTimer;

        /// <summary>
        /// The background of the notification.
        /// </summary>
        private Sprite background;

        /// <summary>
        /// The animated sprite to display on the notification.
        /// </summary>
        private AnimatedSprite image;

        /// <summary>
        /// The text to display on the notification.
        /// </summary>
        private RenderableText text;

        /// <summary>
        /// The centre position of the notification.
        /// </summary>
        private Vector2 centrePosition;

        /// <summary>
        /// The position to render the notification image at.
        /// </summary>
        private Vector2 imagePosition;

        /// <summary>
        /// The position to render the notification text at.
        /// </summary>
        private Vector2 textPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        /// <param name="contentManger">The game's content manger.</param>
        /// <param name="spriteBatch">The sprite batch to render using.</param>
        /// <param name="centrePosition">The position to centre the notification at.</param>
        /// <param name="timeToLive">The time, in seconds, which the notification should be active for.</param>
        /// <param name="text">The text to display on the notification.</param>
        /// <param name="pathToFont">The location of the font to use for the notification text.</param>
        /// <param name="pathToImage">The location of the image to display on the notification.</param>
        /// <param name="numberOfFrames">The number of frames to separate the image into.</param>
        /// <param name="pathToBackgroundImage">The location of the background image of the notification.</param>
        public Notification(ContentManager contentManger, SpriteBatch spriteBatch, Vector2 centrePosition, float timeToLive, string text, string pathToFont, string pathToImage, int numberOfFrames, string pathToBackgroundImage)
        {
            this.Active = true;
            this.timeToLive = timeToLive;
            this.lifeTimer = 0.0f;
            if (pathToImage != null && pathToImage != string.Empty)
            {
                this.image = new AnimatedSprite();
                this.image.InitializeAndLoad(spriteBatch, contentManger, pathToImage);
            }
            else
            {
                this.image = null;
            }

            if (text != null && text != string.Empty)
            {
                this.text = new RenderableText();
                this.text.InitializeAndLoad(spriteBatch, contentManger, pathToFont, text);
            }
            else
            {
                this.text = null;
            }

            if (pathToBackgroundImage != null && pathToBackgroundImage != string.Empty)
            {
                this.background = new Sprite();
                this.background.InitializeAndLoad(spriteBatch, contentManger, pathToBackgroundImage);
            }
            else
            {
                this.background = null;
            }

            this.SetRenderingPositions(centrePosition);
        }

        /// <summary>
        /// Gets a value indicating whether the notification is active or not.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Updates the notification.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="exitDetected">A value indicating if an exit event occurred or not.</param>
        public void Update(GameTime gameTime, bool exitDetected)
        {
            this.lifeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Active = this.lifeTimer < this.timeToLive && !exitDetected;
        }

        /// <summary>
        /// Draws the notification to the screen.
        /// </summary>
        public void Draw()
        {
            if (this.background != null)
            {
                Sprite.Draw(this.background, this.centrePosition, 0.0f);
            }

            if (this.image != null)
            {
                AnimatedSprite.Draw(this.image, this.imagePosition, 0.0f);
            }

            if (this.text != null)
            {
                RenderableText.Draw(this.text, this.textPosition, 0.0f);
            }
        }

        /// <summary>
        /// Sets the position to render the notification text and image.
        /// </summary>
        /// <param name="notificationCentre">The centre position of the notification.</param>
        private void SetRenderingPositions(Vector2 notificationCentre)
        {
            this.centrePosition = notificationCentre;
            this.imagePosition = notificationCentre;
            this.textPosition = notificationCentre;
            if (this.image != null && this.text != null)
            {
                this.imagePosition.Y -= this.text.Height;
                this.textPosition.Y += this.image.Height + (0.5f * this.text.Height);
            }
        }
    }
}
