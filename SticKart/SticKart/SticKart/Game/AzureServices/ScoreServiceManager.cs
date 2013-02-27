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
            bool highScore = false;
            IQueryable<HighScore> queryScores = from score in table where score.Level == levelNumber orderby score.Score ascending select score;
            //HighScore lowest = scores.First();
            DataServiceCollection<HighScore> scores = new DataServiceCollection<HighScore>(queryScores);
            if (scores.Count() < 10)
            {
                context.AddToHighScores(HighScore.CreateHighScore(0, levelNumber, scoreNamePair.Name, scoreNamePair.Score));
                context.SaveChanges();
                highScore = true;
            }
            else if (scoreNamePair.CompareTo(new ScoreNamePair(scores.First().Score, scores.First().Name)) < 0)
            {
                // TODO: get delete or update working
                context.DeleteObject(scores.First());
                //lowest.Score = scoreNamePair.Score;
                //lowest.Name = scoreNamePair.Name;
                //context.UpdateObject(lowest);
                //HighScore temp = scores.First();
                context.AddToHighScores(HighScore.CreateHighScore(0, levelNumber, scoreNamePair.Name, scoreNamePair.Score));
                context.SaveChanges();
                highScore = true;
            }

            return highScore;
        }

        public bool AddScore(ScoreNamePair scoreNamePair, int levelNumber)
        {
            bool scoreAdded = false;
            try
            {
                scoreAdded = this.AddScoreToTable(context.HighScores, scoreNamePair, levelNumber);
            }
            catch (WebException)
            {
                scoreAdded = false;
            }
            catch (DataServiceQueryException)
            {
                scoreAdded = false;
            }
            catch (DataServiceRequestException e)
            {
                scoreAdded = false;
                Console.WriteLine(e.InnerException.Message);
            }

            return scoreAdded;
        }
    }
}
