// -----------------------------------------------------------------------
// <copyright file="LevelScoreTable.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Level
{
    using System.Collections.Generic;

    /// <summary>
    /// Manages high scores for a level.
    /// </summary>
    public class LevelScoreTable
    {
        /// <summary>
        /// The maximum scores to store for any level.
        /// </summary>
        private const int ScoresPerLevel = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelScoreTable"/> class.
        /// </summary>
        public LevelScoreTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelScoreTable"/> class.
        /// </summary>
        /// <param name="createDefaults">A value indicating that the default values should be populated.</param>
        public LevelScoreTable(int createDefaults)
        {
            this.Scores = new List<ScoreNamePair>(LevelScoreTable.ScoresPerLevel);
            for (int count = 0; count < LevelScoreTable.ScoresPerLevel; count++)
            {
                this.Scores.Add(new ScoreNamePair());
            }

            this.Scores.Sort();
        }

        /// <summary>
        /// Gets or sets the list of scores in the table.
        /// </summary>
        public List<ScoreNamePair> Scores { get; set; }

        /// <summary>
        /// Tries to add a score name pair to the table.
        /// </summary>
        /// <param name="scoreNamePair">The score name pair to add.</param>
        /// <returns>A value indicating whether the pair was added or not.</returns>
        public bool AddScore(ScoreNamePair scoreNamePair)
        {
            if (scoreNamePair.CompareTo(this.Scores[this.Scores.Count - 1]) < 0)
            {
                this.Scores.RemoveAt(this.Scores.Count - 1);
                this.Scores.Add(scoreNamePair);
                this.Scores.Sort();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
