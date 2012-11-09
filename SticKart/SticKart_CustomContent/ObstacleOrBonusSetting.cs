namespace SticKart
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The settings for a bonus or an obstacle.
    /// </summary>
    public class ObstacleOrBonusSetting
    {
        /// <summary>
        /// The name of the object.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Whether the object is a bonus or an obstacle.
        /// </summary>
        public bool IsBonus = false;

        /// <summary>
        /// The value to apply to the object.
        /// </summary>
        public int Value = 0;
    }
}
