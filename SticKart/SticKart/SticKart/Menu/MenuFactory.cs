namespace SticKart.Menu
{
    using Display;
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
            playGameText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, SelectableNames.PlayButtonName.ToLowerInvariant());
            relativePos = new Vector2((-largeButtonTile.Width - gapBetweenTiles) * 0.5f, (-largeButtonTile.Height - gapBetweenTiles) * 0.5f);
            button = new MenuButton(relativePos, largeButtonTile, largePlayIcon, playGameText, SelectableNames.PlayButtonName);
            mainMenu.AddItem(button);

            Sprite largeOptionsIcon = new Sprite();
            largeOptionsIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeOptionsIcon);
            RenderableText optionsText = new RenderableText();
            optionsText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, SelectableNames.OptionsButtonName.ToLowerInvariant());
            relativePos = new Vector2((largeButtonTile.Width + gapBetweenTiles) * 0.5f, (-largeButtonTile.Height - gapBetweenTiles) * 0.5f);
            button = new MenuButton(relativePos, largeButtonTile, largeOptionsIcon, optionsText, SelectableNames.OptionsButtonName);
            mainMenu.AddItem(button);
            
            Sprite largeLeaderboardIcon = new Sprite();
            largeLeaderboardIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeLeaderboardIcon);
            RenderableText leaderboardText = new RenderableText();
            leaderboardText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, SelectableNames.LeaderboardButtonName.ToLowerInvariant());
            relativePos = new Vector2((-largeButtonTile.Width - gapBetweenTiles) * 0.5f, (largeButtonTile.Height + gapBetweenTiles) * 0.5f);
            button = new MenuButton(relativePos, largeButtonTile, largeLeaderboardIcon, leaderboardText, SelectableNames.LeaderboardButtonName);
            mainMenu.AddItem(button);

            Sprite largeExitIcon = new Sprite();
            largeExitIcon.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.LargeExitIcon);
            RenderableText exitText = new RenderableText();
            exitText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, SelectableNames.ExitButtonName.ToLowerInvariant());
            relativePos = new Vector2((largeButtonTile.Width + gapBetweenTiles) * 0.5f, (largeButtonTile.Height + gapBetweenTiles) * 0.5f);
            button = new MenuButton(relativePos, largeButtonTile, largeExitIcon, exitText, SelectableNames.ExitButtonName);
            mainMenu.AddItem(button);

            return mainMenu;
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
            backText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFont, SelectableNames.BackButtonName.ToLowerInvariant());
            button = new MenuButton(relativePos, largeButtonTile, largeBackIcon, backText, SelectableNames.BackButtonName);
            placeHolderMenu.AddItem(button);

            return placeHolderMenu;
        }
    }
}
