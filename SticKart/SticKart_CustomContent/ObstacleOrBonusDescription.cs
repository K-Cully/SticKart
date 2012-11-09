namespace SticKart
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Describes the characteristics of a bonus or an obstacle.
    /// </summary>
    public class ObstacleOrBonusDescription
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
