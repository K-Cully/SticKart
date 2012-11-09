namespace SticKart.Game.Level
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
 
    /// <summary>
    /// Loads all the entities for a level.
    /// </summary>
    public static class LevelLoader
    {
        /// <summary>
        /// The file path to the levels folder.
        /// </summary>
        private const string StandardFilePath = "Levels/";

        /// <summary>
        /// The sub path to the list of points which define the level's floor.
        /// </summary>
        private const string PointSubPath = "points";

        /// <summary>
        /// The sub path to the list of platforms.
        /// </summary>
        private const string PlatformSubPath = "platforms";

        /// <summary>
        /// The sub path to the list of level entities which the player can collide with.
        /// </summary>
        private const string CollidableSubPath = "collidables";

        /// <summary>
        /// The list of points defining the level's ground, start point and end point.
        /// </summary>
        private static List<Vector2> levelPoints;

        /// <summary>
        /// The list of platform descriptions.
        /// </summary>
        private static List<PlatformDescription> platformDescriptions;

        /// <summary>
        /// The list of obstacle, power up and bonus descriptions. 
        /// </summary>
        private static List<CollidableDescription> collidableDescriptions;

        /// <summary>
        /// Loads the contents of a level.
        /// </summary>
        /// <param name="content">The game's content manager.</param>
        /// <param name="levelNumber">The level number.</param>
        /// <param name="isCustom">If the level is a custom level or not</param>
        public static void LoadLevel(ContentManager content, int levelNumber, bool isCustom)
        {
            if (isCustom)
            {
                // TODO: load from isolated/local storage
            }
            else
            {

                // TODO: return level object?
                LevelLoader.levelPoints = new List<Vector2>(content.Load<Vector2[]>(LevelLoader.StandardFilePath + levelNumber.ToString() + "/" + LevelLoader.PointSubPath));
                LevelLoader.platformDescriptions = new List<PlatformDescription>(content.Load<PlatformDescription[]>(LevelLoader.StandardFilePath + levelNumber.ToString() + "/" + LevelLoader.PlatformSubPath));
                LevelLoader.collidableDescriptions = new List<CollidableDescription>(content.Load<CollidableDescription[]>(LevelLoader.StandardFilePath + levelNumber.ToString() + "/" + LevelLoader.CollidableSubPath));
            }
        }
    }
}
