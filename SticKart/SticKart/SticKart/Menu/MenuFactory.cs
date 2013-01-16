// -----------------------------------------------------------------------
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

            relativePos = new Vector2(0.0f, -largeButtonTile.Height * 1.1f);
            RenderableText levelComplete = new RenderableText();
            levelComplete.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, MenuConstants.LevelCompleteText);
            text = new MenuText(relativePos, levelComplete);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.25f);
            RenderableText highScoreText = new RenderableText();
            highScoreText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.HighScoreText);
            text = new MenuText(relativePos, highScoreText);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.5f);
            RenderableText scoreText = new RenderableText();
            scoreText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, MenuConstants.ScoreText);
            text = new MenuText(relativePos, scoreText);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.25f);
            RenderableText score = new RenderableText();
            score.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.ScoreDefaultText);
            text = new MenuText(relativePos, score);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.5f);
            RenderableText ratingText = new RenderableText();
            ratingText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, MenuConstants.RatingText);
            text = new MenuText(relativePos, ratingText);
            levelCompleteMenu.AddItem(text);

            relativePos += new Vector2(0.0f, levelComplete.Height * 1.25f);
            RenderableText rating = new RenderableText();
            rating.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, MenuConstants.GetRating(0));
            text = new MenuText(relativePos, rating);
            levelCompleteMenu.AddItem(text);

            return levelCompleteMenu;
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
    }
}
