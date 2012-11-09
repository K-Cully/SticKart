namespace SticKart.Game.Entities
{
    /// <summary>
    /// Contains level entity names.
    /// </summary>
    public class EntityConstants
    {
        #region paths

        /// <summary>
        /// The path to the entity settings folder.
        /// </summary>
        public const string SettingsFolderPath = "Entity_settings/";

        /// <summary>
        /// The sub-folder path to the bonus settings folder.
        /// </summary>
        public const string BonusFolderSubPath = "Bonuses/";

        /// <summary>
        /// The sub-folder path to the obstacle settings folder.
        /// </summary>
        public const string ObstacleFolderSubPath = "Obstacles/";

        /// <summary>
        /// The sub-folder path to the power up settings folder.
        /// </summary>
        public const string PowerUpFolderSubPath = "Power_ups/";

        #endregion

        #region bonus_names

        /// <summary>
        /// The name of a coin bonus entity.
        /// </summary>
        public const string CoinName = "coin";

        #endregion

        #region obstacle_names

        /// <summary>
        /// The name of a rock obstacle entity.
        /// </summary>
        public const string RockName = "rock";

        #endregion

        #region power_up_names

        /// <summary>
        /// The name of an invincible power up entity.
        /// </summary>
        public const string InvincibleName = "invincible";

        #endregion
    }
}
