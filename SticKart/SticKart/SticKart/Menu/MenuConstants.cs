﻿// -----------------------------------------------------------------------
// <copyright file="MenuConstants.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Menu
{
    /// <summary>
    /// A class containing all the menu string constants.
    /// </summary>
    public class MenuConstants
    {
        #region names

        /// <summary>
        /// The name of the options button.
        /// </summary>
        public const string OptionsButtonName = "OPTIONS";

        /// <summary>
        /// The name of the options button.
        /// </summary>
        public const string PlayButtonName = "PLAY";

        /// <summary>
        /// The name of the leaderboard button.
        /// </summary>
        public const string LeaderboardButtonName = "LEADERBOARD";

        /// <summary>
        /// The name of the exit button.
        /// </summary>
        public const string ExitButtonName = "EXIT";

        /// <summary>
        /// The name of the back button.
        /// </summary>
        public const string BackButtonName = "BACK";

        /// <summary>
        /// The name of the continue button.
        /// </summary>
        public const string ContinueButtonName = "CONTINUE";

        /// <summary>
        /// The name of the retry button.
        /// </summary>
        public const string RetryButtonName = "RETRY";

        /// <summary>
        /// The name of the pause command.
        /// </summary>
        public const string PauseCommandName = "PAUSE GAME";

        #endregion

        /// <summary>
        /// The level complete text.
        /// </summary>
        public const string LevelCompleteText = "Level Complete";

        /// <summary>
        /// The default high score text.
        /// </summary>
        public const string HighScoreText = "Congratulations, you set a high score.";

        /// <summary>
        /// The alternate high score text.
        /// </summary>
        public const string HighScoreAlternateText = "Unlucky, you didn't set a high score this time.";

        /// <summary>
        /// The score heading text.
        /// </summary>
        public const string ScoreText = "Your score was:";

        /// <summary>
        /// The default score.
        /// </summary>
        public const string ScoreDefaultText = "0";

        /// <summary>
        /// The rating heading text.
        /// </summary>
        public const string RatingText = "Rating:";

        /// <summary>
        /// The lowest rating.
        /// </summary>
        private const string RatingGood = "Good";

        /// <summary>
        /// The medium rating.
        /// </summary>
        private const string RatingGreat = "Great";

        /// <summary>
        /// The best rating.
        /// </summary>
        private const string RatingExcellent = "Excellent";

        /// <summary>
        /// Retrieves the rating text.
        /// </summary>
        /// <param name="ratingLevel">The rating level.</param>
        /// <returns>The rating text.</returns>
        public static string GetRating(int ratingLevel)
        {
            ratingLevel = ratingLevel < 0 ? 0 : ratingLevel > 2 ? 2 : ratingLevel;

            switch (ratingLevel)
            {
                case 0:
                    return MenuConstants.RatingGood;
                case 1:
                    return MenuConstants.RatingGreat;
                case 2:
                    return MenuConstants.RatingExcellent;
                default:
                    return string.Empty;
            }
        }
    }
}