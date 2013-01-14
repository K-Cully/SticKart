// -----------------------------------------------------------------------
// <copyright file="NotificationFactory.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Notification
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A factory for creating notification objects.
    /// </summary>
    public class NotificationFactory
    {
        /// <summary>
        /// The sprite batch used to render game sprites.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The content manager to use for loading assets.
        /// </summary>
        private ContentManager contentManager;

        /// <summary>
        /// The size of the game display area.
        /// </summary>
        private Vector2 displayDimensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationFactory"/> class.
        /// </summary>
        /// <param name="contentManager">The content manager to use for loading assets.</param>
        /// <param name="spriteBatch">The sprite batch used to render game sprites.</param>
        /// <param name="displayDimensions">The size of the game display area.</param>
        public NotificationFactory(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 displayDimensions)
        {
            this.spriteBatch = spriteBatch;
            this.contentManager = contentManager;
            this.displayDimensions = displayDimensions;
        }

        /// <summary>
        /// Creates a notification of the type passed in.
        /// </summary>
        /// <param name="notificationType">The type of notification to create.</param>
        /// <returns>The new notification.</returns>
        public Notification Create(NotificationType notificationType)
        {
            // TODO: finish this
            return new Notification(this.contentManager, this.spriteBatch, this.displayDimensions / 2.0f, 5.0f, string.Empty, string.Empty, string.Empty, 0, string.Empty);
        }
    }
}
