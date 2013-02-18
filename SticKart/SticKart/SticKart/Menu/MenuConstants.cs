// -----------------------------------------------------------------------
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
        /// The name of the done button.
        /// </summary>
        public const string DoneButtonName = "DONE";

        /// <summary>
        /// The name of the retry button.
        /// </summary>
        public const string RetryButtonName = "RETRY";

        /// <summary>
        /// The name of the pause command.
        /// </summary>
        public const string PauseCommandName = "PAUSE GAME";

        #endregion

        #region charecterSelect

        /// <summary>
        /// The text to display on the a to e charecter select button.
        /// </summary>
        public const string AToE = "a-e";

        /// <summary>
        /// The text to display on the f to j charecter select button.
        /// </summary>
        public const string FToJ = "f-j";

        /// <summary>
        /// The text to display on the k to o charecter select button.
        /// </summary>
        public const string KToO = "k-o";

        /// <summary>
        /// The text to display on the p to t charecter select button.
        /// </summary>
        public const string PToT = "p-t";

        /// <summary>
        /// The text to display on the u to y charecter select button.
        /// </summary>
        public const string UToY = "u-y";

        /// <summary>
        /// The text to display on the z to tilda charecter select button.
        /// </summary>
        public const string ZToTilda = "z-~";

        #endregion

        /// <summary>
        /// The hello text.
        /// </summary>
        public const string HelloText = "Hello";

        /// <summary>
        /// The enter name text.
        /// </summary>
        public const string EnterNameText = "Please enter your name below.";

        /// <summary>
        /// The select a letter text.
        /// </summary>
        public const string SelectLetterText = "Select a letter to change it.";

        /// <summary>
        /// The press done text.
        /// </summary>
        public const string PressDoneText = "Press done when you are finished.";

        /// <summary>
        /// The level complete text.
        /// </summary>
        public const string LevelCompleteText = "Level Complete";
        
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
        /// The default high score text.
        /// </summary>
        private const string HighScoreText = "Congratulations, you set a high score.";

        /// <summary>
        /// The alternate high score text.
        /// </summary>
        private const string HighScoreAlternateText = "Unlucky, you didn't set a high score this time.";

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

        /// <summary>
        /// Retrieves the value for the high score prompt.
        /// </summary>
        /// <param name="setHighScore">A value indicating whether the player set a high score or not.</param>
        /// <returns>The appropriate high score prompt text.</returns>
        public static string GetHighScoreText(bool setHighScore)
        {
            if (setHighScore)
            {
                return MenuConstants.HighScoreText;
            }
            else
            {
                return MenuConstants.HighScoreAlternateText;
            }
        }
    }
}
