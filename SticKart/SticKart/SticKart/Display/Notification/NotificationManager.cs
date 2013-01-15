// -----------------------------------------------------------------------
// <copyright file="NotificationManager.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display.Notification
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Manages the update and display of notifications.
    /// </summary>
    public sealed class NotificationManager
    {
        /// <summary>
        /// The notification manager singleton.
        /// </summary>
        private static volatile NotificationManager managerSingleton = null;

        /// <summary>
        /// An object to lock on which ensures thread-safe instantiation of the notification manager.
        /// </summary>
        private static object mutex = new object();
        
        /// <summary>
        /// A queue of notifications to display.
        /// </summary>
        private volatile Queue<Notification> notificationQueue;

        /// <summary>
        /// The factory to use to create notifications.
        /// </summary>
        private NotificationFactory notificationFactory;

        /// <summary>
        /// The text to inform the user how to force-close a notification.
        /// </summary>
        private RenderableText closeInformation;

        /// <summary>
        /// The position to render the text at.
        /// </summary>
        private Vector2 textPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationManager"/> class.
        /// </summary>
        /// <param name="contentManager">The content manager to use for loading assets.</param>
        /// <param name="spriteBatch">The sprite batch used to render game sprites.</param>
        /// <param name="displayDimensions">The size of the game display area.</param>
        private NotificationManager(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 displayDimensions)
        {
            this.notificationQueue = new Queue<Notification>();
            this.notificationFactory = new NotificationFactory(contentManager, spriteBatch, displayDimensions);
            this.closeInformation = new RenderableText();
            this.closeInformation.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, NotificationStrings.CloseNotification);
            this.closeInformation.Colour = Color.Gray;
            this.textPosition = new Vector2(displayDimensions.X / 2.0f, displayDimensions.Y / 8.0f);
        }

        /// <summary>
        /// Gets the notification manager singleton, if it has been initialized.
        /// </summary>
        public static NotificationManager Instance
        {
            get
            {
                return NotificationManager.managerSingleton;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there are any notifications active or not.
        /// </summary>
        public bool NotificationsActive
        {
            get
            {
                lock (NotificationManager.mutex)
                {
                    return this.notificationQueue.Count > 0;
                }
            }
        }

        /// <summary>
        /// Initializes the <see cref="NotificationManager"/> singleton.
        /// </summary>
        /// <param name="contentManager">The content manager to use for loading assets.</param>
        /// <param name="spriteBatch">The sprite batch used to render game sprites.</param>
        /// <param name="displayDimensions">The size of the game display area.</param>
        public static NotificationManager Initialize(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 displayDimensions)
        {
            if (NotificationManager.managerSingleton == null)
            {
                lock (NotificationManager.mutex)
                {
                    if (NotificationManager.managerSingleton == null)
                    {
                        NotificationManager.managerSingleton = new NotificationManager(contentManager, spriteBatch, displayDimensions);
                    }
                }
            }

            return NotificationManager.managerSingleton;
        }

        /// <summary>
        /// Adds a notification to the notification manager.
        /// </summary>
        /// <param name="type">The type of notification to add.</param>
        public static void AddNotification(NotificationType type)
        {
            lock (NotificationManager.mutex)
            {
                NotificationManager.managerSingleton.notificationQueue.Enqueue(NotificationManager.managerSingleton.notificationFactory.Create(type));
            }
        }

        /// <summary>
        /// Updates the currently displayed notification, if any.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="closeNotification">A value indicating whether to force close the currently displayed notification or not.</param>
        public void Update(GameTime gameTime, bool closeNotification)
        {
            lock (NotificationManager.mutex)
            {
                if (this.notificationQueue.Count > 0)
                {
                    this.notificationQueue.Peek().Update(gameTime, closeNotification);
                    if (!this.notificationQueue.Peek().Active)
                    {
                        this.notificationQueue.Dequeue();
                    }
                }
            }
        }

        /// <summary>
        /// Draws the currently active notification.
        /// </summary>
        public void Draw()
        {
            lock (NotificationManager.mutex)
            {
                if (this.notificationQueue.Count > 0)
                {
                    this.notificationQueue.Peek().Draw();
                    RenderableText.Draw(this.closeInformation, this.textPosition, 0.0f);
                }
            }
        }
    }
}
