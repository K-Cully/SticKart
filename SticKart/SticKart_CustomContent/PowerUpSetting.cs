namespace SticKart
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The settings for a power up entity.
    /// </summary>
    public class PowerUpSetting
    {
        /// <summary>
        /// The name of the power up.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// The time, in seconds, to apply the effect for.
        /// </summary>
        public float TimeOfEffect = 0.0f;
    }
}
