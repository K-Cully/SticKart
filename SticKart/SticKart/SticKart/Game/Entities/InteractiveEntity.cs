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

        public InteractiveEntity(ref World physicsWorld, SpriteBatch spriteBatch, ContentManager contentManager, InteractiveEntityDescription description)
        {
            this.sprite = new Sprite();
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.SetUpPhysics(ref physicsWorld);
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
        protected virtual void SetUpPhysics(ref World physicsWorld)
        {
            this.physicsBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(this.sprite.Width), ConvertUnits.ToSimUnits(this.sprite.Height), 3.0f);
            this.physicsBody.CollisionCategories = EntityConstants.InteractiveEntityCategory;
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
            Sprite.Draw(this.sprite, ConvertUnits.ToDisplayUnits(this.physicsBody.Position), this.physicsBody.Rotation);
        }
    }
}
