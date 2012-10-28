using System;
using Microsoft.Xna.Framework;
using SticKart.Display;

namespace SticKart.Menu
{
    /// <summary>
    /// A basic menu item which contains text.
    /// </summary>
    class TextMenuItem : MenuItem
    {
        /// <summary>
        /// The renderable text object to draw to the screen.
        /// </summary>
        protected RenderableText text;

        /// <summary>
        /// Initalizes a new instance of the <see cref="TextMenuItem"/> base class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="text">The renderable text object to associate with the menu item.</param>
        public TextMenuItem(Vector2 relativePosition, RenderableText text)
            : base(relativePosition)
        {
            this.text = text;
        }

        /// <summary>
        /// Gets the Type of the menu item.
        /// </summary>
        public override Type Type
        {
            get
            {
                return typeof(TextMenuItem);
            }
        }

        /// <summary>
        /// Sets the text stored in the menu item.
        /// </summary>
        /// <param name="newText">The new text value.</param>
        public virtual void SetText(string newText)
        {
            this.text.SetText(newText);
        }

        /// <summary>
        /// Draws the text menu item.
        /// </summary>
        /// <param name="parentPosition">The position of the menu item's parent/owner.</param>
        public override void Draw(Vector2 parentPosition)
        {
            RenderableText.Draw(this.text, parentPosition + base.relativePosition, 0.0f);
        }
    }
}
