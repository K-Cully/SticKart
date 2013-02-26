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

        public bool AddScore(ScoreNamePair scoreNamePair, int levelNumber)
        {
            // Add score if any lower than current score            
            try
            {
                // Define a LINQ query that returns the high score entry for scores over zero
                var query = from scores in context.HighScores_000 where scores.Score > scoreNamePair.Score select scores;

                // Create an DataServiceCollection<T> based on execution of the LINQ query for scores.
                DataServiceCollection<HighScores_000> customerOrders = new DataServiceCollection<HighScores_000>(query);

                if (customerOrders.Count > 0)
                {
                }
            }
            catch (Exception ex)
            {
                this.context = null;
            }
            
            // Else return false


            return false;
        }
    }
}
