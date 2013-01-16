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
    }
}
