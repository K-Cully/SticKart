// -----------------------------------------------------------------------
// <copyright file="MusicManager.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Audio
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Media;

    /// <summary>
    /// A wrapper for loading and selecting music.
    /// <remarks>All music assets should be named using numbers from 1 up.</remarks>
    /// </summary>
    public class MusicManager
    {
        /// <summary>
        /// The location of game-play music assets.
        /// </summary>
        private const string GameMusicLocation = "Music/Game/";
        
        /// <summary>
        /// The location of menu music assets.
        /// </summary>
        private const string MenuMusicLocation = "Music/Menu/";

        /// <summary>
        /// A random number generator, used to select songs.
        /// </summary>
        private Random randomGenerator;

        /// <summary>
        /// The index of the last song played.
        /// </summary>
        private int lastSongNumber;

        /// <summary>
        /// A value indicating whether the last song was a game-play song or not.
        /// </summary>
        private bool lastSongInGame;

        /// <summary>
        /// The number of game-play songs.
        /// </summary>
        private int numberOfGameSongs;

        /// <summary>
        /// The number of menu songs.
        /// </summary>
        private int numberOfMenuSongs;

        /// <summary>
        /// A list of game-play songs.
        /// </summary>
        private List<Song> gameSongs;

        /// <summary>
        /// A list of menu songs.
        /// </summary>
        private List<Song> menuSongs;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicManager"/> class.
        /// </summary>
        /// <param name="numberOfGameSongs">The total number of game-play songs.</param>
        /// <param name="numberOfMenuSongs">The total number of menu songs.</param>
        public MusicManager(uint numberOfGameSongs, uint numberOfMenuSongs)
        {
            this.lastSongInGame = false;
            this.lastSongNumber = 0;
            this.randomGenerator = new Random();
            this.numberOfGameSongs = (int)numberOfGameSongs;
            this.numberOfMenuSongs = (int)numberOfMenuSongs;
            this.gameSongs = new List<Song>(this.numberOfGameSongs);
            this.menuSongs = new List<Song>(this.numberOfMenuSongs);
        }

        /// <summary>
        /// Initializes and loads all songs in the music manager.
        /// </summary>
        /// <param name="contentManager">The game's content manager.</param>
        public void InitializeAndLoad(ContentManager contentManager)
        {
            int length = this.numberOfMenuSongs > this.numberOfGameSongs ? this.numberOfMenuSongs : this.numberOfGameSongs;
            for (int count = 1; count <= length; count++)
            {
                if (count <= this.numberOfMenuSongs)
                {
                    this.menuSongs.Add(contentManager.Load<Song>(MusicManager.MenuMusicLocation + count.ToString()));
                }

                if (count <= this.numberOfGameSongs)
                {
                    this.gameSongs.Add(contentManager.Load<Song>(MusicManager.GameMusicLocation + count.ToString()));
                }
            }
        }

        /// <summary>
        /// Retrieves a random song from the list of loaded songs.
        /// </summary>
        /// <param name="inGame">A value indicating if the player is in the game or not (in menu).</param>
        /// <returns>A randomly selected song.</returns>
        public Song GetNext(bool inGame)
        {
            this.lastSongInGame = inGame;
            if (inGame)
            {
                this.lastSongNumber = this.randomGenerator.Next(this.numberOfGameSongs);
                return this.gameSongs[this.lastSongNumber];
            }
            else
            {
                this.lastSongNumber = this.randomGenerator.Next(this.numberOfMenuSongs);
                return this.menuSongs[this.lastSongNumber];
            }
        }

        /// <summary>
        /// Retrieves the last song played.
        /// </summary>
        /// <param name="inGame">A value indicating whether to retrieve game-play music or not (menu music).</param>
        /// <returns>The last song played if the game-play state has not changed.</returns>
        public Song GetLast(bool inGame)
        {
            if (inGame && this.lastSongInGame)
            {
                return this.gameSongs[this.lastSongNumber];
            }
            else if (!this.lastSongInGame)
            {
                return this.menuSongs[this.lastSongNumber];
            }
            else
            {
                return null;
            }
        }
    }
}
