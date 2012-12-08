// -----------------------------------------------------------------------
// <copyright file="LevelConstants.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Level
{
    /// <summary>
    /// Defines constants used by a level.
    /// </summary>
    public class LevelConstants
    {
        /// <summary>
        /// The delay at the start of a level before the scrolling death entity activates.
        /// </summary>
        public const float ScrollStartDelay = 3.0f;

        /// <summary>
        /// The minimum screen scroll rate.
        /// </summary>
        public const float MinimumScrollRate = 2.0f;

        /// <summary>
        /// The normal screen scroll rate.
        /// </summary>
        public const float ScrollRate = 3.0f;

        /// <summary>
        /// The maximum screen scroll rate.
        /// </summary>
        public const float MaximumScrollRate = 50.0f;

        /// <summary>
        /// The acceleration of the screen scroll rate.
        /// </summary>
        public const float ScrollAcceleration = 80.0f;

        /// <summary>
        /// The deceleration of the screen scroll rate.
        /// </summary>
        public const float ScrollDeceleration = -120.0f;

        /// <summary>
        /// The file path to the levels folder.
        /// </summary>
        public const string StandardFilePath = "Levels/";

        /// <summary>
        /// The sub path to the list of points which define the level's floor.
        /// </summary>
        public const string PointSubPath = "points";

        /// <summary>
        /// The sub path to the list of platforms.
        /// </summary>
        public const string PlatformSubPath = "platforms";

        /// <summary>
        /// The sub path to the list of level entities which the player can interact with.
        /// </summary>
        public const string InteractiveSubPath = "interactiveEntities";
    }
}
