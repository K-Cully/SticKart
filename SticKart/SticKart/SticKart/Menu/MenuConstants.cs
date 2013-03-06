// -----------------------------------------------------------------------
// <copyright file="MenuConstants.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Menu
{
    using Game.Level;

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
        /// The name of the custom content button.
        /// </summary>
        public const string CustomButtonName = "CUSTOM CONTENT";

        /// <summary>
        /// The name of the new level button.
        /// </summary>
        public const string NewButtonName = "NEW LEVEL";

        /// <summary>
        /// The name of the edit level button.
        /// </summary>
        public const string EditButtonName = "EDIT LEVEL";

        /// <summary>
        /// The name of the global button.
        /// </summary>
        public const string GlobalButtonName = "GLOBAL";

        /// <summary>
        /// The name of the local button.
        /// </summary>
        public const string LocalButtonName = "LOCAL";

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

        /// <summary>
        /// The name of the music toggle command.
        /// </summary>
        public const string MusicButtonName = "TOGGLE MUSIC";

        /// <summary>
        /// The name of the sound effects toggle command.
        /// </summary>
        public const string SoundEffectsButtonName = "TOGGLE SOUND EFFECTS";

        /// <summary>
        /// The name of the high score upload toggle command.
        /// </summary>
        public const string UploadButtonName = "TOGGLE SCORE UPLOADS";
        
        /// <summary>
        /// The name of the notification reset command.
        /// </summary>
        public const string NotificationButtonName = "RESET HINTS";

        /// <summary>
        /// The name of the set name command.
        /// </summary>
        public const string NameButtonName = "SET PLAYER NAME";

        #endregion

        #region characterSelect

        /// <summary>
        /// The text to display on the a to f character select button.
        /// </summary>
        public const string AToF = "a-f";

        /// <summary>
        /// The text to display on the g to l character select button.
        /// </summary>
        public const string GToL = "g-l";

        /// <summary>
        /// The text to display on the m to r character select button.
        /// </summary>
        public const string MToR = "m-r";

        /// <summary>
        /// The text to display on the s to x character select button.
        /// </summary>
        public const string SToX = "s-x";

        /// <summary>
        /// The text to display on the y to 3 character select button.
        /// </summary>
        public const string YToThree = "y-3";

        /// <summary>
        /// The text to display on the 4 to 9 character select button.
        /// </summary>
        public const string FourToNine = "4-9";

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
        /// The global high score text.
        /// </summary>
        private const string HighScoreGlobalText = "Congratulations, you set a global high score.";

        /// <summary>
        /// The local high score text.
        /// </summary>
        private const string HighScoreLocalText = "Well done, you set a local high score.";

        /// <summary>
        /// The no high score text.
        /// </summary>
        private const string HighScoreNoneText = "Unlucky, you didn't set a high score this time.";

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
        /// <param name="highScoreSet">A value indicating what high score was set, if any.</param>
        /// <returns>The appropriate high score prompt text.</returns>
        public static string GetHighScoreText(HighScoreType highScoreSet)
        {
            switch (highScoreSet)
            {
                case HighScoreType.Global:
                    return MenuConstants.HighScoreGlobalText;
                case HighScoreType.Local:
                    return MenuConstants.HighScoreLocalText;
                case HighScoreType.None:
                    return MenuConstants.HighScoreNoneText;
                default:
                    return MenuConstants.HighScoreNoneText;
            }
        }
    }
}
