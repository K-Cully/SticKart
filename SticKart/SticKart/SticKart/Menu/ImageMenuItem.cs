using System;
using Microsoft.Xna.Framework;
using SticKart.Display;

namespace SticKart.Menu
{
    /// <summary>
    /// A basic menu item which contains a sprite.
    /// </summary>
    class ImageMenuItem : MenuItem
    {
        /// <summary>
        /// The sprite object to draw to the screen.
        /// </summary>
        protected Sprite sprite;

        /// <summary>
        /// Initalizes a new instance of the <see cref="TextMenuItem"/> base class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="sprite">The sprite to associate with the menu item.</param>
        public ImageMenuItem(Vector2 relativePosition, Sprite sprite)
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
                return typeof(ImageMenuItem);
            }
        }
        
        /// <summary>
        /// Draws the image menu item.
        /// </summary>
        /// <param name="parentPosition">The position of the menu item's parent/owner.</param>
        public override void Draw(Vector2 parentPosition)
        {
            Sprite.Draw(this.sprite, parentPosition + base.relativePosition, 0.0f);
        }
    }
}
