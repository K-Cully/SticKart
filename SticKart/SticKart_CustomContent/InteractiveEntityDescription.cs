namespace SticKart
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Describes a the basic aspects of a game entity which interacts with the player.
    /// </summary>
    public class InteractiveEntityDescription
    {
        /// <summary>
        /// The name of the entity.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// The position of the entity in display coordinates.
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// The size of the entity in display coordinates.
        /// </summary>
        public Vector2 Dimensions = Vector2.Zero;
    }
}
