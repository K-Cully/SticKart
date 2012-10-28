using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SticKart.Menu
{
    /// <summary>
    /// A menu class used to hold and display menu items.
    /// </summary>
    class Menu
    {
        /// <summary>
        /// The menu items held in the menu.
        /// </summary>
        private List<MenuItem> menuItems;

        /// <summary>
        /// A list of names of selectable menu items.
        /// </summary>
        private List<string> selectableItemNames;

        /// <summary>
        /// Initalizes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="position"></param>
        public Menu(Vector2 position)
        {
            this.Position = position;
            this.menuItems = new List<MenuItem>();
            this.selectableItemNames = new List<string>();
        }
        
        /// <summary>
        /// Gets the position of the menu.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Gets the list of selectable item names.
        /// </summary>
        public List<string> SelectableItemNames
        {
            get
            {
                return this.selectableItemNames;
            }
        }

        /// <summary>
        /// Adds a menu item to the menu.
        /// </summary>
        /// <param name="menuItem">The menu item to add.</param>
        public void AddItem(MenuItem menuItem)
        {
            if (typeof(MenuSelectableItem).IsAssignableFrom(menuItem.Type))
            {
                selectableItemNames.Add((menuItem as MenuSelectableItem).Name.ToUpperInvariant()); // TODO: test.               
            }

            menuItems.Add(menuItem);
        }

        /// <summary>
        /// Checks if the selection made is in the menu.
        /// </summary>
        /// <param name="selectionPosition">The position selected.</param>
        /// <returns>Whether the selection is a selectable object in the menu or not.</returns>
        public bool CheckForSelection(Vector2 selectionPosition, string name)
        {            
            if (selectionPosition == Vector2.Zero)
            {
                return false;
            }
            else
            {
                bool itemFound = false;
                foreach (MenuItem menuItem in this.menuItems)
                {
                    if (typeof(MenuSelectableItem).IsAssignableFrom(menuItem.Type))
                    {
                        itemFound = (menuItem as MenuSelectableItem).CheckForClick(selectionPosition, this.Position);
                        if (itemFound)
                        {
                            break;
                        }
                    }
                }
                return itemFound;
            }
        }

        /// <summary>
        /// Checks if the selection made is in the menu.
        /// </summary>
        /// <param name="name">The name of the object to try to select.</param>
        /// <returns>Whether the selection is a selectable object in the menu or not.</returns>
        public bool CheckForSelection(string name)
        {
            if (name == null)
            {
                return false;
            }
            else
            {
                bool itemFound = false;
                foreach (MenuItem menuItem in this.menuItems)
                {
                    if (typeof(MenuSelectableItem).IsAssignableFrom(menuItem.Type))
                    {
                        itemFound = name.ToUpperInvariant() == (menuItem as MenuSelectableItem).Name.ToUpperInvariant();
                        if (itemFound)
                        {
                            break;
                        }
                    }
                }
                return itemFound;
            }            
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public void Draw()
        {
            foreach (MenuItem menuItem in menuItems)
            {
                menuItem.Draw(this.Position);
            }
        }
    }
}
