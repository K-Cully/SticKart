namespace SticKart.Game.Level
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
 
    /// <summary>
    /// Loads all the entities for a level.
    /// </summary>
    public class LevelLoader
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
        /// The sub path to the list of level entities which the player can interact with.
        /// </summary>
        private const string InteractiveSubPath = "interactiveEntities";

        /// <summary>
        /// The content manager to use for loading data.
        /// </summary>
        private ContentManager contentManager;

        /// <summary>
        /// The list of points defining the level's ground, start point and end point.
        /// </summary>
        private List<Vector2> levelPoints;
        
        /// <summary>
        /// The list of platform descriptions.
        /// </summary>
        private List<PlatformDescription> platformDescriptions;

        /// <summary>
        /// The list of obstacle, power up and bonus descriptions. 
        /// </summary>
        private List<InteractiveEntityDescription> interactiveEntityDescriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelLoader"/> class.
        /// </summary>
        /// <param name="contentManager">The content manager to use when loading.</param>
        public LevelLoader(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            this.StartPosition = Vector2.Zero;
            this.EndPosition = Vector2.Zero;
            this.levelPoints = new List<Vector2>();
            this.platformDescriptions = new List<PlatformDescription>();
            this.interactiveEntityDescriptions = new List<InteractiveEntityDescription>();
        }

        /// <summary>
        /// Gets the player's initial level position.
        /// </summary>
        public Vector2 StartPosition { get; private set; }

        /// <summary>
        /// Gets the position to place the end of the level.
        /// </summary>
        public Vector2 EndPosition { get; private set; }
        
        /// <summary>
        /// Gets the list of points defining the floor.
        /// </summary>
        public List<Vector2> FloorPoints
        {
            get
            {
                return this.levelPoints;
            }
        }

        /// <summary>
        /// Gets the list of interactive object descriptions.
        /// </summary>
        public List<InteractiveEntityDescription> InteractiveDescriptions
        {
            get
            {
                return this.interactiveEntityDescriptions;
            }
        }

        /// <summary>
        /// Gets the list of platform object descriptions.
        /// </summary>
        public List<PlatformDescription> PlatformDescriptions
        {
            get
            {
                return this.platformDescriptions;
            }
        }

        /// <summary>
        /// Loads the contents of a level.
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        /// <param name="isCustom">Whether the level is a custom level or not.</param>
        public void LoadLevel(int levelNumber, bool isCustom)
        {
            if (isCustom)
            {
                // TODO: load from isolated/local storage
            }
            else
            {
                this.levelPoints = new List<Vector2>(this.contentManager.Load<Vector2[]>(LevelLoader.StandardFilePath + levelNumber.ToString() + "/" + LevelLoader.PointSubPath));
                this.platformDescriptions = new List<PlatformDescription>(this.contentManager.Load<PlatformDescription[]>(LevelLoader.StandardFilePath + levelNumber.ToString() + "/" + LevelLoader.PlatformSubPath));
                this.interactiveEntityDescriptions = new List<InteractiveEntityDescription>(this.contentManager.Load<InteractiveEntityDescription[]>(LevelLoader.StandardFilePath + levelNumber.ToString() + "/" + LevelLoader.InteractiveSubPath));
                this.StartPosition = this.levelPoints[0];
                this.levelPoints.RemoveAt(0);
                this.EndPosition = this.levelPoints[0];
                this.levelPoints.RemoveAt(0);
            }
        }
    }
}
