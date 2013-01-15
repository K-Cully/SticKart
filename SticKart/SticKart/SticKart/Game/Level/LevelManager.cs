// -----------------------------------------------------------------------
// <copyright file="LevelManager.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Level
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Audio;
    using Display;
    using Entities;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Input;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a level and all elements contained within.
    /// </summary>
    public class LevelManager
    {
        #region level_settings

        /// <summary>
        /// Whether the current level is a custom level or not.
        /// </summary>
        private bool currentLevelCustom;

        /// <summary>
        /// The physics world used by the level.
        /// </summary>
        private World physicsWorld;

        /// <summary>
        /// A timer to control the activation of the scrolling death entity.
        /// </summary>
        private float scrollStartTimer;

        #endregion

        #region game_settings

        /// <summary>
        /// The resolution of the game display area.
        /// </summary>
        private Vector2 gameDisplayResolution;

        /// <summary>
        /// The set frame-time of the game.
        /// </summary>
        private float frameTime;

        /// <summary>
        /// The sprite batch to use when drawing.
        /// </summary>
        private SpriteBatch spriteBatch;

        #endregion

        #region content_managment

        /// <summary>
        /// The floor sprite.
        /// </summary>
        private Sprite floorSprite;

        /// <summary>
        /// The content manager to use for loading content.
        /// </summary>
        private ContentManager contentManager;

        /// <summary>
        /// The level loader.
        /// </summary>
        private LevelLoader levelLoader;

        #endregion

        #region entities

        /// <summary>
        /// The list of floor edges.
        /// </summary>
        private List<Body> floorEdges;

        /// <summary>
        /// The list of visual floor edges.
        /// </summary>
        private List<VisualEdge> visualFloorEdges;

        /// <summary>
        /// The list of platforms.
        /// </summary>
        private List<Platform> platforms;

        /// <summary>
        /// the list of interactive entities.
        /// </summary>
        private List<InteractiveEntity> interactiveEntities;
        
        /// <summary>
        /// The player's in game representation.
        /// </summary>
        private StickMan stickman;

        /// <summary>
        /// The exit entity.
        /// </summary>
        private Exit exit;

        /// <summary>
        /// The scrolling death entity.
        /// </summary>
        private ScrollingDeath scrollingDeath;

        /// <summary>
        /// The mine cart entity.
        /// </summary>
        private MineCart mineCart;

        /// <summary>
        /// The switch entity.
        /// </summary>
        private Switch cartSwitch;

        #endregion

        #region graphics

        /// <summary>
        /// The text to instruct the player to start.
        /// </summary>
        private RenderableText goText;

        /// <summary>
        /// A texture to apply to rocky terrain.
        /// </summary>
        private Texture2D rockyTerrain;

        /// <summary>
        /// The background to display in a level.
        /// </summary>
        private Background background;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelManager"/> class.
        /// </summary>
        /// <param name="gameDisplayResolution">The resolution the game is set to render at.</param>
        /// <param name="frameTime">The frame time set for the game.</param>
        public LevelManager(Vector2 gameDisplayResolution, float frameTime)
        {
            this.goText = new RenderableText();
            this.Complete = false;
            this.physicsWorld = null;
            this.gameDisplayResolution = gameDisplayResolution;
            this.frameTime = frameTime;
            this.physicsWorld = null;
            this.spriteBatch = null;
            this.floorSprite = new Sprite();
            this.contentManager = null;
            this.levelLoader = null;
            this.floorEdges = new List<Body>();
            this.visualFloorEdges = new List<VisualEdge>();
            this.platforms = new List<Platform>();
            this.interactiveEntities = new List<InteractiveEntity>();
            this.stickman = null;
            this.exit = null;
            this.scrollingDeath = null;
            this.mineCart = null;
            this.cartSwitch = null;
            this.scrollStartTimer = 0.0f;
            this.background = new Background(this.gameDisplayResolution, 0.8f);
        }

        #region public_accessors

        /// <summary>
        /// Gets the current player's score.
        /// </summary>
        public int PlayerScore 
        { 
            get
            {
                return this.stickman.Score;
            }
        }

        /// <summary>
        /// Gets the player's remaining health percentage.
        /// </summary>
        public float PlayerHealthPercentage
        {
            get
            {
                return this.stickman.PercentHealth;
            }
        }

        /// <summary>
        /// Gets the player's active power up.
        /// </summary>
        public PowerUpType PlayerPowerUp
        {
            get
            {
                return this.stickman.ActivePowerUp;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the level is complete or not.
        /// </summary>
        public bool Complete { get; private set; }

        /// <summary>
        /// Gets The current level number.
        /// </summary>
        public int CurrentLevel { get; private set; }

        #endregion

        /// <summary>
        /// Loads the content used by entities in a level.
        /// </summary>
        /// <param name="contentManager">The content manager to load content with.</param>
        /// <param name="spriteBatch">The sprite batch to render using.</param>
        public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            this.physicsWorld = new World(ConvertUnits.ToSimUnits(new Vector2(0.0f, 348.8f)));
            this.contentManager = contentManager;
            this.spriteBatch = spriteBatch;
            this.goText.InitializeAndLoad(spriteBatch, this.contentManager, ContentLocations.SegoeUIFontLarge, EntityConstants.GoText);
            this.InitializeAndLoadSprites(this.spriteBatch, this.contentManager);
            this.levelLoader = new LevelLoader(this.contentManager);        
            this.stickman = new StickMan(ref this.physicsWorld, 10.0f, 100, -1.0f, this.spriteBatch, this.contentManager);
            this.exit = new Exit(spriteBatch, contentManager, ref this.physicsWorld, this.levelLoader.EndPosition); 
            this.scrollingDeath = new ScrollingDeath(ref this.physicsWorld, this.gameDisplayResolution.Y, LevelConstants.MinimumScrollRate, LevelConstants.MaximumScrollRate, LevelConstants.ScrollRate, LevelConstants.ScrollAcceleration, LevelConstants.ScrollDeceleration, this.contentManager);
            this.rockyTerrain = contentManager.Load<Texture2D>(ContentLocations.Scenery + ContentLocations.RockyTerrain);
            this.background.InitializeAndLoad(this.spriteBatch, this.contentManager, ContentLocations.RockyBackGround);
        }

        /// <summary>
        /// Loads and starts the level specified.
        /// </summary>
        /// <param name="levelNumber">The level to start.</param>
        /// <param name="isCustom">Whether the level is a custom level or not.</param>
        public void BeginLevel(int levelNumber, bool isCustom)
        {
            this.Complete = false;
            this.CurrentLevel = levelNumber > 0 ? levelNumber : 1;
            this.currentLevelCustom = isCustom;
            this.scrollingDeath = new ScrollingDeath(ref this.physicsWorld, this.gameDisplayResolution.Y, LevelConstants.MinimumScrollRate, LevelConstants.MaximumScrollRate, LevelConstants.ScrollRate, LevelConstants.ScrollAcceleration, LevelConstants.ScrollDeceleration, this.contentManager);
            this.scrollStartTimer = 0.0f;
            
            // TODO: Implement logic to allow for custom levels.
            this.physicsWorld.ClearForces();
            this.levelLoader.LoadLevel(this.CurrentLevel, this.currentLevelCustom);

            LevelFactory.CreateFloor(this.levelLoader.FloorPoints, ref this.physicsWorld, ref this.floorEdges, ref this.visualFloorEdges, this.gameDisplayResolution.Y);
            LevelFactory.CreatePlatforms(this.levelLoader.PlatformDescriptions, ref this.physicsWorld, ref this.platforms, this.spriteBatch, this.contentManager);
            LevelFactory.CreateInteractiveEntities(this.levelLoader.InteractiveDescriptions, ref this.physicsWorld, ref this.interactiveEntities, ref this.mineCart, ref this.cartSwitch, this.spriteBatch, this.contentManager);
            this.exit = new Exit(this.spriteBatch, this.contentManager, ref this.physicsWorld, this.levelLoader.EndPosition); 
            this.stickman.Reset(this.levelLoader.StartPosition);
            float startX = this.visualFloorEdges[this.visualFloorEdges.Count - 1].EndPoint.X;
            float y = this.visualFloorEdges[this.visualFloorEdges.Count - 1].EndPoint.Y - this.gameDisplayResolution.Y;
            this.visualFloorEdges.Add(new VisualEdge(new Vector2(startX, y), new Vector2(startX + this.gameDisplayResolution.X, y)));
            this.background.Reset();
            AudioManager.PlayBackgroundMusic(true);
        }

        /// <summary>
        /// Cleans up after a level.
        /// </summary>
        public void EndLevel()
        {
            LevelFactory.DisposeOfPlatforms(ref this.physicsWorld, ref this.platforms);
            LevelFactory.DisposeOfInteractiveEntities(ref this.physicsWorld, ref this.interactiveEntities, ref this.mineCart, ref this.cartSwitch);
            LevelFactory.DisposeOfFloor(ref this.physicsWorld, ref this.floorEdges, ref this.visualFloorEdges);
            this.scrollingDeath.Dispose(ref this.physicsWorld);
            this.exit.Dispose(ref this.physicsWorld);
            AudioManager.StopBackgroundMusic();
        }
   
        /// <summary>
        /// Updates all the entities in the current level.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="commands">A list of input commands.</param>
        public void Update(GameTime gameTime, List<InputCommand> commands)
        {
            if (this.stickman.IsDead)
            {
                Camera2D.Reset();
                this.background.Reset();
                this.EndLevel();
                this.BeginLevel(this.CurrentLevel, this.currentLevelCustom);
            }
            else if (this.exit.Triggered)
            {
                this.Complete = true;
            }
            else
            {
                this.stickman.Update(gameTime);
                this.exit.Update();
                if (this.mineCart != null)
                {
                    this.cartSwitch.Update(gameTime);
                    this.mineCart.Update(gameTime, this.stickman.PhysicsPosition, this.stickman.HorizontalSpeed, this.stickman.InCart);
                }

                this.UpdateScrollingDeath(gameTime);
                Camera2D.Y = this.stickman.Position.Y - (this.gameDisplayResolution.Y * 0.5f);
                this.background.Update();

                foreach (InputCommand command in commands)
                {
                    switch (command)
                    {
                        case InputCommand.Stand:
                            this.stickman.Stand();
                            break;
                        case InputCommand.Jump:
                            this.stickman.Jump();
                            break;
                        case InputCommand.Crouch:
                            this.stickman.CrouchOrJumpDown();
                            break;
                        case InputCommand.Run:
                            this.stickman.Run();
                            break;
                        default:
                            break;
                    }
                }

                this.physicsWorld.Step(MathHelper.Min((float)gameTime.ElapsedGameTime.TotalSeconds, this.frameTime));
            }
        }

        /// <summary>
        /// Draws the level and any entities it contains.
        /// </summary>
        public void Draw()
        {
            this.background.Draw();

            foreach (Platform platform in this.platforms)
            {
                platform.Draw();
            }

            SceneryRenderer.DrawTerrain(this.spriteBatch, this.rockyTerrain, this.visualFloorEdges, this.gameDisplayResolution.Y * 1.5f);
            foreach (InteractiveEntity entity in this.interactiveEntities)
            {
                entity.Draw();
            }

            this.exit.Draw();
            this.stickman.Draw();
            this.mineCart.Draw();
            this.cartSwitch.Draw();
            if (!this.scrollingDeath.Active)
            {
                RenderableText.Draw(this.goText, this.gameDisplayResolution / 2.0f, 0.0f, Color.Black);
            }
        }

        /// <summary>
        /// Initializes and loads the textures for the floor sprite.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.floorSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Floor);
        }

        /// <summary>
        /// Updates the scrolling death entity.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        private void UpdateScrollingDeath(GameTime gameTime)
        {
            if (this.scrollingDeath.Active)
            {
                this.scrollingDeath.Update(gameTime);
                float distanceAheadOfCamera = this.stickman.Position.X - Camera2D.OffsetPosition.X;
                if (distanceAheadOfCamera < this.gameDisplayResolution.X / 4.0f)
                {
                    this.scrollingDeath.GoToSlowScrollRate();
                }
                else if (distanceAheadOfCamera > this.gameDisplayResolution.X / 1.875f)
                {
                    this.scrollingDeath.GoToFastScrollRate();
                }
                else
                {
                    this.scrollingDeath.GoToNormalScrollRate();
                }
            }
            else
            {
                if (this.stickman.Position.X > this.gameDisplayResolution.X / 3.0f)
                {
                    this.scrollStartTimer = LevelConstants.ScrollStartDelay + (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    this.scrollStartTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (this.scrollStartTimer > LevelConstants.ScrollStartDelay)
                {
                    this.scrollingDeath.Activate();
                }
            }  
        }
    }
}
