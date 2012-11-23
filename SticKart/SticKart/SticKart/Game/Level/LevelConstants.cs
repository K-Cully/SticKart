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
