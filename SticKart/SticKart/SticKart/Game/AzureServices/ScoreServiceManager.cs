// -----------------------------------------------------------------------
// <copyright file="ScoreServiceManager.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.AzureServices
{
    using System;
    using System.Data.Services.Client;
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
                // Instantiate the DataServiceContext.
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
        public ScoreServiceManager Instance
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
        public ScoreServiceManager Initialize()
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
    }
}
