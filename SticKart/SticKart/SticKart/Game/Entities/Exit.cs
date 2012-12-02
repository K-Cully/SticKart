// -----------------------------------------------------------------------
// <copyright file="Exit.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Entities
{
    using Display;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a level exit entity.
    /// </summary>
    public class Exit
    {
        /// <summary>
        /// The entity's physics body.
        /// </summary>
        private Body physicsBody;

        /// <summary>
        /// The entity's sprite.
        /// </summary>
        private Sprite sprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="Exit"/> class.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to render sprites with.</param>
        /// <param name="contentManager">The content manager to load assets with.</param>
        /// <param name="physicsWorld">The game's physics world.</param>
        /// <param name="position">The position of the exit.</param>
        public Exit(SpriteBatch spriteBatch, ContentManager contentManager, ref World physicsWorld, Vector2 position)
        {
            this.sprite = new Sprite();
            this.Triggered = false;
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.SetUpPhysics(ref physicsWorld, position);
        }

        /// <summary>
        /// Gets a value indicating whether the exit has been triggered or not.
        /// </summary>
        public bool Triggered { get; private set; }

        /// <summary>
        /// Updates an exit entity.
        /// </summary>
        public void Update()
        {
            if (!this.Triggered)
            {
                if ((bool)this.physicsBody.UserData == true)
                {
                    this.Triggered = true;
                }
            }
        }

        /// <summary>
        /// Draws an exit entity.
        /// </summary>
        public void Draw()
        {
            Camera2D.Draw(this.sprite, ConvertUnits.ToDisplayUnits(this.physicsBody.Position), 0.0f);
        }

        /// <summary>
        /// Destroys the physics objects associated with an <see cref="Exit"/> object.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        public void Dispose(ref World physicsWorld)
        {
            if (physicsWorld != null)
            {
                physicsWorld.RemoveBody(this.physicsBody);
            }
        }

        /// <summary>
        /// Initializes and loads the textures for all of the sprites in an <see cref="Exit"/> object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.sprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Exit);
        }

        /// <summary>
        /// Sets up the physics bodies and joints used by the entity.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        /// <param name="position">The position of the switch in display coordinates</param>
        private void SetUpPhysics(ref World physicsWorld, Vector2 position)
        {
            float density = 1.1f;
            float restitution = 0.0f;
            this.physicsBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(this.sprite.Width * 0.2f), ConvertUnits.ToSimUnits(this.sprite.Height), density, ConvertUnits.ToSimUnits(position));
            this.physicsBody.BodyType = BodyType.Static;
            this.physicsBody.Restitution = restitution;
            this.physicsBody.CollisionCategories = EntityConstants.ExitCategory;
            this.physicsBody.CollidesWith = EntityConstants.StickManCategory;
            this.physicsBody.UserData = this.Triggered;
        }
    }
}
