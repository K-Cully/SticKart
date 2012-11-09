namespace SticKart.Game.Entities
{
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// Loads all the game's entity settings.
    /// </summary>
    public static class EntitySettingsLoader
    {
        #region bonus_descriptions

        /// <summary>
        /// Gets the settings for a coin bonus.
        /// </summary>
        public static ObstacleOrBonusSetting CoinSettings { get; private set; }

        #endregion

        #region obstacle_descriptions

        /// <summary>
        /// Gets the settings for a rock obstacle.
        /// </summary>
        public static ObstacleOrBonusSetting RockSettings { get; private set; }

        #endregion

        #region powerup_descriptions

        /// <summary>
        /// Gets the settings for an invincible power up.
        /// </summary>
        public static PowerUpSetting InvincibleSettings { get; private set; }

        #endregion

        /// <summary>
        /// Loads all the entity settings used in level entities.
        /// </summary>
        /// <param name="content">The game's content manager.</param>
        public static void LoadEntitySettings(ContentManager content)
        {
            EntitySettingsLoader.CoinSettings = content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.CoinName);
            EntitySettingsLoader.RockSettings = content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.RockName);
            EntitySettingsLoader.InvincibleSettings = content.Load<PowerUpSetting>(EntityConstants.SettingsFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.InvincibleName);
        }
    }
}
