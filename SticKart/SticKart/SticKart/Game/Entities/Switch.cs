// -----------------------------------------------------------------------
// <copyright file="Switch.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Entities
{
    using Display;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;
    using FarseerPhysics.SamplesFramework;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    

    /// <summary>
    /// Defines a switch which activates a mine cart.
    /// </summary>
    public class Switch
    {
        /// <summary>
        /// The physics body of a switch.
        /// </summary>
        private Body physicsBody;

        /// <summary>
        /// The sprite to render for a switch.
        /// </summary>
        private Sprite sprite;

        /// <summary>
        /// The mine cart that a switch should activate.
        /// </summary>
        private MineCart mineCart;

        /// <summary>
        /// A value indicating whether the switch has been activated or not.
        /// </summary>
        private bool activated;

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to render sprites with.</param>
        /// <param name="contentManager">The content manager to load assets with.</param>
        /// <param name="physicsWorld">The game's physics world.</param>
        /// <param name="position">The position of the switch.</param>
        /// <param name="mineCart">The mine cart to activate.</param>
        public Switch(SpriteBatch spriteBatch, ContentManager contentManager, ref World physicsWorld, Vector2 position, MineCart mineCart)
        {
            this.sprite = new Sprite();
            this.activated = false;
            this.mineCart = mineCart;
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.SetUpPhysics(ref physicsWorld, position);
        }

        /// <summary>
        /// Updates a switch enity.
        /// </summary>
        public void Update()
        {
            if (!this.activated)
            {
                if ((bool)this.physicsBody.UserData == true)
                {
                    this.activated = true;
                    this.mineCart.Activate();
                }
            }
        }

        /// <summary>
        /// Draws a switch entity.
        /// </summary>
        public void Draw()
        {
            Camera2D.Draw(this.sprite, ConvertUnits.ToDisplayUnits(this.physicsBody.Position), 0.0f);
        }

        /// <summary>
        /// Destroys the physics objects associated with a <see cref="Switch"/> instance.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        public void Dispose(ref World physicsWorld)
        {
            this.activated = false;
            if (physicsWorld != null)
            {
                physicsWorld.RemoveBody(this.physicsBody);
            }
        }

        /// <summary>
        /// Initializes and loads the textures for all of the sprites in a <see cref="Switch"/> object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.sprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Switch);
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
            this.physicsBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(this.sprite.Width), ConvertUnits.ToSimUnits(this.sprite.Height), density, ConvertUnits.ToSimUnits(position));
            this.physicsBody.BodyType = BodyType.Static;
            this.physicsBody.Restitution = restitution;
            this.physicsBody.CollisionCategories = EntityConstants.SwitchCategory;
            this.physicsBody.CollidesWith = EntityConstants.StickManCategory;
            this.physicsBody.UserData = this.activated;
        }
    }
}
