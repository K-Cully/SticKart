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
        protected MenuImage tile;

        /// <summary>
        /// The icon to display on the button.
        /// </summary>
        protected MenuImage icon;

        /// <summary>
        /// The text to display on the button.
        /// </summary>
        protected MenuText text;
                
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
            this.tile = new MenuImage(Vector2.Zero, tile);
            this.icon = new MenuImage(Vector2.Zero, icon);
            Vector2 relativeTextPosition = new Vector2(0.0f, (tile.Height * 0.5f) - (text.Height * 1.5f));
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
        public override void Draw(Vector2 parentPosition)
        {
            if (this.tile != null)
            {
                this.tile.Draw(parentPosition + this.relativePosition);
            }

            if (this.icon != null)
            {
                this.icon.Draw(parentPosition + this.relativePosition);
            }

            if (this.text != null)
            {
                this.text.Draw(parentPosition + this.relativePosition);
            }
        }
    }
}
