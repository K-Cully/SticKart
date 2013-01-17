// -----------------------------------------------------------------------
// <copyright file="MenuType.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

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
        /// The leaderboard select menu.
        /// </summary>
        LeaderboardSelect,

        /// <summary>
        /// The level selection menu.
        /// </summary>
        LevelSelect,

        /// <summary>
        /// The name prompt menu.
        /// </summary>
        NamePrompt,

        /// <summary>
        /// The letter input menu.
        /// </summary>
        LetterInput,

        /// <summary>
        /// The level complete menu.
        /// </summary>
        LevelComplete
    }
}
