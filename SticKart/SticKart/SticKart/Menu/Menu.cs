namespace SticKart.Menu
{
    using System.Collections.ObjectModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A menu class used to hold and display menu items.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// The menu items held in the menu.
        /// </summary>
        private Collection<MenuItem> menuItems;

        /// <summary>
        /// A collection of names of selectable menu items.
        /// </summary>
        private Collection<string> selectableItemNames;

        /// <summary>
        /// The index of the highlighted menu item, if any.
        /// </summary>
        private int highlightedIndex;

        /// <summary>
        /// The currently selected row.
        /// </summary>
        private int selectedRow;

        /// <summary>
        /// The currently selected column.
        /// </summary>
        private int selectedColumn;

        /// <summary>
        /// The number of selectable rows in the menu.
        /// </summary>
        private int selectableRows;

        /// <summary>
        /// The number of delectable columns in the menu.
        /// </summary>
        private int selectableColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="position">The centre position of the menu.</param>
        /// <param name="selectableRows">The number of selectable rows in the menu.</param>
        /// <param name="selectableColumns">The number of delectable columns in the menu.</param>
        public Menu(Vector2 position, int selectableRows, int selectableColumns)
        {
            this.Position = position;
            this.menuItems = new Collection<MenuItem>();
            this.selectableItemNames = new Collection<string>();
            this.selectableRows = selectableRows;
            this.selectableColumns = selectableColumns;
            this.selectedRow = 0;
            this.selectedColumn = 0;
            this.highlightedIndex = this.selectedRow + this.selectedColumn;
        }

        /// <summary>
        /// Gets the position of the currently highlighted item.
        /// </summary>
        public Vector2 HighlightedItemPosition
        {
            get
            {
                if (this.highlightedIndex > -1 && this.highlightedIndex < this.selectableItemNames.Count)
                {
                    Vector2 position = this.Position;
                    string name = this.selectableItemNames[this.highlightedIndex];
                    foreach (MenuItem menuItem in this.menuItems)
                    {
                        if (typeof(MenuSelectableItem).IsAssignableFrom(menuItem.Type))
                        {
                            if ((menuItem as MenuSelectableItem).Name == name)
                            {
                                position += (menuItem as MenuSelectableItem).RelativePosition;
                                break;
                            }
                        }
                    }
                    return position;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        /// <summary>
        /// Gets the position of the menu.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Gets the collection of selectable item names.
        /// </summary>
        public Collection<string> SelectableItemNames
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
                this.selectableItemNames.Add((menuItem as MenuSelectableItem).Name.ToUpperInvariant());             
            }

            this.menuItems.Add(menuItem);
        }

        /// <summary>
        /// Checks if the selection made is in the menu.
        /// </summary>
        /// <param name="selectionPosition">The position selected.</param>
        /// <returns>The name of the selected item or null if not found.</returns>
        public string CheckForSelection(Vector2 selectionPosition)
        {            
            if (selectionPosition == Vector2.Zero)
            {
                return null;
            }
            else
            {
                string itemFound = null;
                foreach (MenuItem menuItem in this.menuItems)
                {
                    if (typeof(MenuSelectableItem).IsAssignableFrom(menuItem.Type))
                    {
                        if ((menuItem as MenuSelectableItem).CheckForClick(selectionPosition, this.Position))
                        {
                            itemFound = (menuItem as MenuSelectableItem).Name;
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
        /// <returns>The name of the selected item or null if not found.</returns>
        public string CheckForSelection(string name)
        {
            if (name == null)
            {
                return null;
            }
            else
            {
                string itemFound = null;
                foreach (MenuItem menuItem in this.menuItems)
                {
                    if (typeof(MenuSelectableItem).IsAssignableFrom(menuItem.Type))
                    {
                        if (name.ToUpperInvariant() == (menuItem as MenuSelectableItem).Name.ToUpperInvariant())
                        {
                            itemFound = (menuItem as MenuSelectableItem).Name;
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
            foreach (MenuItem menuItem in this.menuItems)
            {
                menuItem.Draw(this.Position);
            }
        }

        /// <summary>
        /// Moves the highlighted item down.
        /// </summary>
        public void MoveSelectionDown()
        {
            this.selectedRow++;
            if (this.selectedRow >= this.selectableRows)
            {
                this.selectedRow = 0;
            }

            this.highlightedIndex = (this.selectedRow * this.selectableColumns) + this.selectedColumn;
            if (this.highlightedIndex >= this.selectableItemNames.Count)
            {
                this.selectedRow = 0;
            }
        }

        /// <summary>
        /// Moves the highlighted item up.
        /// </summary>
        public void MoveSelectionUp()
        {
            this.selectedRow--;
            if (this.selectedRow < 0)
            {
                this.selectedRow = this.selectableRows - 1;
            }

            this.highlightedIndex = (this.selectedRow * this.selectableColumns) + this.selectedColumn;
        }

        /// <summary>
        /// Moves the highlighted item right.
        /// </summary>
        public void MoveSelectionRight()
        {
            this.selectedColumn++;
            if (this.selectedColumn >= this.selectableColumns)
            {
                this.selectedColumn = 0;
            }

            this.highlightedIndex = (this.selectedRow * this.selectableColumns) + this.selectedColumn;
            if (this.highlightedIndex >= this.selectableItemNames.Count)
            {
                this.selectedColumn = 0;
            }
        }

        /// <summary>
        /// Moves the highlighted item to the left.
        /// </summary>
        public void MoveSelectionLeft()
        {
            this.selectedColumn--;
            if (this.selectedColumn < 0)
            {
                this.selectedColumn = this.selectableColumns - 1;
            }

            this.highlightedIndex = (this.selectedRow * this.selectableColumns) + this.selectedColumn;
        }
    }
}
