// -----------------------------------------------------------------------
// <copyright file="MenuImage.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Menu
{
    using System;
    using Display;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A basic menu item which contains a sprite.
    /// </summary>
    public class MenuImage : MenuItem
    {
        /// <summary>
        /// The sprite object to draw to the screen.
        /// </summary>
        private Sprite sprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuImage"/> class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="sprite">The sprite to associate with the menu item.</param>
        public MenuImage(Vector2 relativePosition, Sprite sprite)
            : base(relativePosition)
        {
            this.sprite = sprite;
        }

        /// <summary>
        /// Gets the Type of the menu item.
        /// </summary>
        public override Type Type
        {
            get
            {
                return typeof(MenuImage);
            }
        }
        
        /// <summary>
        /// Draws the image menu item.
        /// </summary>
        /// <param name="parentPosition">The position of the menu item's parent/owner.</param>
        /// <param name="grayedOut">Whether the item should be grayed out or not.</param>
        public override void Draw(Vector2 parentPosition, bool grayedOut = false)
        {
            if (grayedOut)
            {
                Sprite.Draw(this.sprite, parentPosition + this.RelativePosition, 0.0f, new Color(30, 30, 30));
            }
            else
            {
                Sprite.Draw(this.sprite, parentPosition + this.RelativePosition, 0.0f);
            }
        }
    }
}
