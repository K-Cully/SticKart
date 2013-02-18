﻿// -----------------------------------------------------------------------
// <copyright file="Menufactory.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Menu
{
    using Display;
    using Game;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A simple factory for creating different menu types.
    /// </summary>
    public class MenuFactory
    {
        /// <summary>
        /// Creates a main menu object.
        /// </summary>
        /// <param name="contentManager">The content manager to use to load resources.</param>
        /// <param name="spriteBatch">The sprite batch to attach to menu items.</param>
        /// <param name="position">The position of the menu.</param>
        /// <returns>The new menu created.</returns>
        public static Menu CreateMainMenu(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 position)
        {
            Menu mainMenu = new Menu(position, 2, 2);
            MenuButton button = null;
            Sprite largeButtonTile = new Sprite();
            largeButtonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeButtonTile);            
            float gapBetweenTiles = 32.0f;
            Vector2 relativePos = Vector2.Zero;

            Sprite largePlayIcon = new Sprite();
            largePlayIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargePlayIcon);
            RenderableText playGameText = new RenderableText();
            playGameText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.PlayButtonName.ToLowerInvariant());
            relativePos = new Vector2((-largeButtonTile.Width - gapBetweenTiles) * 0.5f, (-largeButtonTile.Height - gapBetweenTiles) * 0.5f);
            button = new MenuButton(relativePos, largeButtonTile, largePlayIcon, playGameText, MenuConstants.PlayButtonName);
            mainMenu.AddItem(button);

            Sprite largeOptionsIcon = new Sprite();
            largeOptionsIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeOptionsIcon);
            RenderableText optionsText = new RenderableText();
            optionsText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.OptionsButtonName.ToLowerInvariant());
            relativePos = new Vector2((largeButtonTile.Width + gapBetweenTiles) * 0.5f, (-largeButtonTile.Height - gapBetweenTiles) * 0.5f);
            button = new MenuButton(relativePos, largeButtonTile, largeOptionsIcon, optionsText, MenuConstants.OptionsButtonName);
            mainMenu.AddItem(button);
            
            Sprite largeLeaderboardIcon = new Sprite();
            largeLeaderboardIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeLeaderboardIcon);
            RenderableText leaderboardText = new RenderableText();
            leaderboardText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.LeaderboardButtonName.ToLowerInvariant());
            relativePos = new Vector2((-largeButtonTile.Width - gapBetweenTiles) * 0.5f, (largeButtonTile.Height + gapBetweenTiles) * 0.5f);
            button = new MenuButton(relativePos, largeButtonTile, largeLeaderboardIcon, leaderboardText, MenuConstants.LeaderboardButtonName);
            mainMenu.AddItem(button);

            Sprite largeExitIcon = new Sprite();
            largeExitIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeExitIcon);
            RenderableText exitText = new RenderableText();
            exitText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.ExitButtonName.ToLowerInvariant());
            relativePos = new Vector2((largeButtonTile.Width + gapBetweenTiles) * 0.5f, (largeButtonTile.Height + gapBetweenTiles) * 0.5f);
            button = new MenuButton(relativePos, largeButtonTile, largeExitIcon, exitText, MenuConstants.ExitButtonName);
            mainMenu.AddItem(button);

            return mainMenu;
        }

        /// <summary>
        /// Creates a level complete menu.
        /// </summary>
        /// <param name="contentManager">The content manager to use to load resources.</param>
        /// <param name="spriteBatch">The sprite batch to attach to menu items.</param>
        /// <param name="position">The position of the menu.</param>
        /// <returns>The new menu created.</returns>
        public static Menu CreateLevelCompleteMenu(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 position)
        {
            Menu levelCompleteMenu = new Menu(position, 1, 3);
            MenuButton button = null;
            MenuText text = null;
            Sprite largeButtonTile = new Sprite();
            largeButtonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeButtonTile);
            Vector2 relativePos = Vector2.Zero;
            float tileSpacing = 32.0f;

            relativePos = new Vector2(-largeButtonTile.Width - tileSpacing, largeButtonTile.Height * 0.75f);
            Sprite largePlayIcon = new Sprite();
            largePlayIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargePlayIcon);
            RenderableText continueText = new RenderableText();
            continueText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.ContinueButtonName.ToLowerInvariant());
            button = new MenuButton(relativePos, largeButtonTile, largePlayIcon, continueText, MenuConstants.ContinueButtonName);
            levelCompleteMenu.AddItem(button);

            relativePos.X = 0.0f;
            Sprite largeRetryIcon = new Sprite();
            largeRetryIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeRetryIcon);
            RenderableText retryText = new RenderableText();
            retryText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.RetryButtonName.ToLowerInvariant());
            button = new MenuButton(relativePos, largeButtonTile, largeRetryIcon, retryText, MenuConstants.RetryButtonName);
            levelCompleteMenu.AddItem(button);

            relativePos.X = largeButtonTile.Width + tileSpacing;
            Sprite largeExitIcon = new Sprite();
            largeExitIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeExitIcon);
            RenderableText exitText = new RenderableText();
            exitText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.ExitButtonName.ToLowerInvariant());
            button = new MenuButton(relativePos, largeButtonTile, largeExitIcon, exitText, MenuConstants.ExitButtonName);
            levelCompleteMenu.AddItem(button);

            relativePos = new Vector2(-largeButtonTile.Width - tileSpacing, -largeButtonTile.Height * 0.75f);
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    largeButtonTile = new Sprite();
                    largeButtonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeButtonTile);
                    MenuImage background = new MenuImage(relativePos, largeButtonTile);
                    levelCompleteMenu.AddItem(background);
                    relativePos.X += largeButtonTile.Width * 0.745f;
                }

                relativePos.Y += largeButtonTile.Height * 0.375f;
                relativePos.X = -largeButtonTile.Width - tileSpacing;
            }

            relativePos = new Vector2(0.0f, -largeButtonTile.Height * 1.1f);
            RenderableText levelComplete = new RenderableText();
            levelComplete.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, MenuConstants.LevelCompleteText);
            text = new MenuText(relativePos, levelComplete);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.25f);
            RenderableText highScoreText = new RenderableText();
            highScoreText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.GetHighScoreText(true));
            text = new MenuText(relativePos, highScoreText, true);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.5f);
            RenderableText scoreText = new RenderableText();
            scoreText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, MenuConstants.ScoreText);
            text = new MenuText(relativePos, scoreText);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.25f);
            RenderableText score = new RenderableText();
            score.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.ScoreDefaultText);
            text = new MenuText(relativePos, score, true);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.5f);
            RenderableText ratingText = new RenderableText();
            ratingText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, MenuConstants.RatingText);
            text = new MenuText(relativePos, ratingText);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.25f);
            RenderableText rating = new RenderableText();
            rating.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.GetRating(0));
            text = new MenuText(relativePos, rating, true);
            levelCompleteMenu.AddItem(text);

            return levelCompleteMenu;
        }
        
        /// <summary>
        /// Creates a level complete menu.
        /// </summary>
        /// <param name="contentManager">The content manager to use to load resources.</param>
        /// <param name="spriteBatch">The sprite batch to attach to menu items.</param>
        /// <param name="position">The position of the menu.</param>
        /// <returns>The new menu created.</returns>
        public static Menu CreateLeaderboardMenu(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 position)
        {
            Menu leaderboardMenu = new Menu(position, 1, 1);
            MenuButton button = null;
            MenuText text = null;
            Sprite largeButtonTile = new Sprite();
            largeButtonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeButtonTile);
            Vector2 relativePos = Vector2.Zero;
            float tileOffset = 32.0f;

            relativePos = new Vector2((-largeButtonTile.Width - tileOffset) * 1.5f, (-largeButtonTile.Height - tileOffset) * 0.5f);
            Sprite largeBackIcon = new Sprite();
            largeBackIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeBackIcon);
            RenderableText backText = new RenderableText();
            backText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.BackButtonName.ToLowerInvariant());
            button = new MenuButton(relativePos, largeButtonTile, largeBackIcon, backText, MenuConstants.BackButtonName);
            leaderboardMenu.AddItem(button);
            
            relativePos = new Vector2((-largeButtonTile.Width) * 0.5f, (-largeButtonTile.Height - tileOffset) * 0.5f);
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    largeButtonTile = new Sprite();
                    largeButtonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeButtonTile);
                    MenuImage background = new MenuImage(relativePos, largeButtonTile);
                    leaderboardMenu.AddItem(background);
                    relativePos.X += largeButtonTile.Width;
                }

                relativePos.Y += largeButtonTile.Height;
                relativePos.X = (-largeButtonTile.Width) * 0.5f;
            }

            relativePos = new Vector2(0.0f, -largeButtonTile.Height * 0.825f);
            for (int count = 0; count < 10; count++)
            {
                RenderableText placeHolderText = new RenderableText();
                placeHolderText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, string.Empty);
                text = new MenuText(relativePos, placeHolderText, true);
                leaderboardMenu.AddItem(text);
                relativePos.Y += tileOffset * 1.5f;
            }

            return leaderboardMenu;
        }

        /// <summary>
        /// Creates a level select menu.
        /// </summary>
        /// <param name="contentManager">The content manager to use to load resources.</param>
        /// <param name="spriteBatch">The sprite batch to attach to menu items.</param>
        /// <param name="position">The position of the menu.</param>
        /// <param name="screenWidth">The width of the rendering window.</param>
        /// <param name="gameSettings">The game settings.</param>
        /// <returns>The new menu created.</returns>
        public static Menu CreateLevelSelectMenu(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 position, float screenWidth, GameSettings gameSettings)
        {
            int pages = (gameSettings.TotalLevels + 1) / 8;
            pages += (gameSettings.TotalLevels + 1) % 8 == 0 ? 0 : 1;
            Menu levelSelectMenu = new Menu(position, 2, 4, pages, screenWidth);
            Sprite largeButtonTile = new Sprite();
            largeButtonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeButtonTile);
            
            float gapBetweenTiles = 32.0f;
            Vector2 firstTileRelativePos = new Vector2((-largeButtonTile.Width * 1.5f) + (-gapBetweenTiles * 1.5f), (-largeButtonTile.Height - gapBetweenTiles) * 0.5f);
            Vector2 tileOffset = new Vector2(largeButtonTile.Width + gapBetweenTiles, largeButtonTile.Height + gapBetweenTiles);

            float screenCount = 0.0f;
            int total = 0;
            int tilesPerScreen = 8;
            int columns = 4;
            int rowOffset = 0;
            int colOffset = 0;

            while (total < gameSettings.TotalLevels + 1)
            {
                MenuButton button = null;
                Vector2 relativePos = new Vector2(screenCount * screenWidth, 0.0f) + firstTileRelativePos + new Vector2(tileOffset.X * colOffset, tileOffset.Y * rowOffset);
                if (total == 0)
                {
                    Sprite largeBackIcon = new Sprite();
                    largeBackIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeBackIcon);
                    RenderableText backText = new RenderableText();
                    backText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.BackButtonName.ToLowerInvariant());
                    button = new MenuButton(relativePos, largeButtonTile, largeBackIcon, backText, MenuConstants.BackButtonName);
                    levelSelectMenu.AddItem(button);
                }
                else
                {
                    RenderableText text = new RenderableText();
                    RenderableText largeText = new RenderableText();
                    string name = ConvertToWords.ConvertIntToWords(total);
                    text.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, name);
                    largeText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontLarge, total.ToString());
                    button = new MenuButton(relativePos, largeButtonTile, largeText, text, total.ToString());
                    levelSelectMenu.AddItem(button);
                }

                colOffset++;
                total++;
                if (total % tilesPerScreen == 0)
                {
                    rowOffset = 0;
                    colOffset = 0;
                    screenCount++;
                }
                else if (total % columns == 0)
                {
                    rowOffset++;
                    colOffset = 0;
                }
            }

            levelSelectMenu.SelectablesActive = gameSettings.LevelsUnlocked + 1;
            return levelSelectMenu;
        }
        
        /// <summary>
        /// Creates a name prompt menu.
        /// </summary>
        /// <param name="contentManager">The content manager to use to load resources.</param>
        /// <param name="spriteBatch">The sprite batch to attach to menu items.</param>
        /// <param name="position">The position of the menu.</param>
        /// <param name="playerName">The stored player's name.</param>
        /// <returns>The new menu created.</returns>
        public static Menu CreateNamePromptMenu(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 position, string playerName)
        {
            Menu namePromptMenu = new Menu(position, 2, 3);
            MenuButton button = null;

            Sprite buttonTile = new Sprite();
            RenderableText textIcon = new RenderableText();
            RenderableText text = new RenderableText();
            buttonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.MediumButtonTile);
            float tileGap = 16.0f;

            Vector2 relativePosition = new Vector2(-tileGap - buttonTile.Width, 0.0f);

            for (int count = 0; count < 3; count++)
            {
                buttonTile = new Sprite();
                buttonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.MediumButtonTile);
                textIcon = new RenderableText();
                textIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontLarge, playerName[count].ToString().ToUpperInvariant());
                text = new RenderableText();
                text.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, playerName[count].ToString().ToLowerInvariant());
                text.Colour = Color.Black;
                button = new MenuButton(relativePosition, buttonTile, textIcon, text, count.ToString());
                namePromptMenu.AddItem(button);
                relativePosition.X += tileGap + buttonTile.Width;
            }

            relativePosition = new Vector2(0.0f, buttonTile.Height + tileGap);
            Sprite largePlayIcon = new Sprite();
            largePlayIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.MediumPlayIcon);
            RenderableText continueText = new RenderableText();
            continueText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.DoneButtonName.ToLowerInvariant());
            button = new MenuButton(relativePosition, buttonTile, largePlayIcon, continueText, MenuConstants.DoneButtonName);
            namePromptMenu.AddItem(button);

            relativePosition = new Vector2(-buttonTile.Width - tileGap, -buttonTile.Height - tileGap);
            float offset = (buttonTile.Width + tileGap) / 2.0f;
            for (int col = 0; col < 5; col++)
            {
                buttonTile = new Sprite();
                buttonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.MediumButtonTile);
                MenuImage background = new MenuImage(relativePosition, buttonTile);
                namePromptMenu.AddItem(background);
                relativePosition.X += offset;
            }

            relativePosition = new Vector2(0.0f, -1.55f * (buttonTile.Height - tileGap));
            RenderableText hello = new RenderableText();
            hello.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, MenuConstants.HelloText);
            MenuText helloText = new MenuText(relativePosition, hello);
            namePromptMenu.AddItem(helloText);

            RenderableText enterName = new RenderableText();
            enterName.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.EnterNameText);
            relativePosition.Y += enterName.Height * 1.2f;
            MenuText enterNameText = new MenuText(relativePosition, enterName);
            namePromptMenu.AddItem(enterNameText);

            relativePosition.Y += enterName.Height * 1.2f;
            RenderableText selectLetter = new RenderableText();
            selectLetter.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.SelectLetterText);
            MenuText selectLetterText = new MenuText(relativePosition, selectLetter);
            namePromptMenu.AddItem(selectLetterText);

            relativePosition.Y += enterName.Height * 1.2f;
            RenderableText pressDone = new RenderableText();
            pressDone.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.PressDoneText);
            MenuText pressDoneText = new MenuText(relativePosition, pressDone);
            namePromptMenu.AddItem(pressDoneText);

            return namePromptMenu;
        }

        /// <summary>
        /// Creates a letter input menu.
        /// </summary>
        /// <param name="contentManager">The content manager to use to load resources.</param>
        /// <param name="spriteBatch">The sprite batch to attach to menu items.</param>
        /// <param name="position">The position of the menu.</param>
        /// <param name="firstLetter">The first letter to put in the menu. This should be MaxValue if creating the base character select menu.</param>
        /// <returns>The new menu created.</returns>
        public static Menu CreateLetterInputMenu(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 position, char firstLetter)
        {
            int rows = 2;
            int columns = 3;
            float tileGap = 32.0f;
            Menu nameInutMenu = new Menu(position, rows, columns);            
            Sprite buttonTile = new Sprite();
            RenderableText textIcon = new RenderableText();
            RenderableText text = new RenderableText();
            buttonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeButtonTile);
            Vector2 firstTileRelativePosition = new Vector2(-buttonTile.Width - tileGap, -0.5f * (buttonTile.Height + tileGap));
            Vector2 currentTileOffset = Vector2.Zero;
            char letter = firstLetter;
            string label = string.Empty;
            for (int rowCount = 0; rowCount < rows; rowCount++)
            {
                currentTileOffset.Y = rowCount * (buttonTile.Height + tileGap);
                for (int colCount = 0; colCount < columns; colCount++)
                {
                    if (firstLetter == char.MaxValue)
                    {
                        label = MenuFactory.GetCharacterSelectionLabel((rowCount * columns) + colCount);
                    }
                    else
                    {
                        if (letter == '{')
                        {
                            letter = '0';
                        }
                        
                        label = letter.ToString();
                    }

                    currentTileOffset.X = colCount * (buttonTile.Width + tileGap);
                    buttonTile = new Sprite();
                    buttonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeButtonTile);
                    textIcon = new RenderableText();
                    textIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontLarge, label.ToUpperInvariant()); 
                    text = new RenderableText();
                    text.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, label);
                    text.Colour = Color.Black;
                    MenuButton button = new MenuButton(firstTileRelativePosition + currentTileOffset, buttonTile, textIcon, text, label.ToUpperInvariant());
                    nameInutMenu.AddItem(button);
                    letter += (char)1;
                }
            }

            return nameInutMenu;
        }

        /// <summary>
        /// Creates a place holder for menu's which are yet to be implemented.
        /// </summary>
        /// <param name="contentManager">The content manager to use to load resources.</param>
        /// <param name="spriteBatch">The sprite batch to attach to menu items.</param>
        /// <param name="position">The position of the menu.</param>
        /// <returns>The new menu created.</returns>
        public static Menu CreatePlaceholderMenu(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 position)
        {
            Menu placeHolderMenu = new Menu(position, 1, 1);
            MenuButton button = null;
            Sprite largeButtonTile = new Sprite();
            largeButtonTile.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeButtonTile);
            Vector2 relativePos = Vector2.Zero;

            Sprite largeBackIcon = new Sprite();
            largeBackIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeBackIcon);
            RenderableText backText = new RenderableText();
            backText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.BackButtonName.ToLowerInvariant());
            button = new MenuButton(relativePos, largeButtonTile, largeBackIcon, backText, MenuConstants.BackButtonName);
            placeHolderMenu.AddItem(button);

            return placeHolderMenu;
        }

        /// <summary>
        /// Retrieves the label for a character selection menu button.
        /// </summary>
        /// <param name="buttonCount">The index of the button being created.</param>
        /// <returns>The label to place on the button.</returns>
        private static string GetCharacterSelectionLabel(int buttonCount)
        {
            string label = string.Empty;
            switch (buttonCount)
            {
                case 0:
                    label = MenuConstants.AToF;
                    break;
                case 1:
                    label = MenuConstants.GToL;
                    break;
                case 2:
                    label = MenuConstants.MToR;
                    break;
                case 3:
                    label = MenuConstants.SToX;
                    break;
                case 4:
                    label = MenuConstants.YToThree;
                    break;
                case 5:
                    label = MenuConstants.FourToNine;
                    break;
                default:
                    label = string.Empty;
                    break;
            }

            return label;
        }
    }
}
