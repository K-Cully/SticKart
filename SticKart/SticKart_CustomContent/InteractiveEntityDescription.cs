namespace SticKart
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Describes a the basic aspects of a game entity which interacts with the player.
    /// </summary>
    public class InteractiveEntityDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveEntityDescription"/> class.
        /// </summary>
        public InteractiveEntityDescription()
        {
            this.Name = string.Empty;
            this.Position = Vector2.Zero;
            this.Dimensions = Vector2.Zero;
        }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the position of the entity in display coordinates.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the size of the entity in display coordinates.
        /// </summary>
        public Vector2 Dimensions { get; set; }
    }
}
