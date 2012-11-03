namespace SticKart.Gestures
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A wrapper for a three dimensional gesture point at a specific time.
    /// </summary>
    public class GestureEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GestureEntry"/> class.
        /// </summary>
        /// <param name="position">The three dimensional position of the gesture entity.</param>
        /// <param name="time">The time at which the gesture entity was recorded.</param>
        public GestureEntry(Vector3 position, DateTime time)
        {
            this.Position = position;
            this.Time = time;
        }

        /// <summary>
        /// Gets the time at which the gesture entity was recorded.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Gets the position of the gesture entity.
        /// </summary>
        public Vector3 Position { get; private set; }
    }
}
