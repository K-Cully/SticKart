// -----------------------------------------------------------------------
// <copyright file="ScoresWcfDataService.svc.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKartScoresAzureWebRole
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services;
    using System.Data.Services.Common;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;
    using System.Web;

    /// <summary>
    /// Defines the high score and matchmaking data service.
    /// </summary>
    public class ScoresWcfDataService : DataService<SticKartScores_0Entities>
    {
        /// <summary>
        /// Initializes the service and sets access rules.
        /// </summary>
        /// <param name="config">The data service configuration.</param>
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("HighScores", EntitySetRights.All);
            config.SetEntitySetAccessRule("ActivePlayers", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Statistics", EntitySetRights.AllRead);
            config.UseVerboseErrors = false;
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
            config.SetServiceOperationAccessRule("RegisterPlayer", ServiceOperationRights.All);
            config.SetServiceOperationAccessRule("AddActivePlayer", ServiceOperationRights.All);
            config.SetServiceOperationAccessRule("SetPlayerInGame", ServiceOperationRights.All);
            config.SetServiceOperationAccessRule("AreOpponentsAvailable", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetNextOpponentAddress", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("RemoveFromActivePlayers", ServiceOperationRights.All);            
        }

        /// <summary>
        /// Registers a player with the matchmaking service.
        /// </summary>
        /// <param name="name">The player's name.</param>
        /// <param name="password">The player's password.</param>
        /// <returns>A value indicating whether the player was added or not.</returns>
        [WebGet]
        public bool RegisterPlayer(string name, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || name.Length > 32 || password.Length > 4)
            {
                return false;
            }

            SticKartScores_0Entities context = this.CurrentDataSource;
            try
            {
                var playersRegistered = (from player in context.Statistics where player.Name == name && player.Password == password select player).Take(1);
                if (playersRegistered.Count() == 0)
                {
                    Statistic playerData = new Statistic();
                    playerData.Name = name;
                    playerData.Password = password;
                    context.Statistics.AddObject(playerData);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", exception.Message));
            }
        }

        /// <summary>
        /// Adds a player to the matchmaking table if they are registered with the service.
        /// </summary>
        /// <param name="gamePort">The port the player is waiting to play on.</param>
        /// <param name="name">The player's name.</param>
        /// <param name="password">The player's password.</param>
        /// <param name="isHost">A value indicating whether the player should be registered as a host or client.</param>
        /// <returns>A value indicating whether the player was added to the matchmaking table or not.</returns>
        [WebGet]
        public bool AddActivePlayer(string gamePort, string name, string password, bool isHost)
        {
            if (string.IsNullOrEmpty(gamePort) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || gamePort.Length > 4)
            {
                return false;
            }

            SticKartScores_0Entities context = this.CurrentDataSource;
            try
            {
                var playersRegistered = from player in context.Statistics where player.Name == name && player.Password == password select player;
                if (playersRegistered.Count() == 0)
                {
                    return false;
                }
                else
                {
                    MessageProperties messageProperties = OperationContext.Current.IncomingMessageProperties;
                    RemoteEndpointMessageProperty endpoint = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                    ActivePlayer matchmakingEntry = new ActivePlayer();
                    matchmakingEntry.Ip = endpoint.Address;
                    matchmakingEntry.Port = gamePort;
                    matchmakingEntry.Player = playersRegistered.First().Id;
                    matchmakingEntry.State = isHost ? "H" : "C";
                    context.ActivePlayers.AddObject(matchmakingEntry);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", exception.Message));
            }
        }

        /// <summary>
        /// Sets the player's state to in game in the matchmaking lobby.
        /// </summary>
        /// <param name="gamePort">The port which the player is playing on.</param>
        /// <returns>A value indicating whether the update was successful or not.</returns>
        [WebGet]
        public bool SetPlayerInGame(string gamePort)
        {
            if (string.IsNullOrEmpty(gamePort) || gamePort.Length > 4)
            {
                return false;
            }

            MessageProperties messageProperties = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            SticKartScores_0Entities context = this.CurrentDataSource;
            try
            {
                var playersRegistered = (from player in context.ActivePlayers where player.Ip == endpoint.Address && player.Port == gamePort select player).Take(1);
                if (playersRegistered.Count() == 0)
                {
                    return false;
                }
                else
                {
                    playersRegistered.First().State = "G";
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", exception.Message));
            }
        }
        
        /// <summary>
        /// Checks whether there are any opponents available or not.
        /// </summary>
        /// <param name="lookingForHosts">A value indicating whether to look for hosts or clients.</param>
        /// <returns>A value indicating whether any opponents are available or not.</returns>
        [WebGet]
        public bool AreOpponentsAvailable(bool lookingForHosts)
        {
            SticKartScores_0Entities context = this.CurrentDataSource;
            try
            {
                string opponentState = lookingForHosts ? "H" : "C";
                var players = (from player in context.ActivePlayers where player.State == opponentState select player).Take(1);
                return players.Count() > 0;
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", exception.Message));
            }
        }

        /// <summary>
        /// Retrieves the next available opponent's address and port to play on.
        /// </summary>
        /// <param name="lookingForHosts">A value indicating whether to search for hosts or clients.</param>
        /// <returns>The next available opponent's address and game port.</returns>
        [WebGet]
        public string GetNextOpponentAddress(bool lookingForHosts)
        {
            SticKartScores_0Entities context = this.CurrentDataSource;
            try
            {
                string opponentState = lookingForHosts ? "H" : "C";
                var players = (from player in context.ActivePlayers where player.State == opponentState select player).Take(1);
                if (players.Count() > 0)
                {
                    return players.Single().Ip + ":" + players.Single().Port;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", exception.Message));
            }
        }

        /// <summary>
        /// Removes a player from the matchmaking table and updates their statistics.
        /// </summary>
        /// <param name="gamePort">The port the player was playing on.</param>
        /// <param name="gameComplete">A value indicating whether the game finished correctly or not.</param>
        /// <param name="gameWon">A value indicating whether the player won or not.</param>
        /// <returns>A value indicating whether the player was removed successfully or not.</returns>
        [WebGet]
        public bool RemoveFromActivePlayers(string gamePort, bool gameComplete, bool gameWon)
        {
            if (string.IsNullOrEmpty(gamePort))
            {
                return false;
            }

            MessageProperties messageProperties = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            SticKartScores_0Entities databaseContext = this.CurrentDataSource;
            try
            {
                var players = (from player in databaseContext.ActivePlayers where player.Ip == endpoint.Address && player.Port == gamePort select player).Take(1);
                if (players.Count() > 0)
                {
                    players.First().StatisticReference.Value.GamesPlayed = players.First().StatisticReference.Value.GamesPlayed + 1;
                    if (gameComplete)
                    {
                        if (gameWon)
                        {
                            players.First().StatisticReference.Value.GamesWon = players.First().StatisticReference.Value.GamesWon + 1;
                        }
                        else
                        {
                            players.First().StatisticReference.Value.GamesLost = players.First().StatisticReference.Value.GamesLost + 1;
                        }
                    }

                    databaseContext.ActivePlayers.DeleteObject(players.First());
                    databaseContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", exception.Message));
            }
        }
    }
}
