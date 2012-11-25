// -----------------------------------------------------------------------
// <copyright file="HeadsUpDisplay.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display
{
    using Game.Entities;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a heads up display for the game.
    /// </summary>
    public class HeadsUpDisplay
    {
        /// <summary>
        /// The word to signify a value indicates health.
        /// </summary>
        private const string HealthWord = "Health";

        /// <summary>
        /// The word to signify a value indicates score.
        /// </summary>
        private const string ScoreWord = "Score";

        #region positions

        /// <summary>
        /// The size of the display area.
        /// </summary>
        private Vector2 displaySize;

        /// <summary>
        /// The position to display power up icons at.
        /// </summary>
        private Vector2 powerUpPosition;

        /// <summary>
        /// The position to display the score word at.
        /// </summary>
        private Vector2 scoreTextPosition;

        /// <summary>
        /// The position to display the health word at.
        /// </summary>
        private Vector2 healthTextPosition;

        /// <summary>
        /// The position to display the score at.
        /// </summary>
        private Vector2 scorePosition;

        /// <summary>
        /// The position to display the health at.
        /// </summary>
        private Vector2 healthPosition;

        #endregion

        #region sprites

        /// <summary>
        /// A sprite of a box to display power up icons in.
        /// </summary>
        private Sprite powerUpBox;

        /// <summary>
        /// A sprite to represent the invincibility power up.
        /// </summary>
        private Sprite invincible;

        /// <summary>
        /// A sprite to represent the health power up.
        /// </summary>
        private Sprite healthPowerUp;

        /// <summary>
        /// A sprite to represent the jump power up.
        /// </summary>
        private Sprite jump;

        /// <summary>
        /// A sprite to represent the speed power up.
        /// </summary>
        private Sprite speed;

        #endregion

        #region text

        /// <summary>
        /// The text indicating that the value beside it is the score.
        /// </summary>
        private RenderableText scoreText;

        /// <summary>
        /// The player's score.
        /// </summary>
        private RenderableText score;

        /// <summary>
        /// The text indicating that the value beside it is the health.
        /// </summary>
        private RenderableText healthText;

        /// <summary>
        /// The player's health.
        /// </summary>
        private RenderableText health;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadsUpDisplay"/> class.
        /// </summary>
        /// <param name="displaySize">The size of the display area.</param>
        public HeadsUpDisplay(Vector2 displaySize)
        {
            this.displaySize = displaySize;
            this.powerUpBox = new Sprite();
            this.invincible = new Sprite();
            this.healthPowerUp = new Sprite();
            this.jump = new Sprite();
            this.speed = new Sprite();
            this.scoreText = new RenderableText();
            this.score = new RenderableText();
            this.healthText = new RenderableText();
            this.health = new RenderableText();
            this.ActivePowerUp = PowerUpType.None;
            this.Score = 0;
            this.HealthPercentage = 100.0f;
            this.powerUpPosition = new Vector2(this.displaySize.X * 0.5f, this.displaySize.Y * 0.1f);
            this.scoreTextPosition = new Vector2(this.displaySize.X * 0.8f, this.displaySize.Y * 0.075f);
            this.scorePosition = new Vector2(this.displaySize.X * 0.9f, this.displaySize.Y * 0.075f);
            this.healthTextPosition = new Vector2(this.displaySize.X * 0.1f, this.displaySize.Y * 0.075f);
            this.healthPosition = new Vector2(this.displaySize.X * 0.2f, this.displaySize.Y * 0.075f);
        }

        #region public_accessors

        /// <summary>
        /// Gets or sets the currently active power up to display.
        /// </summary>
        public PowerUpType ActivePowerUp { get; set; }

        /// <summary>
        /// Gets or sets the current score to display.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the current health to display.
        /// </summary>
        public float HealthPercentage { get; set; }

        #endregion

        #region initialization

        /// <summary>
        /// Initializes the heads up display and loads its content.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use in drawing items.</param>
        /// <param name="contentManager">The content manager to load content with.</param>
        public void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.powerUpBox.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.HudPath + ContentLocations.PowerUpBox);
            this.invincible.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.HudPath + ContentLocations.InvincibleIcon);
            this.healthPowerUp.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.HudPath + ContentLocations.HealthIcon);
            this.jump.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.HudPath + ContentLocations.JumpIcon);
            this.speed.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.HudPath + ContentLocations.SpeedIcon);
            this.healthText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, HeadsUpDisplay.HealthWord);
            this.scoreText.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, HeadsUpDisplay.ScoreWord);
            this.health.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, this.HealthPercentage.ToString("P"));
            this.score.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.SegoeUIFontMedium, this.Score.ToString("D8"));
        }

        /// <summary>
        /// Resets the heads up display values.
        /// </summary>
        public void Reset()
        {
            this.ActivePowerUp = PowerUpType.None;
            this.Score = 0;
            this.HealthPercentage = 100.0f;
        }

        #endregion

        /// <summary>
        /// Draws the heads up display to the screen.
        /// </summary>
        public void Draw()
        {
            Sprite.Draw(this.powerUpBox, this.powerUpPosition, 0.0f);
            switch (this.ActivePowerUp)
            {
                case PowerUpType.Invincibility:
                    Sprite.Draw(this.invincible, this.powerUpPosition, 0.0f);                    
                    break;
                case PowerUpType.Health:
                    Sprite.Draw(this.healthPowerUp, this.powerUpPosition, 0.0f);
                    break;
                case PowerUpType.Jump:
                    Sprite.Draw(this.jump, this.powerUpPosition, 0.0f);
                    break;
                case PowerUpType.Speed:
                    Sprite.Draw(this.speed, this.powerUpPosition, 0.0f);
                    break;
                default:
                    break;
            }

            this.score.SetText(this.Score.ToString("D8"));
            this.health.SetText(this.HealthPercentage.ToString("N0") + "%");
            RenderableText.Draw(this.scoreText, this.scoreTextPosition, 0.0f, Color.Black);
            RenderableText.Draw(this.score, this.scorePosition, 0.0f, Color.Black);
            RenderableText.Draw(this.healthText, this.healthTextPosition, 0.0f, Color.Black);
            RenderableText.Draw(this.health, this.healthPosition, 0.0f, Color.Black);
        }
    }
}
