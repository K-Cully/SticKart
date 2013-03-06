// -----------------------------------------------------------------------
// <copyright file="MenuButton.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Menu
{
    using System;
    using Display;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A button menu item.
    /// </summary>
    public class MenuButton : MenuSelectableItem
    {
        /// <summary>
        /// The button's background.
        /// </summary>
        private MenuImage tile;

        /// <summary>
        /// The icon to display on the button.
        /// </summary>
        private MenuImage icon;

        /// <summary>
        /// The text to display on the button.
        /// </summary>
        private MenuText text;

        /// <summary>
        /// The text to use as an icon, in place of an image.
        /// </summary>
        private MenuText textIcon;
                
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="tile">The button's background sprite.</param>
        /// <param name="textIcon">The text to display at the centre of the button.</param>
        /// <param name="text">The text to display at the bottom of the button.</param>
        /// <param name="name">The name of the button.</param>
        public MenuButton(Vector2 relativePosition, Sprite tile, RenderableText textIcon, RenderableText text, string name)
            : base(relativePosition, name, new Vector2(tile.Width, tile.Height))
        {
            this.textIcon = new MenuText(Vector2.Zero, textIcon);
            this.tile = new MenuImage(Vector2.Zero, tile);
            this.icon = null;
            Vector2 relativeTextPosition = new Vector2(0.0f, tile.Height * 0.36f);
            this.text = new MenuText(relativeTextPosition, text);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="tile">The button's background sprite.</param>
        /// <param name="icon">The icon to display on the button.</param>
        /// <param name="text">The text to display on the button.</param>
        /// <param name="name">The name of the button.</param>
        public MenuButton(Vector2 relativePosition, Sprite tile, Sprite icon, RenderableText text, string name)
            : base(relativePosition, name, new Vector2(tile.Width, tile.Height))
        {
            this.textIcon = null;
            this.tile = new MenuImage(Vector2.Zero, tile);
            this.icon = new MenuImage(Vector2.Zero, icon);
            Vector2 relativeTextPosition = new Vector2(0.0f, tile.Height * 0.36f);
            this.text = new MenuText(relativeTextPosition, text);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="tile">The button's background sprite.</param>
        /// <param name="icon">The icon to display on the button.</param>
        /// <param name="textIcon">The text to display at the centre of the button.</param>
        /// <param name="text">The text to display on the button.</param>
        /// <param name="name">The name of the button.</param>
        public MenuButton(Vector2 relativePosition, Sprite tile, Sprite icon, RenderableText textIcon, RenderableText text, string name)
            : base(relativePosition, name, new Vector2(tile.Width, tile.Height))
        {
            this.textIcon = new MenuText(Vector2.Zero, textIcon);
            this.tile = new MenuImage(Vector2.Zero, tile);
            this.icon = new MenuImage(Vector2.Zero, icon);
            Vector2 relativeTextPosition = new Vector2(0.0f, tile.Height * 0.36f);
            this.text = new MenuText(relativeTextPosition, text);
        }
        
        /// <summary>
        /// Gets the Type of the menu item.
        /// </summary>
        public override Type Type
        {
            get
            {
                return typeof(MenuButton);
            }
        }
        
        /// <summary>
        /// Draws the button to the screen.
        /// </summary>
        /// <param name="parentPosition">The position of the button's parent/owner.</param>
        /// <param name="grayedOut">Whether the item should be grayed out or not.</param>
        public override void Draw(Vector2 parentPosition, bool grayedOut = false)
        {
            if (this.tile != null)
            {
                this.tile.Draw(parentPosition + this.RelativePosition, grayedOut);
            }

            if (this.icon != null)
            {
                this.icon.Draw(parentPosition + this.RelativePosition, grayedOut);
            }

            if (this.text != null)
            {
                this.text.Draw(parentPosition + this.RelativePosition, grayedOut);
            }

            if (this.textIcon != null)
            {
                this.textIcon.Draw(parentPosition + this.RelativePosition, grayedOut);
            }
        }

        /// <summary>
        /// Sets the text displayed as the icon of the button. 
        /// </summary>
        /// <param name="newText">The new text to use.</param>
        public void SetIconText(string newText)
        {
            if (this.textIcon != null)
            {
                this.textIcon.SetText(newText);
            }
        }
    }
}
