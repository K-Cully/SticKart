namespace SticKart
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The settings for a power up entity.
    /// </summary>
    public class PowerUpSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PowerUpSetting"/> class.
        /// </summary>
        public PowerUpSetting()
        {
            this.Name = string.Empty;
            this.TimeOfEffect = 0.0f;
        }

        /// <summary>
        /// Gets or sets the name of the power up.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the time, in seconds, to apply the effect for.
        /// </summary>
        public float TimeOfEffect { get; set; }
    }
}
