// -----------------------------------------------------------------------
// <copyright file="ContentLocations.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display
{
    /// <summary>
    /// Contains the locations of image and font assets, relative to the root content directory.
    /// </summary>
    public class ContentLocations
    {
        #region fonts

        /// <summary>
        /// The location of the Segoe UI sprite font asset.
        /// </summary>
        public const string SegoeUIFont = "Fonts/SegoeUIMono";

        /// <summary>
        /// The location of the medium Segoe UI sprite font asset.
        /// </summary>
        public const string SegoeUIFontMedium = "Fonts/SegoeUIMonoMedium";

        /// <summary>
        /// The location of the large Segoe UI sprite font asset.
        /// </summary>
        public const string SegoeUIFontLarge = "Fonts/SegoeUIMonoLarge";

        #endregion

        #region notifications

        /// <summary>
        /// The path to the directory containing sprites used by notifications.
        /// </summary>
        public const string NotificationsPath = "Sprites/Notifications/";

        /// <summary>
        /// The name of the background sprite asset.
        /// </summary>
        public const string Background = "Background";
        
        /// <summary>
        /// The name of the push animated sprite asset.
        /// </summary>
        public const string Push = "push";

        #endregion

        #region hud

        /// <summary>
        /// The path to the directory containing sprites used by the heads up display.
        /// </summary>
        public const string HudPath = "Sprites/Hud/";

        /// <summary>
        /// The name of the power up box asset.
        /// </summary>
        public const string PowerUpBox = "powerUpBox";
        
        /// <summary>
        /// The name of the invincible icon asset.
        /// </summary>
        public const string InvincibleIcon = "invincible";

        /// <summary>
        /// The name of the health icon asset.
        /// </summary>
        public const string HealthIcon = "health";

        /// <summary>
        /// The name of the jump icon asset.
        /// </summary>
        public const string JumpIcon = "jump";

        /// <summary>
        /// The name of the speed icon asset.
        /// </summary>
        public const string SpeedIcon = "speed";

        /// <summary>
        /// The name of the white pixel asset.
        /// </summary>
        public const string WhitePixel = "whitePixel";
        
        #endregion

        #region menu

        /// <summary>
        /// The location of the hand icon texture asset.
        /// </summary>
        public const string HandIcon = "Sprites/Menu/Hand";
        
        /// <summary>
        /// The location of the large tile texture asset.
        /// </summary>
        public const string LargeButtonTile = "Sprites/Menu/Button_large";
        
        /// <summary>
        /// The location of the large play icon texture asset.
        /// </summary>
        public const string LargePlayIcon = "Sprites/Menu/Play_icon_large";

        /// <summary>
        /// The location of the large options icon texture asset.
        /// </summary>
        public const string LargeOptionsIcon = "Sprites/Menu/Options_icon_large";

        /// <summary>
        /// The location of the large exit icon texture asset.
        /// </summary>
        public const string LargeExitIcon = "Sprites/Menu/Exit_icon_large";

        /// <summary>
        /// The location of the large back icon texture asset.
        /// </summary>
        public const string LargeBackIcon = "Sprites/Menu/Back_icon_large";

        /// <summary>
        /// The location of the large leaderboard icon texture asset.
        /// </summary>
        public const string LargeLeaderboardIcon = "Sprites/Menu/Leaderboard_icon_large";

        #endregion

        #region effects

        /// <summary>
        /// The path to the Kinect colour visualizer effect.
        /// </summary>
        public const string KinectColourEffect = "Effects/KinectColorVisualizer";

        #endregion

        #region scenery

        /// <summary>
        /// The location of the scenery sprite assets.
        /// </summary>
        public const string Scenery = "Sprites/Scenery/";

        /// <summary>
        /// The name of the rock terrain texture asset.
        /// </summary>
        public const string RockyTerrain = "RockyTexture";

        /// <summary>
        /// The name of the rock background texture asset.
        /// </summary>
        public const string RockyBackGround = "BackgroundRocky";

        #endregion
    }
}
