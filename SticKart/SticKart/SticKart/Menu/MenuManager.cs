// -----------------------------------------------------------------------
// <copyright file="MenuManager.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Display;
    using Display.Notification;
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

        #region events

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

        #endregion

        #region public_accessors

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

        #endregion

        /// <summary>
        /// Initializes all menus and loads all menu content.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use in drawing menu items.</param>
        /// <param name="contentManager">The content manager to load content with.</param>
        /// <param name="gameSettings">The game settings.</param>
        public void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager, GameSettings gameSettings)
        {
            this.ActiveMenu = MenuType.Main; // TODO: implement all menus.
            NotificationManager.AddNotification(NotificationType.PushGesture);
            NotificationManager.AddNotification(NotificationType.VoiceCommand);
            this.menus.Add(MenuType.Main, MenuFactory.CreateMainMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.Options, MenuFactory.CreatePlaceholderMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.LeaderboardSelect, MenuFactory.CreateLevelSelectMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, this.screenDimensions.X, gameSettings));
            this.menus.Add(MenuType.Leaderboard, MenuFactory.CreatePlaceholderMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.LevelSelect, MenuFactory.CreateLevelSelectMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, this.screenDimensions.X, gameSettings));
            this.menus.Add(MenuType.NameInput, null);
            this.menus.Add(MenuType.LevelComplete, MenuFactory.CreateLevelCompleteMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
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
                        this.HandleLevelSelection(selectedItemName);
                    }
                    else if (this.ActiveMenu == MenuType.LeaderboardSelect)
                    {
                        this.HandleLeaderboardSelection(selectedItemName, gameSettings);
                    }
                    else
                    {
                        this.HandleStandardSelection(selectedItemName);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the list of words which can be used to select menu items.
        /// </summary>
        /// <returns>The list of words which can be used to select menu items</returns>
        public List<string> GetAllSelectableNames()
        {
            List<string> selectableNames = new List<string>();
            foreach (MenuType menuType in this.menus.Keys)
            {
                switch (menuType)
                {
                    case MenuType.LevelSelect:
                        foreach (string name in this.menus[menuType].SelectableItemNames)
                        {
                            if (name == MenuConstants.BackButtonName)
                            {
                                selectableNames.Add(name);
                            }
                            else
                            {
                                selectableNames.Add(ConvertToWords.ConvertIntToWords(int.Parse(name)));
                            }
                        }

                        break;
                    case MenuType.LeaderboardSelect:
                        // Names will be identical to level select.
                        break;
                    default:
                        if (this.menus[menuType] != null)
                        {
                            selectableNames.AddRange(this.menus[menuType].SelectableItemNames);
                        }

                        break;
                }
            }

            return selectableNames;
        }

        /// <summary>
        /// Changes the page of the current menu.
        /// </summary>
        /// <param name="right">A value indicating whether to move to the right or the left.</param>
        public void FlipPage(bool right)
        {
            if (this.menus[this.ActiveMenu] != null)
            {
                this.menus[this.ActiveMenu].FlipPage(right);
            }
        }

        /// <summary>
        /// Sets the text of the level complete menu based on the player's performance.
        /// </summary>
        /// <param name="setHighScore">A value indicating whether the player set ahigh score or not.</param>
        /// <param name="score">The player's score for the level.</param>
        /// <param name="ratingLevel">The overall rating for the level in the inclusive range 0 to 2.</param>
        public void SetLevelCompleteMenuText(bool setHighScore, int score, int ratingLevel)
        {
            Collection<MenuItem> levelCompleteItems = this.menus[MenuType.LevelComplete].MenuItems;
            int changedCount = 0;
            for (int count = 0; count < levelCompleteItems.Count; count++)
            {
                if (levelCompleteItems[count].Type == typeof(MenuText) && (levelCompleteItems[count] as MenuText).IsChangeable)
                {
                    switch (changedCount)
                    {
                        case 0:
                            (levelCompleteItems[count] as MenuText).SetText(MenuConstants.GetHighScoreText(setHighScore));
                            break;
                        case 1:
                            (levelCompleteItems[count] as MenuText).SetText(score.ToString());
                            break;
                        case 2:
                            (levelCompleteItems[count] as MenuText).SetText(MenuConstants.GetRating(ratingLevel));
                            break;
                        default:
                            count = levelCompleteItems.Count;
                            break;
                    }

                    changedCount++;
                }
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

        #region selection_movement

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

        #endregion

        /// <summary>
        /// Manages state changes from the leaderboard select menu, based on user input.
        /// </summary>
        /// <param name="selectedItemName">The name of the selected item.</param>
        /// <param name="gameSettings">The game settings.</param>
        private void HandleLeaderboardSelection(string selectedItemName, GameSettings gameSettings)
        {
            try
            {
                int level = int.Parse(selectedItemName);
                this.ActiveMenu = MenuType.Leaderboard;                
                Collection<MenuItem> leaderboardItems = this.menus[MenuType.Leaderboard].MenuItems;
                int changedCount = 0;
                for (int count = 0; count < leaderboardItems.Count; count++)
                {
                    if (leaderboardItems[count].Type == typeof(MenuText) && (leaderboardItems[count] as MenuText).IsChangeable)
                    {
                        (leaderboardItems[count] as MenuText).SetText(gameSettings.LevelScoreTables[level].Scores[changedCount].ToString());
                        changedCount++;
                    }
                }
            }
            catch
            {
                this.ActiveMenu = MenuType.Main;
            }
        }

        /// <summary>
        /// Manages state changes from the level select menu, based on user input.
        /// </summary>
        /// <param name="selectedItemName">The name of the selected item.</param>
        private void HandleLevelSelection(string selectedItemName)
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

        /// <summary>
        /// Manages standard state changes based on menu selection.
        /// </summary>
        /// <param name="selectedItemName">The name of the selected menu item.</param>
        private void HandleStandardSelection(string selectedItemName)
        {
            switch (selectedItemName)
            {
                case MenuConstants.PlayButtonName:
                    this.ActiveMenu = MenuType.LevelSelect;
                    NotificationManager.AddNotification(NotificationType.SwipeGesture);
                    this.menus[this.ActiveMenu].Reset();
                    break;
                case MenuConstants.OptionsButtonName:
                    this.ActiveMenu = MenuType.Options;
                    this.menus[this.ActiveMenu].Reset();
                    break;
                case MenuConstants.LeaderboardButtonName:
                    this.ActiveMenu = MenuType.LeaderboardSelect;
                    this.menus[this.ActiveMenu].Reset();
                    break;
                case MenuConstants.ExitButtonName:
                    if (this.ActiveMenu == MenuType.Main)
                    {
                        this.ActiveMenu = MenuType.None;
                        if (this.OnQuitGameDetected != null)
                        {
                            this.OnQuitGameDetected(true);
                        }
                    }
                    else
                    {
                        this.menus[this.ActiveMenu].Reset();
                        this.ActiveMenu = MenuType.Main;
                        this.menus[this.ActiveMenu].Reset();
                    }

                    break;
                case MenuConstants.BackButtonName:
                    if (this.ActiveMenu == MenuType.Options || this.ActiveMenu == MenuType.LeaderboardSelect)
                    {
                        this.ActiveMenu = MenuType.Main;
                        this.menus[this.ActiveMenu].Reset();
                    }
                    else if (this.ActiveMenu == MenuType.Leaderboard)
                    {
                        this.ActiveMenu = MenuType.LeaderboardSelect;
                        this.menus[this.ActiveMenu].Reset();
                    }

                    break;
                case MenuConstants.ContinueButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    this.OnBeginLevelDetected(0);
                    break;
                case MenuConstants.RetryButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    this.OnBeginLevelDetected(int.MaxValue);
                    break;
                default:
                    break;
            }
        }
    }
}
