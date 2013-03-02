// -----------------------------------------------------------------------
// <copyright file="ScoreServiceManager.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.AzureServices
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Net;
    using Game.Level;
    using SticKartScores;

    /// <summary>
    /// Manages connection to and querying of the cloud hosted high score service.
    /// </summary>
    public sealed class ScoreServiceManager
    {
        /// <summary>
        /// The <see cref="ScoreServiceManager"/> singleton.
        /// </summary>
        private static ScoreServiceManager managerSingleton = null;

        /// <summary>
        /// The context to use when querying the score service.
        /// </summary>
        private SticKartScores_0Entities context;
        
        /// <summary>
        /// Prevents a default instance of the <see cref="ScoreServiceManager"/> class from being created.
        /// </summary>
        private ScoreServiceManager()
        {
            try
            {
                this.context = new SticKartScores_0Entities(new Uri(ScoreServiceConstants.ServiceUriString));
                this.context.Credentials = new NetworkCredential(ScoreServiceConstants.AppLoginName, ScoreServiceConstants.AppLoginPassword);
                this.AddScoreToTable(new ScoreNamePair(0, "AAA"), 1);
            }
            catch (Exception)
            {
                this.context = null;
            }
        }

        /// <summary>
        /// Gets the score service manager singleton, if it has been initialized.
        /// </summary>
        public static ScoreServiceManager Instance
        {
            get
            {
                return ScoreServiceManager.managerSingleton;
            }
        }

        /// <summary>
        /// Initializes the <see cref="ScoreServiceManager"/> singleton.
        /// </summary>
        /// <returns>The newly created score service manager or null if there was an error during initialization.</returns>
        public static ScoreServiceManager Initialize()
        {
            if (ScoreServiceManager.managerSingleton == null)
            {
                ScoreServiceManager.managerSingleton = new ScoreServiceManager();
                if (ScoreServiceManager.managerSingleton.context == null)
                {
                    ScoreServiceManager.managerSingleton = null;
                }
            }

            return ScoreServiceManager.managerSingleton;
        }

        /// <summary>
        /// Adds a score-name pair to the database and checks if it is a new high score or not.
        /// </summary>
        /// <param name="scoreNamePair">The score-name pair to add.</param>
        /// <param name="levelNumber">The level number.</param>
        /// <returns>A value indicating whether the score was a new high score or not.</returns>
        public bool AddScore(ScoreNamePair scoreNamePair, int levelNumber)
        {
            bool scoreAdded = false;
            try
            {
                scoreAdded = this.AddScoreToTable(scoreNamePair, levelNumber);
            }
            catch (WebException)
            {
                scoreAdded = false;
            }
            catch (DataServiceQueryException)
            {
                scoreAdded = false;
            }
            catch (DataServiceRequestException)
            {
                scoreAdded = false;
            }

            return scoreAdded;
        }

        /// <summary>
        /// Retrieves the top ten high score entries for a level.
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        /// <returns>The top ten scores for the level.</returns>
        public Collection<ScoreNamePair> GetScoresFor(int levelNumber)
        {
            Collection<ScoreNamePair> scoreNamePairs = new Collection<ScoreNamePair>();
            try
            {
                Collection<HighScore> scores = new Collection<HighScore>((from score in this.context.HighScores where score.Level == levelNumber orderby score.Score descending select score).ToList());
                for (int count = 0; count < 10; ++count)
                {
                    if (count < scores.Count)
                    {
                        scoreNamePairs.Add(new ScoreNamePair(scores[count].Score, scores[count].Name));
                    }
                    else
                    {
                        scoreNamePairs.Add(new ScoreNamePair(0, "AAA"));
                    }
                }
            }
            catch
            {
                return scoreNamePairs;
            }

            return scoreNamePairs;
        }

        /// <summary>
        /// Creates a query which retrieves all elements in a table.
        /// </summary>
        /// <typeparam name="T">The element type of the table.</typeparam>
        /// <param name="table">The table to query.</param>
        /// <returns>The query of all elements.</returns>
        private IQueryable<T> CreateQueryForAllElements<T>(DataServiceQuery<T> table)
        {
            return from entity in table select entity;
        }

        /// <summary>
        /// Tries to add a score-name pair to a high table.
        /// </summary>
        /// <param name="scoreNamePair">The score-name pair to try to add.</param>
        /// <param name="levelNumber">The level number.</param>
        /// <returns>A value indicating if the score has made it onto the high score table.</returns>
        private bool AddScoreToTable(ScoreNamePair scoreNamePair, int levelNumber)
        {
            bool highScore = false;
            IQueryable<HighScore> scores = from score in this.context.HighScores where score.Level == levelNumber orderby score.Score ascending select score;
            if (scores.Count() < 10)
            {
                this.context.AddToHighScores(HighScore.CreateHighScore(0, levelNumber, scoreNamePair.Name, scoreNamePair.Score));
                this.context.SaveChanges();
                highScore = true;
            }
            else if (scoreNamePair.CompareTo(new ScoreNamePair(scores.First().Score, scores.First().Name)) < 0)
            {
                HighScore lowest = scores.First();
                lowest.Score = scoreNamePair.Score;
                lowest.Name = scoreNamePair.Name;
                this.context.UpdateObject(lowest);
                this.context.SaveChanges();
                highScore = true;
            }

            return highScore;
        }
    }
}
