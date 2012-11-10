namespace SticKart.Game.Level
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        // TODO: remove/change to scrolling death once floor is implemented.
        private Body boundry;

        /// <summary>
        /// The resolution of the game display area.
        /// </summary>
        private Vector2 gameDisplayResolution;

        /// <summary>
        /// The set frame-time of the game.
        /// </summary>
        private float frameTime;

        /// <summary>
        /// The physics world used by the level.
        /// </summary>
        private World physicsWorld;

        /// <summary>
        /// The list of floor edges.
        /// </summary>
        private List<Body> floorEdges;

        /// <summary>
        /// The sprite batch to use when drawing.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The content manager to use for loading content.
        /// </summary>
        private ContentManager contentManager;

        /// <summary>
        /// The level loader.
        /// </summary>
        private LevelLoader levelLoader;

        /// <summary>
        /// The current level number.
        /// </summary>
        private int currentLevel;

        /// <summary>
        /// Whether the current level is a custom level or not.
        /// </summary>
        private bool currentLevelCustom;

        /// <summary>
        /// The player's in game representation.
        /// </summary>
        private StickMan stickman;

        // TODO: Remove this boundry or scroll.
        protected Vertices GetBounds()
        {
            float height = ConvertUnits.ToSimUnits(this.gameDisplayResolution.Y);
            float width = ConvertUnits.ToSimUnits(this.gameDisplayResolution.X * 2.0f);

            Vertices bounds = new Vertices(4);
            bounds.Add(new Vector2(0.0f, 0.0f));
            bounds.Add(new Vector2(width, 0.0f));
            bounds.Add(new Vector2(width, height));
            bounds.Add(new Vector2(0.0f, height));
            return bounds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelManager"/> class.
        /// </summary>
        /// <param name="gameDisplayResolution">The resolution the game is set to render at.</param>
        /// <param name="frameTime">The frame time set for the game.</param>
        public LevelManager(Vector2 gameDisplayResolution, float frameTime)
        {
            this.physicsWorld = null;
            this.floorEdges = new List<Body>();
            this.gameDisplayResolution = gameDisplayResolution;
            this.frameTime = frameTime;
            this.physicsWorld = null;
            this.spriteBatch = null;
            this.contentManager = null;
            this.levelLoader = null;
            this.stickman = null;
        }

        /// <summary>
        /// Loads the content used by entities in a level.
        /// </summary>
        /// <param name="contentManager">The content manager to load content with.</param>
        /// <param name="spriteBatch">The sprite batch to render using.</param>
        public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            this.physicsWorld = new World(ConvertUnits.ToSimUnits(new Vector2(0.0f, 348.8f)));

            this.boundry = BodyFactory.CreateLoopShape(this.physicsWorld, this.GetBounds());
            this.boundry.Friction = float.MaxValue;
            this.boundry.CollisionCategories = Category.All;
            this.boundry.CollidesWith = Category.All;

            this.contentManager = contentManager;
            this.spriteBatch = spriteBatch;

            this.levelLoader = new LevelLoader(this.contentManager);
        
            this.stickman = new StickMan(ref this.physicsWorld, 10.0f, 100, -1.0f, this.spriteBatch, this.contentManager);
        }

        /// <summary>
        /// Loads and starts the level specified.
        /// </summary>
        /// <param name="levelNumber">The level to start.</param>
        /// <param name="isCustom">Whether the level is a custom level or not.</param>
        public void BeginLevel(int levelNumber, bool isCustom)
        {
            this.currentLevel = levelNumber > 0 ? levelNumber : 1;
            this.currentLevelCustom = isCustom;

            // TODO: Implement logic to allow for custom levels.

            this.physicsWorld.ClearForces();
            this.levelLoader.LoadLevel(this.currentLevel, this.currentLevelCustom);

            this.CreateLevelFloor(this.levelLoader.FloorPoints);
            this.stickman.Reset(this.levelLoader.StartPosition);
        }

        /// <summary>
        /// Updates all the entities in the current level.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="commands">A list of input commands.</param>
        public void Update(GameTime gameTime, List<InputCommand> commands)
        {
            // TODO
            this.stickman.Update(gameTime);

            foreach (InputCommand command in commands)
            {
                switch (command)
                {
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

        /// <summary>
        /// Draws the level and any entities it contains.
        /// </summary>
        public void Draw()
        {
            // TODO
            this.stickman.Draw();
        }

        /// <summary>
        /// Creates the floor of a level.
        /// </summary>
        /// <param name="points">The points which define the floor.</param>
        private void CreateLevelFloor(List<Vector2> points)
        {
            foreach (Body body in this.floorEdges)
            {
                this.physicsWorld.RemoveBody(body);
            }

            this.floorEdges.Clear();            
            if (points.Count > 0)
            {
                Vector2 startPoint = points[0];
                foreach (Vector2 point in points)
                {
                    if (point != startPoint)
                    {
                        this.floorEdges.Add(BodyFactory.CreateEdge(this.physicsWorld, ConvertUnits.ToSimUnits(startPoint), ConvertUnits.ToSimUnits(point)));
                        startPoint = point;
                    }
                }

                // Add a wall at the end of the level.
                this.floorEdges.Add(BodyFactory.CreateEdge(this.physicsWorld, ConvertUnits.ToSimUnits(startPoint), ConvertUnits.ToSimUnits(new Vector2(startPoint.X, -this.gameDisplayResolution.Y)))); 
            }
        }
    }
}
