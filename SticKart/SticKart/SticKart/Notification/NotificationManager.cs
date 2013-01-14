// -----------------------------------------------------------------------
// <copyright file="NotificationManager.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Notification
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Manages the update and display of notifications.
    /// </summary>
    public sealed class NotificationManager
    {
        /// <summary>
        /// The notification manager singleton.
        /// </summary>
        private static volatile NotificationManager manager = null;

        /// <summary>
        /// An object to lock on which ensures thread-safe instanciation of the notification manager.
        /// </summary>
        private static object mutex = new object();
        
        /// <summary>
        /// A queue of notifications to display.
        /// </summary>
        private volatile Queue<Notification> notificationQueue;

        /// <summary>
        /// Prevents initialization of the <see cref="NotificationManager"/> class.
        /// </summary>
        private NotificationManager()
        {
            this.notificationQueue = new Queue<Notification>();
        }

        /// <summary>
        /// Gets the notification manager singleton.
        /// </summary>
        public static NotificationManager Instance
        {
            get
            {
                if (NotificationManager.manager == null)
                {
                    lock (NotificationManager.mutex)
                    {
                        NotificationManager.manager = new NotificationManager();
                    }
                }

                return NotificationManager.manager;
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
    }
}
