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
        /// The minimum screen scroll rate.
        /// </summary>
        public const float MinimumScrollRate = 10.0f;

        /// <summary>
        /// The normal screen scroll rate.
        /// </summary>
        public const float ScrollRate = 40.0f;

        /// <summary>
        /// The maximum screen scroll rate.
        /// </summary>
        public const float MaximumScrollRate = 100.0f;

        /// <summary>
        /// The acceleration of the screen scroll rate.
        /// </summary>
        public const float ScrollAcceleration = 2.0f;

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
