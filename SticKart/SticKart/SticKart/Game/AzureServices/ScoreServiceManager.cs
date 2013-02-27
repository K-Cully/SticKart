// -----------------------------------------------------------------------
// <copyright file="ScoreServiceManager.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.AzureServices
{
    using System;
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
        /// Prevents the initialization of the <see cref="ScoreServiceManager"/> class.
        /// </summary>
        private ScoreServiceManager()
        {
            try
            {
                this.context = new SticKartScores_0Entities(new Uri(ScoreServiceConstants.ServiceUriString));
            }
            catch (Exception exception)
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
        /// Initailizes the <see cref="ScoreServiceManager"/> singleton.
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
        /// Creates a query which retrievs all elements in a table.
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
        /// <typeparam name="T">The type of table to add to.</typeparam>
        /// <param name="table">The table to add to.</param>
        /// <param name="scoreNamePair">The score-name pair to try to add.</param>
        /// <param name="levelNumber">The level number.</param>
        /// <returns>A value indicating if the score has made it onto the high score table.</returns>
        private bool AddScoreToTable(DataServiceQuery<HighScore> table, ScoreNamePair scoreNamePair, int levelNumber)
        {
            // Add score if any lower than current score  
            IQueryable<HighScore> query = from score in table where score.Level == levelNumber select score;
            DataServiceCollection<HighScore> scores = new DataServiceCollection<HighScore>(query);
            
            // if count < 10 or is higher than any other element add and possibly remove lowest

            int comparison = 0;
            for (int count = 0; count < scores.Count; count++)
            {
                comparison = scoreNamePair.CompareTo(new ScoreNamePair(scores[count].Score, scores[count].Name));
            }

            return false;
        }

        public bool AddScore(ScoreNamePair scoreNamePair, int levelNumber)
        {
            bool scoreAdded = false;
            try
            {
                scoreAdded = this.AddScoreToTable(context.HighScores, scoreNamePair, levelNumber);

                // Get all scores that are lower than the current score.
                // var getHigherScoresQuery = from scores in context.HighScores_000 where scores.Score < scoreNamePair.Score select scores;
                //this.AddScoreToTable<HighScores_000>(this.CreateQueryForAllElements<HighScores_000>(context.HighScores_000));
                //DataServiceCollection<HighScores_000> customerOrders = new DataServiceCollection<HighScores_000>(getHigherScoresQuery);
            }
            catch (WebException)
            {
                scoreAdded = false;
            }
            catch (DataServiceQueryException)
            {
                scoreAdded = false;
            }

            return scoreAdded;
        }
    }
}
