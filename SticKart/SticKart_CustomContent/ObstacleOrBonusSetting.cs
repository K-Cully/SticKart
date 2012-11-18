namespace SticKart
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The settings for a bonus or an obstacle.
    /// </summary>
    public class ObstacleOrBonusSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObstacleOrBonusSetting"/> class.
        /// </summary>
        public ObstacleOrBonusSetting()
        {
            this.Name = string.Empty;
            this.IsBonus = false;
            this.Value = 0;
        }

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the object is a bonus or an obstacle.
        /// </summary>
        public bool IsBonus { get; set; }

        /// <summary>
        /// Gets or sets the value to apply to the object.
        /// </summary>
        public int Value { get; set; }
    }
}
