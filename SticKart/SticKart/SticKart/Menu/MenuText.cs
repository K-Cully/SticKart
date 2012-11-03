namespace SticKart.Menu
{
    using System;
    using Display;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A basic menu item which contains text.
    /// </summary>
    public class MenuText : MenuItem
    {
        /// <summary>
        /// The text object to draw to the screen.
        /// </summary>
        private RenderableText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuText"/> class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="text">The text object to associate with the menu item.</param>
        public MenuText(Vector2 relativePosition, RenderableText text)
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
                return typeof(MenuText);
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
            RenderableText.Draw(this.text, parentPosition + this.relativePosition, 0.0f);
        }
    }
}
