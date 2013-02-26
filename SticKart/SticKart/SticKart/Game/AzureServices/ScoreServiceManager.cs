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
            return from scores in table select scores;
        }
        

        /// <summary>
        /// Tries to add a score-name pair to a high table.
        /// </summary>
        /// <typeparam name="T">The type of table to add to.</typeparam>
        /// <param name="table">The table to add to.</param>
        /// <param name="scoreNamePair">The score-name pair to try to add.</param>
        /// <param name="typeOfT">The type of the template paramater.</param>
        /// <returns>A value indicating if the score has made it onto the high score table.</returns>
        private bool AddScoreToTable<T>(DataServiceQuery<T> table, ScoreNamePair scoreNamePair, Type typeOfT)
        {
            // Add score if any lower than current score  
            DataServiceCollection<T> scores = new DataServiceCollection<T>(this.CreateQueryForAllElements<T>(table));

            // TODO: restructure data

            int comparison = 0;
            for (int count = 0; count < scores.Count; count++)
            {
                if (typeOfT == typeof(HighScores_000))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_000).Score, (scores[count] as HighScores_000).Name));                     
                }
                else if (typeOfT == typeof(HighScores_001))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_001).Score, (scores[count] as HighScores_001).Name));
                }
                else if (typeOfT == typeof(HighScores_002))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_002).Score, (scores[count] as HighScores_002).Name));
                }
                else if (typeOfT == typeof(HighScores_003))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_003).Score, (scores[count] as HighScores_003).Name));
                }
                else if (typeOfT == typeof(HighScores_004))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_004).Score, (scores[count] as HighScores_004).Name));
                }
                else if (typeOfT == typeof(HighScores_005))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_005).Score, (scores[count] as HighScores_005).Name));
                }
                else if (typeOfT == typeof(HighScores_006))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_006).Score, (scores[count] as HighScores_006).Name));
                }
                else if (typeOfT == typeof(HighScores_007))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_007).Score, (scores[count] as HighScores_007).Name));
                }
                else if (typeOfT == typeof(HighScores_008))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_008).Score, (scores[count] as HighScores_008).Name));
                }
                else if (typeOfT == typeof(HighScores_009))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_009).Score, (scores[count] as HighScores_009).Name));
                }
                else if (typeOfT == typeof(HighScores_010))
                {
                    comparison = scoreNamePair.CompareTo(new ScoreNamePair((scores[count] as HighScores_010).Score, (scores[count] as HighScores_010).Name));
                }
            }

            return false;
        }

        public bool AddScore(ScoreNamePair scoreNamePair, int levelNumber)
        {
            bool scoreAdded = false;
            try
            {
                switch (levelNumber)
                {
                    case 1:
                        scoreAdded= this.AddScoreToTable<HighScores_000>(context.HighScores_000, scoreNamePair, typeof(HighScores_000));
                        break;
                    //case 2:
                    //    scoreAdded = this.AddScoreToTable<HighScores_001>(context.HighScores_001, scoreNamePair);
                    //    break;
                    //case 3:
                    //    scoreAdded = this.AddScoreToTable<HighScores_002>(context.HighScores_002, scoreNamePair);
                    //    break;
                    //case 4:
                    //    scoreAdded = this.AddScoreToTable<HighScores_003>(context.HighScores_003, scoreNamePair);
                    //    break;
                    //case 5:
                    //    scoreAdded = this.AddScoreToTable<HighScores_004>(context.HighScores_004, scoreNamePair);
                    //    break;
                    //case 6:
                    //    scoreAdded = this.AddScoreToTable<HighScores_005>(context.HighScores_005, scoreNamePair);
                    //    break;
                    //case 7:
                    //    scoreAdded = this.AddScoreToTable<HighScores_006>(context.HighScores_006, scoreNamePair);
                    //    break;
                    //case 8:
                    //    scoreAdded = this.AddScoreToTable<HighScores_007>(context.HighScores_007, scoreNamePair);
                    //    break;
                    //case 9:
                    //    scoreAdded = this.AddScoreToTable<HighScores_008>(context.HighScores_008, scoreNamePair);
                    //    break;
                    //case 10:
                    //    scoreAdded = this.AddScoreToTable<HighScores_009>(context.HighScores_009, scoreNamePair);
                    //    break;
                    //case 11:
                    //    scoreAdded = this.AddScoreToTable<HighScores_010>(context.HighScores_010, scoreNamePair);
                    //    break;
                    default:
                        scoreAdded = false;
                        break;
                }

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
