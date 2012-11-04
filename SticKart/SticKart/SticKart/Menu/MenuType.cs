namespace SticKart.Menu
{
    /// <summary>
    /// An enumeration of different menu types.
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// No menu type. To be used in place of null.
        /// </summary>
        None,
        
        /// <summary>
        /// The main menu.
        /// </summary>
        Main,
        
        /// <summary>
        /// The options menu.
        /// </summary>
        Options,

        /// <summary>
        /// The leaderboard menu.
        /// </summary>
        Leaderboard,

        /// <summary>
        /// The level selection menu.
        /// </summary>
        LevelSelect,

        /// <summary>
        /// The name input menu.
        /// </summary>
        NameInput,

        /// <summary>
        /// The level complete menu.
        /// </summary>
        LevelComplete
    }
}
