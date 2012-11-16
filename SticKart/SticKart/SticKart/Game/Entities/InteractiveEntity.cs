namespace SticKart.Game.Entities
{
    using Display;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines the abstract base class for an interactive level entity.
    /// </summary>
    public abstract class InteractiveEntity
    {
        // TODO: Implement.
        protected Body physicsBody;

        protected Sprite sprite; 

        public InteractiveEntity(ref World physicsWorld, SpriteBatch spriteBatch, ContentManager contentManager, InteractiveEntityDescription description)
        {
            this.sprite = new Sprite();
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.physicsBody.CollisionCategories = EntityConstants.InteractiveEntityCategory;
        }

        /// <summary>
        /// Initializes and loads the textures for the sprites in an InteractiveEntity object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            // TODO: finish this
            // this.sprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.);
        }

        /// <summary>
        /// Destroys the body associated with the interactive entity.
        /// </summary>
        /// <param name="physicsWorld">The physics world containing the body.</param>
        public virtual void Dispose(ref World physicsWorld)
        {
            // TODO: Implement
        }
    }
}
