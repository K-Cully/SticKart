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
        public static ObstacleOrBonusDescription CoinSettings { get; private set; }

        #endregion

        #region obstacle_descriptions

        /// <summary>
        /// Gets the settings for a rock obstacle.
        /// </summary>
        public static ObstacleOrBonusDescription RockSettings { get; private set; }

        #endregion

        #region powerup_descriptions

        /// <summary>
        /// Gets the settings for an invincible power up.
        /// </summary>
        public static PowerUpDescription InvincibleSettings { get; private set; }

        #endregion

        /// <summary>
        /// Loads all the entity settings used in level entities.
        /// </summary>
        /// <param name="content">The game's content manager.</param>
        public static void LoadEntitySettings(ContentManager content)
        {
            EntitySettingsLoader.CoinSettings = content.Load<ObstacleOrBonusDescription>(EntityConstants.DescriptionFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.CoinName);
            EntitySettingsLoader.RockSettings = content.Load<ObstacleOrBonusDescription>(EntityConstants.DescriptionFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.RockName);
            EntitySettingsLoader.InvincibleSettings = content.Load<PowerUpDescription>(EntityConstants.DescriptionFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.InvincibleName);
        }
    }
}
