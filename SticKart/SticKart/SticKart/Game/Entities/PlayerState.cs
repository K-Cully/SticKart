namespace SticKart.Game.Entities
{
    /// <summary>
    /// An enumeration of states which the stickman can be in.
    /// </summary>
    public enum PlayerState
    {
        /// <summary>
        /// The player is standing.
        /// </summary>
        standing,

        /// <summary>
        /// The player is crouching.
        /// </summary>
        crouching,

        /// <summary>
        /// The player is jumping.
        /// </summary>
        jumping,

        /// <summary>
        /// The player is running.
        /// </summary>
        running,

        /// <summary>
        /// The player is falling.
        /// </summary>
        falling,

        /// <summary>
        /// The player is dead.
        /// </summary>
        dead
    }
}
