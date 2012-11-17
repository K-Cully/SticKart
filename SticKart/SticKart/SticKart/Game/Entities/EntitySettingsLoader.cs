namespace SticKart.Game.Entities
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// Loads all the game's entity settings.
    /// </summary>
    public static class EntitySettingsLoader
    {
        /// <summary>
        /// The list of bonus and obstacle settings.
        /// </summary>
        private static List<ObstacleOrBonusSetting> obstacleAndBonusSettings = new List<ObstacleOrBonusSetting>();
        
        /// <summary>
        /// The list of power up settings.
        /// </summary>
        private static List<PowerUpSetting> powerUpSettings = new List<PowerUpSetting>();

        /// <summary>
        /// Loads all the entity settings used in level entities.
        /// </summary>
        /// <param name="content">The game's content manager.</param>
        public static void LoadEntitySettings(ContentManager content)
        {
            EntitySettingsLoader.obstacleAndBonusSettings.Add(content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.CoinName));
            EntitySettingsLoader.obstacleAndBonusSettings.Add(content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.RubyName));
            EntitySettingsLoader.obstacleAndBonusSettings.Add(content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.DiamondName));
            EntitySettingsLoader.obstacleAndBonusSettings.Add(content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.RockName));
            EntitySettingsLoader.obstacleAndBonusSettings.Add(content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.SpikeName));
            EntitySettingsLoader.obstacleAndBonusSettings.Add(content.Load<ObstacleOrBonusSetting>(EntityConstants.SettingsFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.FireName));
            EntitySettingsLoader.powerUpSettings.Add(content.Load<PowerUpSetting>(EntityConstants.SettingsFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.InvincibleName));
            EntitySettingsLoader.powerUpSettings.Add(content.Load<PowerUpSetting>(EntityConstants.SettingsFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.HealthName));
            EntitySettingsLoader.powerUpSettings.Add(content.Load<PowerUpSetting>(EntityConstants.SettingsFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.SpeedName));
            EntitySettingsLoader.powerUpSettings.Add(content.Load<PowerUpSetting>(EntityConstants.SettingsFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.JumpName));
        }

        /// <summary>
        /// Gets the obstacle or bonus settings with the name passed in.
        /// </summary>
        /// <param name="name">The name of the settings to look for.</param>
        /// <returns>The settings for the entity or null if not found.</returns>
        public static ObstacleOrBonusSetting GetObstacleOrBonusSetting(string name)
        {
            foreach (ObstacleOrBonusSetting setting in EntitySettingsLoader.obstacleAndBonusSettings)
            {
                if (setting.Name == name)
                {
                    return setting;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the power up settings with the name passed in.
        /// </summary>
        /// <param name="name">The name of the settings to look for.</param>
        /// <returns>The settings for the power up or null if not found.</returns>
        public static PowerUpSetting GetPowerUpSettings(string name)
        {
            foreach (PowerUpSetting setting in EntitySettingsLoader.powerUpSettings)
            {
                if (setting.Name == name)
                {
                    return setting;
                }
            }

            return null;
        }
    }
}
