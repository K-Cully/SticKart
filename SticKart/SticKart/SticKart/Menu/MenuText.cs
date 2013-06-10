// -----------------------------------------------------------------------
// <copyright file="MenuText.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

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
            this.IsChangeable = false;
            this.text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuText"/> class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="text">The text object to associate with the menu item.</param>
        /// <param name="canChange">A value indicating whether the text is allowed to be changed or not.</param>
        public MenuText(Vector2 relativePosition, RenderableText text, bool canChange)
            : base(relativePosition)
        {
            this.IsChangeable = canChange;
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
        /// Gets a value indicating whether the text is allowed to change.
        /// </summary>
        public bool IsChangeable { get; private set; }

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
        /// <param name="grayedOut">Whether the item should be grayed out or not.</param>
        public override void Draw(Vector2 parentPosition, bool grayedOut = false)
        {
            if (grayedOut)
            {
                RenderableText.Draw(this.text, parentPosition + this.RelativePosition, 0.0f, new Color(30, 30, 30));
            }
            else
            {
                RenderableText.Draw(this.text, parentPosition + this.RelativePosition, 0.0f);
            }
        }
    }
}
