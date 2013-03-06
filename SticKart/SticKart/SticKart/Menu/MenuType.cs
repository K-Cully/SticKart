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
        /// The leaderboard type select menu.
        /// </summary>
        LeaderboardTypeSelect,

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
        /// The first letter input sub menu.
        /// </summary>
        LetterInputA,

        /// <summary>
        /// The second letter input sub menu.
        /// </summary>
        LetterInputG,

        /// <summary>
        /// The third letter input sub menu.
        /// </summary>
        LetterInputM,

        /// <summary>
        /// The fourth letter input sub menu.
        /// </summary>
        LetterInputS,

        /// <summary>
        /// The fifth letter input sub menu.
        /// </summary>
        LetterInputY,

        /// <summary>
        /// The sixth letter input sub menu.
        /// </summary>
        LetterInput4,

        /// <summary>
        /// The level complete menu.
        /// </summary>
        LevelComplete,

        /// <summary>
        /// The custom content menu.
        /// </summary>
        CustomContent,

        /// <summary>
        /// The edit level select menu.
        /// </summary>
        EditLevelSelect,
        
        /// <summary>
        /// The editor overlay menu.
        /// </summary>
        EditorOverlay
    }
}
