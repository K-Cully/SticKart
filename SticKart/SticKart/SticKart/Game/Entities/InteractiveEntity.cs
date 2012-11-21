// -----------------------------------------------------------------------
// <copyright file="InteractiveEntity.cs" company="None">
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
    /// Defines the abstract base class for an interactive level entity.
    /// </summary>
    public abstract class InteractiveEntity
    {
        /// <summary>
        /// The interactive entity's physics body.
        /// </summary>
        protected Body physicsBody;

        /// <summary>
        /// The sprite to render.
        /// </summary>
        protected Sprite sprite; 

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveEntity"/> class.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        /// <param name="description">The description of the entity.</param>
        public InteractiveEntity(ref World physicsWorld, SpriteBatch spriteBatch, ContentManager contentManager, InteractiveEntityDescription description)
        {
            this.sprite = new Sprite();
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.SetUpPhysics(ref physicsWorld, description);
        }

        /// <summary>
        /// Destroys the body associated with the interactive entity.
        /// </summary>
        /// <param name="physicsWorld">The physics world containing the body.</param>
        public virtual void Dispose(ref World physicsWorld)
        {
            physicsWorld.RemoveBody(this.physicsBody);
        }

        /// <summary>
        /// Draws the interactive entity.
        /// </summary>
        public virtual void Draw()
        {
            Camera2D.Draw(this.sprite, ConvertUnits.ToDisplayUnits(this.physicsBody.Position), this.physicsBody.Rotation);
        }

        /// <summary>
        /// Initializes and loads the textures for the sprites in an InteractiveEntity object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        protected abstract void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager);

        /// <summary>
        /// Sets up the physics body for an interactive entity.
        /// </summary>
        /// <param name="physicsWorld">The physics world to create the body in.</param>
        /// <param name="description">The description of the interactive entity.</param>
        protected virtual void SetUpPhysics(ref World physicsWorld, InteractiveEntityDescription description)
        {
            this.physicsBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(description.Dimensions.X), ConvertUnits.ToSimUnits(description.Dimensions.Y), 3.0f, ConvertUnits.ToSimUnits(description.Position));
            this.physicsBody.CollisionCategories = EntityConstants.InteractiveEntityCategory;
        }
    }
}
