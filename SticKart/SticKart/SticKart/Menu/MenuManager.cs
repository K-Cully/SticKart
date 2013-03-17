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
    using Audio;
    using Display;
    using Display.Notification;
    using Game;
    using Game.Level;
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
        /// Indicates which character in the name the player is trying to change.
        /// </summary>
        private int nameCharacterSelected;

        /// <summary>
        /// A value indicating whether the local leaderboard is active or not.
        /// </summary>
        private bool localLeaderboardActive;

        /// <summary>
        /// A left arrow sprite.
        /// </summary>
        private Sprite leftArrow;

        /// <summary>
        /// A right arrow sprite.
        /// </summary>
        private Sprite rightArrow;

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
            this.nameCharacterSelected = -1;
            this.localLeaderboardActive = false;
            this.leftArrow = new Sprite();
            this.rightArrow = new Sprite();
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
                
        /// <summary>
        /// An event triggered on the user selecting to edit a level.
        /// </summary>
        public event Action<int> OnEditLevelSelected;

        /// <summary>
        /// An event triggered on the user selecting to save a custom level.
        /// </summary>
        public event Action<int> OnEditorSaveSelected;

        /// <summary>
        /// An event triggered on the user selecting to undo from the editor overlay.
        /// </summary>
        public event Action<int> OnEditorUndoSelected;

        /// <summary>
        /// An event triggered on the user selecting an entity type from the editor overlay.
        /// </summary>
        public event Action<int> OnEditorTypeSelected;

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
            this.ActiveMenu = MenuType.NamePrompt;
            NotificationManager.AddNotification(NotificationType.PushGesture);
            this.menus.Add(MenuType.Main, MenuFactory.CreateMainMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.LeaderboardTypeSelect, MenuFactory.CreateLeaderboardTypeMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.LeaderboardSelect, MenuFactory.CreateLevelSelectMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, this.screenDimensions.X, gameSettings));
            this.menus.Add(MenuType.Leaderboard, MenuFactory.CreateLeaderboardMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.LevelSelect, MenuFactory.CreateLevelSelectMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, this.screenDimensions.X, gameSettings));
            this.menus.Add(MenuType.LetterInput,  MenuFactory.CreateLetterInputMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, char.MaxValue));
            this.menus.Add(MenuType.LetterInputA, MenuFactory.CreateLetterInputMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, 'a'));
            this.menus.Add(MenuType.LetterInputG, MenuFactory.CreateLetterInputMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, 'g'));
            this.menus.Add(MenuType.LetterInputM, MenuFactory.CreateLetterInputMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, 'm'));
            this.menus.Add(MenuType.LetterInputS, MenuFactory.CreateLetterInputMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, 's'));
            this.menus.Add(MenuType.LetterInputY, MenuFactory.CreateLetterInputMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, 'y'));
            this.menus.Add(MenuType.LetterInput4, MenuFactory.CreateLetterInputMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, '4'));
            this.menus.Add(MenuType.LevelComplete, MenuFactory.CreateLevelCompleteMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.NamePrompt, MenuFactory.CreateNamePromptMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, gameSettings.PlayerName));
            this.menus.Add(MenuType.Options, MenuFactory.CreateOptionsMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, gameSettings));
            this.menus.Add(MenuType.CustomContent, MenuFactory.CreateCustomContentMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.EditLevelSelect, MenuFactory.CreateCustomLevelSelectMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, this.screenDimensions.X, gameSettings));
            this.menus.Add(MenuType.EditorOverlayMain, MenuFactory.CreateEditorMainMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.EditorOverlayMenu, MenuFactory.CreateEditorSubMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.CustomLevelSelect, MenuFactory.CreateCustomLevelSelectMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f, this.screenDimensions.X, gameSettings));
            this.menus.Add(MenuType.EditorOverlayType, MenuFactory.CreateEditorTypeMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.Pause, MenuFactory.CreatePauseMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.menus.Add(MenuType.About, MenuFactory.CreateAboutMenu(contentManager, spriteBatch, this.screenDimensions / 2.0f));
            this.leftArrow.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LeftArrow);
            this.rightArrow.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.RightArrow);
        }

        /// <summary>
        /// Updates the active menu and checks for user selections.
        /// </summary>
        /// <param name="selectionPosition">The selection position.</param>
        /// <param name="selectionName">The selection name.</param>
        /// <param name="gameSettings">The game settings.</param>
        public void Update(Vector2 selectionPosition, string selectionName, ref GameSettings gameSettings)
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
                    else if (this.ActiveMenu == MenuType.CustomLevelSelect)
                    {
                        this.HandleCustomLevelSelection(selectedItemName);
                    }
                    else if (this.ActiveMenu == MenuType.LeaderboardSelect)
                    {
                        this.HandleLeaderboardSelection(selectedItemName, gameSettings);
                    }
                    else if (this.ActiveMenu == MenuType.EditLevelSelect)
                    {
                        this.HandleLevelEditSelection(selectedItemName);
                    }
                    else if (this.ActiveMenu == MenuType.Options)
                    {
                        this.HandleOptionsSelection(selectedItemName, ref gameSettings);
                    }
                    else if (this.ActiveMenu == MenuType.EditorOverlayType)
                    {
                        this.HandleEditorTypeSelection(selectedItemName);
                    }
                    else if (this.ActiveMenu == MenuType.NamePrompt || this.ActiveMenu == MenuType.LetterInput || this.ActiveMenu == MenuType.LetterInputA
                        || this.ActiveMenu == MenuType.LetterInputG || this.ActiveMenu == MenuType.LetterInputM || this.ActiveMenu == MenuType.LetterInputS
                        || this.ActiveMenu == MenuType.LetterInputY || this.ActiveMenu == MenuType.LetterInput4)
                    {
                        this.HandleNameSelection(selectedItemName, ref gameSettings);
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
        /// <param name="highScoreSet">A value indicating what high score was set, if any.</param>
        /// <param name="score">The player's score for the level.</param>
        /// <param name="ratingLevel">The overall rating for the level in the inclusive range 0 to 2.</param>
        public void SetLevelCompleteMenuText(HighScoreType highScoreSet, int score, int ratingLevel)
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
                            (levelCompleteItems[count] as MenuText).SetText(MenuConstants.GetHighScoreText(highScoreSet));
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
        /// Updates the number of active buttons in the pause menu.
        /// </summary>
        /// <param name="playerDied">A value indicating whether the player died or not.</param>
        public void UpdatePauseMenuTiles(bool playerDied)
        {
            if (playerDied)
            {
                this.menus[MenuType.Pause].SelectablesActive = 2;
            }
            else
            {
                this.menus[MenuType.Pause].SelectablesActive = 3;
            }
        }

        /// <summary>
        /// Unlocks the menu entries for unlocked levels.
        /// </summary>
        /// <param name="levelsUnlocked">The number of unlocked levels.</param>
        public void UpdateLevelsUnlocked(int levelsUnlocked)
        {
            this.menus[MenuType.LeaderboardSelect].SelectablesActive = levelsUnlocked + 1;
            this.menus[MenuType.LevelSelect].SelectablesActive = levelsUnlocked + 1;
        }

        /// <summary>
        /// Unlocks the menu entries for created levels.
        /// </summary>
        /// <param name="levelsCreated">The number of unlocked levels.</param>
        public void UpdateLevelsCreated(int levelsCreated)
        {
            this.menus[MenuType.EditLevelSelect].SelectablesActive = levelsCreated + 1;
            this.menus[MenuType.CustomLevelSelect].SelectablesActive = levelsCreated + 1;
        }

        /// <summary>
        /// Draws the active menu.
        /// </summary>
        public void Draw()
        {
            if (this.menus[this.ActiveMenu] != null)
            {
                this.menus[this.ActiveMenu].Draw();
                if (this.menus[this.ActiveMenu].HasPages)
                {
                    Sprite.Draw(this.leftArrow, new Vector2(this.leftArrow.Width, this.screenDimensions.Y * 0.5f), 0.0f);
                    Sprite.Draw(this.rightArrow, new Vector2(this.screenDimensions.X - this.rightArrow.Width, this.screenDimensions.Y * 0.5f), 0.0f);
                }
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
        /// Manages state and setting changes from the editor type select menu, based on user input.
        /// </summary>
        /// <param name="selectedItemName">The name of the selected item.</param>
        private void HandleEditorTypeSelection(string selectedItemName)
        {
            int type = -1;
            switch (selectedItemName)
            {
                case MenuConstants.FloorButtonName:
                    type = 0;
                    break;
                case MenuConstants.PlatformButtonName:
                    type = 1;
                    break;
                case MenuConstants.MainTypeButtonName:
                    type = 2;
                    break;
                case MenuConstants.BonusButtonName:
                    type = 3;
                    break;
                case MenuConstants.ObstacleButtonName:
                    type = 4;
                    break;
                case MenuConstants.PowerUpButtonName:
                    type = 5;
                    break;
                default:
                    break;
            }

            if (type >= 0)
            {
                this.menus[this.ActiveMenu].Reset();
                this.ActiveMenu = MenuType.EditorOverlayMain;
                if (this.OnEditorTypeSelected != null)
                {
                    this.OnEditorTypeSelected(type);
                }
            }
        }

        /// <summary>
        /// Manages state and setting changes from the options menu, based on user input.
        /// </summary>
        /// <param name="selectedItemName">The name of the selected item.</param>
        /// <param name="gameSettings">The game settings.</param>
        private void HandleOptionsSelection(string selectedItemName, ref GameSettings gameSettings)
        {
            Collection<MenuItem> optionsItems = this.menus[this.ActiveMenu].MenuItems;
            int count = 0;
            for (count = 0; count < optionsItems.Count; count++)
            {
                if (optionsItems[count].Type == typeof(MenuButton) && (optionsItems[count] as MenuButton).Name.ToUpperInvariant() == selectedItemName.ToUpperInvariant())
                {
                    break;
                }
            }

            switch (selectedItemName)
            {
                case MenuConstants.MusicButtonName:
                    gameSettings.MusicEnabled = !gameSettings.MusicEnabled;
                    if (gameSettings.MusicEnabled)
                    {
                        AudioManager.PlayBackgroundMusic(false);
                        (optionsItems[count] as MenuButton).SetIconText(" ");
                    }
                    else
                    {
                        (optionsItems[count] as MenuButton).SetIconText("X");
                    }

                    break;
                case MenuConstants.SoundEffectsButtonName:
                    gameSettings.SoundEffectsEnabled = !gameSettings.SoundEffectsEnabled;
                    if (gameSettings.SoundEffectsEnabled)
                    {
                        (optionsItems[count] as MenuButton).SetIconText(" ");
                    }
                    else
                    {
                        (optionsItems[count] as MenuButton).SetIconText("X");
                    }

                    break;
                case MenuConstants.UploadButtonName:
                    gameSettings.UploadHighScores = !gameSettings.UploadHighScores;
                    if (gameSettings.UploadHighScores)
                    {
                        (optionsItems[count] as MenuButton).SetIconText(" ");
                    }
                    else
                    {
                        (optionsItems[count] as MenuButton).SetIconText("X");
                    }

                    break;
                case MenuConstants.NotificationButtonName:
                    NotificationManager.Instance.Reset();
                    NotificationManager.AddNotification(NotificationType.PushGesture);
                    break;
                case MenuConstants.NameButtonName:
                    this.ActiveMenu = MenuType.NamePrompt;
                    this.menus[this.ActiveMenu].Reset();
                    break;
                case MenuConstants.DoneButtonName:
                    this.ActiveMenu = MenuType.Main;
                    this.menus[this.ActiveMenu].Reset();
                    break;
                default:
                    break;
            }            
        }

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
                LevelScoreTable scoreTable = this.localLeaderboardActive ? gameSettings.LevelScoreTables[level - 1] : gameSettings.GetGlobalScoresFor(level);
                int changedCount = 0;
                for (int count = 0; count < leaderboardItems.Count; count++)
                {
                    if (leaderboardItems[count].Type == typeof(MenuText) && (leaderboardItems[count] as MenuText).IsChangeable)
                    {
                        (leaderboardItems[count] as MenuText).SetText(scoreTable.Scores[changedCount].ToString());
                        changedCount++;
                    }
                }
            }
            catch
            {
                this.ActiveMenu = MenuType.LeaderboardTypeSelect;
            }
        }

        /// <summary>
        /// Handles state changes from the name prompt and letter input menus.
        /// </summary>
        /// <param name="selectedItemName">The selected item name.</param>
        /// <param name="gameSettings">The game settings.</param>
        private void HandleNameSelection(string selectedItemName, ref GameSettings gameSettings)
        {
            if (this.ActiveMenu == MenuType.NamePrompt)
            {
                if (selectedItemName == MenuConstants.DoneButtonName)
                {
                    this.menus[this.ActiveMenu].Reset();
                    NotificationManager.AddNotification(NotificationType.VoiceCommand);
                    this.ActiveMenu = MenuType.Main;
                    this.nameCharacterSelected = -1;
                }
                else
                {
                    this.menus[this.ActiveMenu].Reset();
                    this.nameCharacterSelected = int.Parse(selectedItemName);
                    this.ActiveMenu = MenuType.LetterInput;
                }
            }
            else if (this.ActiveMenu == MenuType.LetterInput)
            {
                switch (selectedItemName.ToLowerInvariant())
                {
                    case MenuConstants.AToF:
                        this.ActiveMenu = MenuType.LetterInputA;
                        break;
                    case MenuConstants.GToL:
                        this.ActiveMenu = MenuType.LetterInputG;
                        break;
                    case MenuConstants.MToR:
                        this.ActiveMenu = MenuType.LetterInputM;
                        break;
                    case MenuConstants.SToX:
                        this.ActiveMenu = MenuType.LetterInputS;
                        break;
                    case MenuConstants.YToThree:
                        this.ActiveMenu = MenuType.LetterInputY;
                        break;
                    case MenuConstants.FourToNine:
                        this.ActiveMenu = MenuType.LetterInput4;
                        break;
                    default:
                        this.ActiveMenu = MenuType.NamePrompt;
                        break;
                }
            }
            else
            {
                char[] newName = gameSettings.PlayerName.ToCharArray();
                newName[this.nameCharacterSelected] = selectedItemName.ToUpperInvariant().ToCharArray()[0];
                gameSettings.PlayerName = new string(newName);
                this.menus[this.ActiveMenu].Reset();
                this.ActiveMenu = MenuType.NamePrompt;
                (this.menus[this.ActiveMenu].MenuItems[this.nameCharacterSelected] as MenuButton).SetIconText(selectedItemName.ToUpperInvariant());
                this.nameCharacterSelected = -1;
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
        /// Manages state changes from the custom level select menu, based on user input.
        /// </summary>
        /// <param name="selectedItemName">The name of the selected item.</param>
        private void HandleCustomLevelSelection(string selectedItemName)
        {
            try
            {
                int level = int.Parse(selectedItemName);
                if (this.OnBeginLevelDetected != null)
                {
                    this.OnBeginLevelDetected(-level);
                }
            }
            catch
            {
                this.ActiveMenu = MenuType.CustomContent;
            }
        }

        /// <summary>
        /// Manages state changes from the level edit select menu, based on user input.
        /// </summary>
        /// <param name="selectedItemName">The name of the selected item.</param>
        private void HandleLevelEditSelection(string selectedItemName)
        {
            try
            {
                int level = int.Parse(selectedItemName);
                if (this.OnEditLevelSelected != null)
                {
                    this.OnEditLevelSelected(level);
                }
            }
            catch
            {
                this.ActiveMenu = MenuType.CustomContent;
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
                    NotificationManager.AddNotification(NotificationType.SwipeGesture);
                    if (this.ActiveMenu == MenuType.CustomContent)
                    {
                        this.ActiveMenu = MenuType.CustomLevelSelect;
                    }
                    else
                    {
                        this.ActiveMenu = MenuType.LevelSelect;
                    }

                    this.menus[this.ActiveMenu].Reset();
                    break;
                case MenuConstants.OptionsButtonName:
                    this.ActiveMenu = MenuType.Options;
                    this.menus[this.ActiveMenu].Reset();
                    break;
                case MenuConstants.LeaderboardButtonName:
                    this.ActiveMenu = MenuType.LeaderboardTypeSelect;                    
                    this.menus[this.ActiveMenu].Reset();
                    break;
                case MenuConstants.LocalButtonName:
                    this.ActiveMenu = MenuType.LeaderboardSelect;
                    NotificationManager.AddNotification(NotificationType.SwipeGesture);
                    this.localLeaderboardActive = true;
                    this.menus[this.ActiveMenu].Reset();
                    break;
                case MenuConstants.GlobalButtonName:
                    this.ActiveMenu = MenuType.LeaderboardSelect;
                    NotificationManager.AddNotification(NotificationType.SwipeGesture);
                    this.localLeaderboardActive = false;
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
                    else if (this.ActiveMenu == MenuType.EditorOverlayMenu)
                    {
                        this.menus[this.ActiveMenu].Reset();
                        this.ActiveMenu = MenuType.CustomContent;
                        this.menus[this.ActiveMenu].Reset();                        
                    }
                    else
                    {
                        this.menus[this.ActiveMenu].Reset();
                        this.ActiveMenu = MenuType.Main;
                        this.menus[this.ActiveMenu].Reset();
                    }

                    break;
                case MenuConstants.BackButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    if (this.ActiveMenu == MenuType.Options || this.ActiveMenu == MenuType.LeaderboardTypeSelect || this.ActiveMenu == MenuType.CustomContent || this.ActiveMenu == MenuType.About)
                    {
                        this.ActiveMenu = MenuType.Main;
                    }
                    else if (this.ActiveMenu == MenuType.LeaderboardSelect)
                    {
                        this.ActiveMenu = MenuType.LeaderboardTypeSelect;
                    }
                    else if (this.ActiveMenu == MenuType.Leaderboard)
                    {
                        this.ActiveMenu = MenuType.LeaderboardSelect;
                    }
                    else if (this.ActiveMenu == MenuType.EditorOverlayMenu)
                    {
                        this.ActiveMenu = MenuType.EditorOverlayMain;
                    }

                    this.menus[this.ActiveMenu].Reset();
                    break;
                case MenuConstants.ContinueButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    if (this.ActiveMenu == MenuType.LevelComplete)
                    {
                        if (this.OnBeginLevelDetected != null)
                        {
                            this.OnBeginLevelDetected(0);
                        }
                    }
                    else
                    {
                        if (this.OnResumeGameDetected != null)
                        {
                            this.OnResumeGameDetected(true);
                        }
                    }

                    break;
                case MenuConstants.RetryButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    this.OnBeginLevelDetected(int.MaxValue);
                    break;
                case MenuConstants.CustomButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    this.ActiveMenu = MenuType.CustomContent;
                    break;
                case MenuConstants.EditButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    this.ActiveMenu = MenuType.EditLevelSelect;
                    break;
                case MenuConstants.NewButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    if (this.OnEditLevelSelected != null)
                    {
                        this.OnEditLevelSelected(0);
                    }

                    break;
                case MenuConstants.MenuButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    this.ActiveMenu = MenuType.EditorOverlayMenu;
                    break;
                case MenuConstants.UndoButtonName:
                    if (this.OnEditorUndoSelected != null)
                    {
                        this.OnEditorUndoSelected(0);
                    }

                    break;
                case MenuConstants.ChangeButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    this.ActiveMenu = MenuType.EditorOverlayType;
                    break;
                case MenuConstants.SaveButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    if (this.OnEditorSaveSelected != null)
                    {
                        this.OnEditorSaveSelected(0);
                    }

                    this.ActiveMenu = MenuType.EditorOverlayMain;
                    break;
                case MenuConstants.AboutButtonName:
                    this.menus[this.ActiveMenu].Reset();
                    this.ActiveMenu = MenuType.About;
                    break;
                default:
                    break;
            }
        }
    }
}
