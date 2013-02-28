// -----------------------------------------------------------------------
// <copyright file="GameSettings.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Xml.Serialization;
    using AzureServices;
    using Level;

    /// <summary>
    /// Defines a manager for saving and loading game settings.
    /// </summary>
    public class GameSettings
    {
        /// <summary>
        /// The name of the settings file.
        /// </summary>
        private static string filename = "settings.xml";

        /// <summary>
        /// The score data service manager.
        /// </summary>
        private ScoreServiceManager scoreServiceManager;
        
        /// <summary>
        /// Prevents a default instance of the <see cref="GameSettings"/> class from being created.
        /// </summary>
        private GameSettings()
        {
            this.PlayerName = "BOB";
            this.LevelsUnlocked = 1;
            this.TotalLevels = 5;
            this.LevelScoreTables = new Collection<LevelScoreTable>();
            this.scoreServiceManager = ScoreServiceManager.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettings"/> class.
        /// </summary>
        /// <param name="createDefaults">A value indicating that the default lists should be created.</param>
        private GameSettings(int createDefaults)
        {
            this.PlayerName = "BOB";
            this.LevelsUnlocked = 1;
            this.TotalLevels = 5;
            this.LevelScoreTables = new Collection<LevelScoreTable>();
            this.scoreServiceManager = ScoreServiceManager.Initialize();
            for (int count = 0; count < this.TotalLevels; ++count)
            {
                this.LevelScoreTables.Add(new LevelScoreTable(1));
            }
        }

        /// <summary>
        /// Gets or sets the collection of level score tables.
        /// </summary>
        public Collection<LevelScoreTable> LevelScoreTables { get; set; }

        /// <summary>
        /// Gets or sets the current player name.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the number of levels the player has unlocked.
        /// </summary>
        public int LevelsUnlocked { get; set; }

        /// <summary>
        /// Gets or sets the total number of levels in the game.
        /// </summary>
        public int TotalLevels { get; set; }

        /// <summary>
        /// Loads the game settings from persistent storage.
        /// If the file does not exist a default object is created.
        /// </summary>
        /// <returns>The game settings.</returns>
        public static GameSettings Load()
        {
#if WINDOWS_PHONE
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
#else
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain())
#endif
            {
                GameSettings settings;
                if (storage.FileExists(GameSettings.filename))
                {
                    using (IsolatedStorageFileStream stream = storage.OpenFile(GameSettings.filename, FileMode.Open))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(GameSettings));
                        settings = xml.Deserialize(stream) as GameSettings;
                    }
                }
                else
                {
                    settings = new GameSettings(1);
                }

                return settings;
            }
        }

        /// <summary>
        /// Removes the file from persistent storage.
        /// </summary>
        public static void Clear()
        {
#if WINDOWS_PHONE
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
#else
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain())
#endif
            {
                if (storage.FileExists(GameSettings.filename))
                {
                    storage.DeleteFile(GameSettings.filename);
                }
            }
        }

        /// <summary>
        /// Saves the current game settings to persistent storage.
        /// </summary>
        public void Save()
        {
#if WINDOWS_PHONE
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
#else
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain())
#endif
            {
                using (IsolatedStorageFileStream stream = storage.CreateFile(GameSettings.filename))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(GameSettings));
                    xml.Serialize(stream, this);
                }
            }
        }

        /// <summary>
        /// Tries to add the score to the high score table for the level specified. 
        /// </summary>
        /// <param name="levelNumber">The level number to add the score to.</param>
        /// <param name="score">The score to add.</param>
        /// <returns>The type of high score set, if any.</returns>
        public HighScoreType AddScore(int levelNumber, int score)
        {
            HighScoreType scoreSet = HighScoreType.None;
            if (levelNumber < 1 || levelNumber > this.TotalLevels)
            {
                scoreSet = HighScoreType.None;
            }
            else
            {
                if (this.LevelScoreTables[levelNumber - 1].AddScore(new ScoreNamePair(score, this.PlayerName)))
                {
                    scoreSet = HighScoreType.Local;
                }

                if (this.scoreServiceManager.AddScore(new ScoreNamePair(score, this.PlayerName), levelNumber))
                {
                    scoreSet = HighScoreType.Global;
                }
            }

            return scoreSet;
        }
    }
}
