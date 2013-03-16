// -----------------------------------------------------------------------
// <copyright file="ScoreServiceConstants.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.AzureServices
{
    /// <summary>
    /// Defines all constants used by the score service manager.
    /// </summary>
    public class ScoreServiceConstants
    {
        /// <summary>
        /// The uri string of the hosted data service.
        /// </summary>
        internal const string ServiceUriString = "https://stickarthighscores.cloudapp.net/ScoresWcfDataService.svc";

        /// <summary>
        /// The name and issuer to expect from certificate of data service.
        /// </summary>
        internal const string CertName = "CN=stickarthighscores";

        /// <summary>
        /// The app login credentials.
        /// </summary>
        internal const string AppLoginName = "SticKartApp";

        /// <summary>
        /// The app login password.
        /// </summary>
        internal const string AppLoginPassword = "1_7_13_47AeioufqR";
    }
}
