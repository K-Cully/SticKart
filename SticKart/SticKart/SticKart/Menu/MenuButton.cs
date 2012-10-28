using System;
using Microsoft.Xna.Framework;
using SticKart.Display;

namespace SticKart.Menu
{
    /// <summary>
    /// A button menu item.
    /// </summary>
    class MenuButton : MenuItem
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
        /// The button's bounding box.
        /// </summary>
        protected Rectangle boundingBox;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class.
        /// </summary>
        /// <param name="relativePosition">The centre position of the menu item, relative to it's owner/parent.</param>
        /// <param name="tile">The button's background sprite.</param>
        /// <param name="icon">The icon to display on the button.</param>
        /// <param name="text">The text to display on the button.</param>
        /// <param name="name">The name of the button.</param>
        public MenuButton(Vector2 relativePosition, Sprite tile, Sprite icon, RenderableText text, string name)
            : base(relativePosition)
        {
            this.Name = name;
            this.boundingBox = new Rectangle((int)(relativePosition.X - tile.Width / 2), (int)(relativePosition.Y - tile.Height / 2), (int)tile.Width, (int)tile.Height);
            this.tile = new MenuImage(Vector2.Zero, tile);
            this.icon = new MenuImage(Vector2.Zero, icon);            
            Vector2 relativeTextPosition = new Vector2(0.0f, tile.Height * 0.5f - text.Height * 1.5f);
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
        /// The name of the button.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Checks if the click position passed in intersects the button.
        /// </summary>
        /// <param name="clickPosition">The position of the click.</param>
        /// <param name="parentPosition">The position of the button's parent/owner.</param>
        /// <returns></returns>
        public bool CheckForClick(Vector2 clickPosition, Vector2 parentPosition)
        {
            return this.boundingBox.Contains((int)(clickPosition.X - parentPosition.X), (int)(clickPosition.Y - parentPosition.Y));
        }

        /// <summary>
        /// Draws the button to the screen.
        /// </summary>
        /// <param name="parentPosition">The position of the button's parent/owner.</param>
        public override void Draw(Vector2 parentPosition)
        {
            if (this.tile != null)
            {
                this.tile.Draw(parentPosition + base.relativePosition);
            }

            if (this.icon != null)
            {
                this.icon.Draw(parentPosition + base.relativePosition);
            }

            if (this.text != null)
            {
                this.text.Draw(parentPosition + base.relativePosition);
            }
        }
    }
}
