// -----------------------------------------------------------------------
// <copyright file="AudioManager.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Audio
{
    using System;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Media;

    /// <summary>
    /// A wrapper for managing audio playback and settings.
    /// </summary>
    public static class AudioManager
    {
        /// <summary>
        /// A value indicating whether sound effects are enabled or not.
        /// </summary>
        private static bool sfxEnabled = true;

        /// <summary>
        /// The volume to play sound effects at.
        /// </summary>
        private static float sfxVolume = 1.0f;

        /// <summary>
        /// A value indicating whether music is enabled or not.
        /// </summary>
        private static bool musicEnabled = true;

        /// <summary>
        /// The volume to play music at.
        /// </summary>
        private static float musicVolume = 0.7f;

        /// <summary>
        /// A manager for music loading.
        /// </summary>
        private static MusicManager musicManager;

        /// <summary>
        /// Sets the sound effect volume.
        /// Values should be in the range 0 to 10, inclusive.
        /// </summary>
        public static uint SoundEffectsVolume
        {
            set
            {
                if (value > 0)
                {
                    AudioManager.sfxEnabled = true;
                }
                else
                {
                    AudioManager.sfxEnabled = false;
                }

                if (value > 10)
                {
                    value = 10;
                }
                
                AudioManager.sfxVolume = (float)value / 10.0f;
            }
        }

        /// <summary>
        /// Sets a value indicating whether sound effects should be enabled or not.
        /// </summary>
        public static bool SoundEffectsEnabled
        {
            set
            {
                AudioManager.sfxEnabled = value;
            }
        }

        /// <summary>
        /// Sets the sound music.
        /// Values should be in the range 0 to 10, inclusive.
        /// </summary>
        public static uint MusicVolume
        {
            set
            {
                if (value > 0)
                {
                    AudioManager.musicEnabled = true;
                }
                else
                {
                    AudioManager.musicEnabled = false;
                }

                if (value > 10)
                {
                    value = 10;
                }

                AudioManager.musicVolume = (float)value / 10.0f;
                if (MediaPlayer.GameHasControl)
                {
                    MediaPlayer.Volume = AudioManager.musicVolume;
                }
            }
        }

        /// <summary>
        /// Sets a value indicating whether music should be enabled or not.
        /// </summary>
        public static bool MusicEnabled
        {
            set
            {
                AudioManager.musicEnabled = value;
            }
        }

        /// <summary>
        /// Initializes the audio manager.
        /// </summary>
        /// <param name="contentManager">The game's content manager.</param>
        public static void InitializeAndLoad(ContentManager contentManager)
        {
            AudioManager.musicManager = new MusicManager(3, 2);
            AudioManager.musicManager.InitializeAndLoad(contentManager);
        }

        /// <summary>
        /// Plays a random background track.
        /// </summary>
        /// <param name="inGame">A value indicating if the player is currently in the game or not (in the menu).</param>
        public static void PlayBackgroundMusic(bool inGame)
        {
            if (AudioManager.musicEnabled)
            {
                try
                {
                    MediaPlayer.Volume = AudioManager.musicVolume;
                    if (MediaPlayer.State == MediaState.Paused && AudioManager.musicManager.GetLast(inGame) != null)
                    {
                        MediaPlayer.Play(AudioManager.musicManager.GetLast(inGame));
                    }
                    else
                    {
                        MediaPlayer.Play(AudioManager.musicManager.GetNext(inGame));
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Pauses the current background track.
        /// </summary>
        public static void PauseBackgroundMusic()
        {
            if (AudioManager.musicEnabled)
            {
                try
                {
                    MediaPlayer.Pause();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Stops the current background track.
        /// </summary>
        public static void StopBackgroundMusic()
        {
            if (AudioManager.musicEnabled)
            {
                try
                {
                    MediaPlayer.Stop();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Plays a sound effect at the set volume.
        /// </summary>
        /// <param name="effect">The sound effect to play.</param>
        public static void PlayEffect(SoundEffect effect)
        {
            if (AudioManager.sfxEnabled)
            {
                effect.Play(AudioManager.sfxVolume, 0.0f, 0.0f);
            }
        }

        /// <summary>
        /// Plays a sound effect instance at the set volume.
        /// </summary>
        /// <param name="effectInstance">The effect instance to play.</param>
        /// <param name="interuptSelf">A value indicating whether the effect should interrupt itself or not.</param>
        public static void PlayEffectIncstance(SoundEffectInstance effectInstance, bool interuptSelf)
        {
            if (AudioManager.sfxEnabled)
            {
                effectInstance.Volume = AudioManager.sfxVolume;
                if (interuptSelf || effectInstance.State != SoundState.Playing)
                {
                    effectInstance.Play();
                }
            }
        }
    }
}
