// -----------------------------------------------------------------------
// <copyright file="MenuSelectableItem.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Menu
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The base class from which all selectable menu items derive.
    /// </summary>
    public abstract class MenuSelectableItem : MenuItem
    {
        /// <summary>
        /// The selectable item's bounding box.
        /// </summary>
        protected Rectangle boundingBox;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuSelectableItem"/> class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="name">The name of the selectable item.</param>
        /// <param name="dimensions">The bounding box of the selectable item.</param>
        public MenuSelectableItem(Vector2 relativePosition, string name, Vector2 dimensions)
            : base(relativePosition)
        {
            this.Name = name;
            this.boundingBox = new Rectangle((int)(relativePosition.X - (dimensions.X * 0.5f)), (int)(relativePosition.Y - (dimensions.Y * 0.5f)), (int)dimensions.X, (int)dimensions.Y);
        }

        /// <summary>
        /// Gets the Type of the menu item.
        /// </summary>
        public override Type Type
        {
            get
            {
                return typeof(MenuSelectableItem);
            }
        }

        /// <summary>
        /// Gets the name of the selectable menu item.
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// Checks if the click position passed in intersects the selectable menu item.
        /// </summary>
        /// <param name="clickPosition">The position of the click.</param>
        /// <param name="parentPosition">The position of the menu item's parent/owner.</param>
        /// <returns>Whether a click occurred or not.</returns>
        public virtual bool CheckForClick(Vector2 clickPosition, Vector2 parentPosition)
        {
            return this.boundingBox.Contains((int)(clickPosition.X - parentPosition.X), (int)(clickPosition.Y - parentPosition.Y));
        }
    }
}
