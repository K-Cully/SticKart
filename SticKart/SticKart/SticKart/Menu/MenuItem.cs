﻿namespace SticKart.Menu
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The base class from which all menu items derive.
    /// </summary>
    public abstract class MenuItem
    {
        /// <summary>
        /// The centre position of the menu item, relative to it's owner/parent.
        /// </summary>
        protected Vector2 relativePosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        public MenuItem(Vector2 relativePosition)
        {
            this.relativePosition = relativePosition;
        }

        /// <summary>
        /// Gets the Type of the menu item.
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// Draws the menu item.
        /// </summary>
        /// <param name="parentPosition">The position of the menu item's parent/owner.</param>
        public abstract void Draw(Vector2 parentPosition);
    }
}
