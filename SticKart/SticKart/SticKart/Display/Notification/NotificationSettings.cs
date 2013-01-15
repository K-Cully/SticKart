// -----------------------------------------------------------------------
// <copyright file="NotificationSettings.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display.Notification
{
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Xml.Serialization;

    /// <summary>
    /// Keeps track of which notifications are active.
    /// </summary>
    public class NotificationSettings
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="NotificationSettings"/> class from being created.
        /// </summary>
        private NotificationSettings()
        {
            this.Reset();
        }

        #region properties

        /// <summary>
        /// Gets or sets a value indicating whether the push gesture notification is enabled or not. 
        /// </summary>
        public bool Push { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the swipe gesture notification is enabled or not. 
        /// </summary>
        public bool Swipe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the run gesture notification is enabled or not. 
        /// </summary>
        public bool Run { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the jump gesture notification is enabled or not. 
        /// </summary>
        public bool Jump { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the crouch gesture notification is enabled or not. 
        /// </summary>
        public bool Crouch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the jump down gesture notification is enabled or not. 
        /// </summary>
        public bool JumpDown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the exit notification is enabled or not. 
        /// </summary>
        public bool Exit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mine cart notification is enabled or not. 
        /// </summary>
        public bool Cart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bonus notification is enabled or not. 
        /// </summary>
        public bool Bonus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the obstacle notification is enabled or not. 
        /// </summary>
        public bool Obstacle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scrolling death notification is enabled or not. 
        /// </summary>
        public bool ScrollingDeath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the voice command notification is enabled or not. 
        /// </summary>
        public bool Voice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the power-up notification is enabled or not. 
        /// </summary>
        public bool PowerUp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the switch notification is enabled or not. 
        /// </summary>
        public bool Switch { get; set; }

        #endregion

        /// <summary>
        /// Loads the notification settings from persistent storage.
        /// If the file does not exist a default object is created.
        /// </summary>
        /// <returns>The notification settings.</returns>
        public static NotificationSettings Load()
        {
#if WINDOWS_PHONE
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
#else
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain())
#endif
            {
                NotificationSettings settings;
                if (storage.FileExists(NotificationStrings.SettingsFileName))
                {
                    using (IsolatedStorageFileStream stream = storage.OpenFile(NotificationStrings.SettingsFileName, FileMode.Open))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(NotificationSettings));
                        settings = xml.Deserialize(stream) as NotificationSettings;
                    }
                }
                else
                {
                    settings = new NotificationSettings();
                }

                return settings;
            }
        }

        /// <summary>
        /// Removes the notification settings file from persistent storage.
        /// </summary>
        public static void Clear()
        {
#if WINDOWS_PHONE
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
#else
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain())
#endif
            {
                if (storage.FileExists(NotificationStrings.SettingsFileName))
                {
                    storage.DeleteFile(NotificationStrings.SettingsFileName);
                }
            }
        }

        /// <summary>
        /// Saves the current notification settings to persistent storage.
        /// </summary>
        public void Save()
        {
#if WINDOWS_PHONE
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
#else
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain())
#endif
            {
                using (IsolatedStorageFileStream stream = storage.CreateFile(NotificationStrings.SettingsFileName))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(NotificationSettings));
                    xml.Serialize(stream, this);
                }
            }
        }

        /// <summary>
        /// Disables the notification of the type passed in.
        /// </summary>
        /// <param name="type">The type of notification to disable.</param>
        public void DisableNotification(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.None:
                    break;
                case NotificationType.StepBack:
                    break;
                case NotificationType.PushGesture:
                    this.Push = false;
                    break;
                case NotificationType.SwipeGesture:
                    this.Swipe = false;
                    break;
                case NotificationType.VoiceCommand:
                    this.Voice = false;
                    break;
                case NotificationType.Run:
                    this.Run = false;
                    break;
                case NotificationType.JumpUp:
                    this.Jump = false;
                    break;
                case NotificationType.JumpDown:
                    this.JumpDown = false;
                    break;
                case NotificationType.Crouch:
                    this.Crouch = false;
                    break;
                case NotificationType.ScrollingDeath:
                    this.ScrollingDeath = false;
                    break;
                case NotificationType.Exit:
                    this.Exit = false;
                    break;
                case NotificationType.Cart:
                    this.Cart = false;
                    break;
                case NotificationType.Bonus:
                    this.Bonus = false;
                    break;
                case NotificationType.Obstacle:
                    this.Obstacle = false;
                    break;
                case NotificationType.PowerUp:
                    this.PowerUp = false;
                    break;
                case NotificationType.Switch:
                    this.Switch = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Checks if a notification type is enabled.
        /// </summary>
        /// <param name="type">The type of notification.</param>
        /// <returns>A value indicating if the type of notification is enabled or not.</returns>
        public bool IsNotificationEnabled(NotificationType type)
        {
            bool enabled = false;
            switch (type)
            {
                case NotificationType.None:
                    enabled = false;
                    break;
                case NotificationType.StepBack:
                    enabled = true;
                    break;
                case NotificationType.PushGesture:
                    enabled = this.Push;
                    break;
                case NotificationType.SwipeGesture:
                    enabled = this.Swipe;
                    break;
                case NotificationType.VoiceCommand:
                    enabled = this.Voice;
                    break;
                case NotificationType.Run:
                    enabled = this.Run;
                    break;
                case NotificationType.JumpUp:
                    enabled = this.Jump;
                    break;
                case NotificationType.JumpDown:
                    enabled = this.JumpDown;
                    break;
                case NotificationType.Crouch:
                    enabled = this.Crouch;
                    break;
                case NotificationType.ScrollingDeath:
                    enabled = this.ScrollingDeath;
                    break;
                case NotificationType.Exit:
                    enabled = this.Exit;
                    break;
                case NotificationType.Cart:
                    enabled = this.Cart;
                    break;
                case NotificationType.Bonus:
                    enabled = this.Bonus;
                    break;
                case NotificationType.Obstacle:
                    enabled = this.Obstacle;
                    break;
                case NotificationType.PowerUp:
                    enabled = this.PowerUp;
                    break;
                case NotificationType.Switch:
                    enabled = this.Switch;
                    break;
                default:
                    enabled = false;
                    break;
            }

            return enabled;
        }

        /// <summary>
        /// Re-enables all notifications.
        /// </summary>
        public void Reset()
        {
            this.Bonus = true;
            this.Cart = true;
            this.Crouch = true;
            this.Exit = true;
            this.Jump = true;
            this.JumpDown = true;
            this.Obstacle = true;
            this.PowerUp = true;
            this.Push = true;
            this.Run = true;
            this.ScrollingDeath = true;
            this.Swipe = true;
            this.Switch = true;
            this.Voice = true;
        }
    }
}
