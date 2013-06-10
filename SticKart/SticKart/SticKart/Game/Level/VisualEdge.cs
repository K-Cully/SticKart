// -----------------------------------------------------------------------
// <copyright file="VisualEdge.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Level
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an edge to display on screen.
    /// </summary>
    public class VisualEdge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualEdge"/> class.
        /// </summary>
        /// <param name="startPoint">The start point of the edge.</param>
        /// <param name="endPoint">The end point of the edge.</param>
        public VisualEdge(Vector2 startPoint, Vector2 endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Position = (startPoint + endPoint) / 2.0f;
            Vector2 direction = endPoint - startPoint;
            direction.Normalize();
            this.Angle = (float)Math.Acos(direction.X);
            if (direction.Y < 0.0f)
            {
                this.Angle *= -1.0f;
            }
        }

        /// <summary>
        /// Gets the position of the edge.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Gets the angle of the edge.
        /// </summary>
        public float Angle { get; private set; }

        /// <summary>
        /// Gets the start position of the edge.
        /// </summary>
        public Vector2 StartPoint { get; private set; }

        /// <summary>
        /// Gets the end position of the edge.
        /// </summary>
        public Vector2 EndPoint { get; private set; }
    }
}
