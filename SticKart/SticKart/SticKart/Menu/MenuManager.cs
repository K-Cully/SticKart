namespace SticKart.Menu
{
    using System;
    using System.Collections.Generic;
    using Game;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    
    /// <summary>
    /// The main controller of the menu system.
    /// </summary>
    public class MenuManager
    {
        /// <summary>
        /// The current display size in pixels.
        /// </summary>
        private Vector2 screenDimensions;
        
        /// <summary>
        /// A lookup table of available menus.
        /// </summary>
        private Dictionary<MenuType, Menu> menus;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuManager"/> class.
        /// </summary>
        /// <param name="screenDimensions">The current display size in pixels.</param>
        public MenuManager(Vector2 screenDimensions)
        {
            this.menus = new Dictionary<MenuType, Menu>();
            this.menus.Add(MenuType.None, null);
            this.screenDimensions = screenDimensions;
            this.ActiveMenu = MenuType.None;
        }

        /// <summary>
        /// An event triggered on the user selecting exit.
        /// </summary>
        public event Action<bool> OnQuitGameDetected;

        /// <summary>
        /// An event triggered on the user selecting to continue gameplay.
        /// </summary>
        public event Action<bool> OnResumeGameDetected;

        /// <summary>
        /// An event triggered on the user selecting to start a new level.
        /// </summary>
        public event Action<int> OnBeginLevelDetected;

        /// <summary>
        /// Gets or sets the currently active menu.
        /// </summary>
        public MenuType ActiveMenu { get; set; }

        /// <summary>
        /// Gets the position of the currently highlighted menu item.
        /// </summary>
        public Vector2 HighlightedPosition
        {
            get
            {
                if (this.menus[this.ActiveMenu] != null)
                {
                    return this.menus[this.ActiveMenu].HighlightedItemPosition;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        /// <summary>
        /// Gets the drawing position of the currently highlighted menu item.
        /// </summary>
        public Vector2 HighlightedDrawingPosition
        {
            get
            {
                if (this.menus[this.ActiveMenu] != null)
                {
                    return this.menus[this.ActiveMenu].Offset + this.menus[this.ActiveMenu].HighlightedItemPosition;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        /// <summary>
        /// Initializes all menus and loads all menu content.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use in drawing menu items.</param>
        /// <param name="contentManager">The content manager to load content with.</param>
        /// <param name="gameSettings">The game settings.</param>
        public void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager, GameSettings gameSettings)
        {
            this.ActiveMenu = MenuType.Main; // TODO: implement all menus.
            this.menus.Add(MenuType.Main, MenuFactory.CreateMainMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.Options, MenuFactory.CreatePlaceholderMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.Leaderboard, MenuFactory.CreatePlaceholderMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.LevelSelect, MenuFactory.CreateLevelSelectMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, this.screenDimensions.X, gameSettings));
            this.menus.Add(MenuType.NameInput, null);
            this.menus.Add(MenuType.LevelComplete, null);
        }

        /// <summary>
        /// Updates the active menu and checks for user selections.
        /// </summary>
        /// <param name="selectionPosition">The selection position.</param>
        /// <param name="selectionName">The selection name.</param>
        /// <param name="gameSettings">The game settings.</param>
        public void Update(Vector2 selectionPosition, string selectionName, GameSettings gameSettings)
        {
            if (this.menus[this.ActiveMenu] != null)
            {
                string selectedItemName = null;
                if (selectionPosition != Vector2.Zero)
                {
                    selectedItemName = this.menus[this.ActiveMenu].CheckForSelection(selectionPosition);
                }
                else if (selectionName != null)
                {
                    selectedItemName = this.menus[this.ActiveMenu].CheckForSelection(selectionName);
                }

                if (selectedItemName != null)
                {
                    if (this.ActiveMenu == MenuType.LevelSelect)
                    {
                        try
                        {
                            int level = int.Parse(selectedItemName);
                            if (this.OnBeginLevelDetected != null)
                            {
                                this.OnBeginLevelDetected(level);
                            }
                        }
                        catch
                        {
                            this.ActiveMenu = MenuType.Main;
                        }
                    }
                    else
                    {
                        switch (selectedItemName)
                        {
                            case SelectableNames.PlayButtonName:
                                this.ActiveMenu = MenuType.LevelSelect; //TODO: Fix
                                this.menus[this.ActiveMenu].Reset();
                                break;
                            case SelectableNames.OptionsButtonName:
                                this.ActiveMenu = MenuType.Options;
                                this.menus[this.ActiveMenu].Reset();
                                break;
                            case SelectableNames.LeaderboardButtonName:
                                this.ActiveMenu = MenuType.Leaderboard;
                                this.menus[this.ActiveMenu].Reset();
                                break;
                            case SelectableNames.ExitButtonName:
                                this.ActiveMenu = MenuType.None;
                                if (this.OnQuitGameDetected != null)
                                {
                                    this.OnQuitGameDetected(true);
                                }

                                break;
                            case SelectableNames.BackButtonName:
                                if (this.ActiveMenu == MenuType.Options || this.ActiveMenu == MenuType.Leaderboard)
                                {
                                    this.ActiveMenu = MenuType.Main;
                                    this.menus[this.ActiveMenu].Reset();
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Changes the page of the current menu.
        /// </summary>
        /// <param name="right">A value indicating whether to move to th right or the left.</param>
        public void FlipPage(bool right)
        {
            if (this.menus[this.ActiveMenu] != null)
            {
                this.menus[this.ActiveMenu].FlipPage(right);
            }
        }

        /// <summary>
        /// Draws the active menu.
        /// </summary>
        public void Draw()
        {
            if (this.menus[this.ActiveMenu] != null)
            {
                this.menus[this.ActiveMenu].Draw();
            }
        }

        /// <summary>
        /// Moves the selection right.
        /// </summary>
        public void MoveSelectionRight()
        {
            if (this.menus[this.ActiveMenu] != null)
            {
                this.menus[this.ActiveMenu].MoveSelectionRight();
            }
        }

        /// <summary>
        /// Moves the selection left.
        /// </summary>
        public void MoveSelectionLeft()
        {
            if (this.menus[this.ActiveMenu] != null)
            {
                this.menus[this.ActiveMenu].MoveSelectionLeft();
            }
        }

        /// <summary>
        /// Moves the selection up.
        /// </summary>
        public void MoveSelectionUp()
        {
            if (this.menus[this.ActiveMenu] != null)
            {
                this.menus[this.ActiveMenu].MoveSelectionUp();
            }
        }

        /// <summary>
        /// Moves the selection down.
        /// </summary>
        public void MoveSelectionDown()
        {
            if (this.menus[this.ActiveMenu] != null)
            {
                this.menus[this.ActiveMenu].MoveSelectionDown();
            }
        }
    }
}
