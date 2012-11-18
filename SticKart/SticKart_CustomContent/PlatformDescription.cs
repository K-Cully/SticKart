namespace SticKart
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Describes a platform's placement and size.
    /// </summary>
    public class PlatformDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformDescription"/> class.
        /// </summary>
        public PlatformDescription()
        {
            this.Position = Vector2.Zero;
            this.Length = 0.0f;
        }

        /// <summary>
        /// Gets or sets the position of the platform, in display units.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the length of the platform, in display units.
        /// </summary>
        public float Length { get; set; }
    }
}
