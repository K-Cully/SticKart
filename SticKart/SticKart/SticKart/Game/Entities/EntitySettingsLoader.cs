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

        /// <summary>
        /// Gets the settings for a ruby bonus.
        /// </summary>
        public static ObstacleOrBonusSetting RubySettings { get; private set; }

        /// <summary>
        /// Gets the settings for a diamond bonus.
        /// </summary>
        public static ObstacleOrBonusSetting DiamondSettings { get; private set; }

        #endregion

        #region obstacle_descriptions

        /// <summary>
        /// Gets the settings for a rock obstacle.
        /// </summary>
        public static ObstacleOrBonusSetting RockSettings { get; private set; }

        /// <summary>
        /// Gets the settings for a spike obstacle.
        /// </summary>
        public static ObstacleOrBonusSetting SpikeSettings { get; private set; }

        /// <summary>
        /// Gets the settings for a fire obstacle.
        /// </summary>
        public static ObstacleOrBonusSetting FireSettings { get; private set; }

        #endregion

        #region powerup_descriptions

        /// <summary>
        /// Gets the settings for an invincible power up.
        /// </summary>
        public static PowerUpSetting InvincibleSettings { get; private set; }

        /// <summary>
        /// Gets the settings for a health power up.
        /// </summary>
        public static PowerUpSetting HealthSettings { get; private set; }

        /// <summary>
        /// Gets the settings for a speed power up.
        /// </summary>
        public static PowerUpSetting SpeedSettings { get; private set; }

        /// <summary>
        /// Gets the settings for a jump power up.
        /// </summary>
        public static PowerUpSetting JumpSettings { get; private set; }

        #endregion

        /// <summary>
        /// Loads all the entity settings used in level entities.
        /// </summary>
        /// <param name="content">The game's content manager.</param>
        public static void LoadEntitySettings(ContentManager content)
        {
            EntitySettingsLoader.CoinSettings = content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.CoinName);
            EntitySettingsLoader.RubySettings = content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.RubyName);
            EntitySettingsLoader.DiamondSettings = content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.DiamondName);
            EntitySettingsLoader.RockSettings = content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.RockName);
            EntitySettingsLoader.SpikeSettings = content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.SpikeName);
            EntitySettingsLoader.FireSettings = content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.FireName);
            EntitySettingsLoader.InvincibleSettings = content.Load<PowerUpSetting>(EntityConstants.SettingsFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.InvincibleName);
            EntitySettingsLoader.HealthSettings = content.Load<PowerUpSetting>(EntityConstants.SettingsFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.HealthName);
            EntitySettingsLoader.SpeedSettings = content.Load<PowerUpSetting>(EntityConstants.SettingsFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.SpeedName);
            EntitySettingsLoader.JumpSettings = content.Load<PowerUpSetting>(EntityConstants.SettingsFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.JumpName);
        }
    }
}
