// -----------------------------------------------------------------------
// <copyright file="ScrollingDeath.cs" company="None">
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

    /// <summary>
    /// Defines the scrolling entity which kills the player when it catches them.
    /// </summary>
    class ScrollingDeath
    {
        /// <summary>
        /// The entity's body.
        /// </summary>
        private Body physicsBody;

        /// <summary>
        /// The standard velocity, in display units, at which the entity should move.
        /// </summary>
        private Vector2 normalVelocity;

        /// <summary>
        /// The minimum velocity, in display units, at which the entity should move.
        /// </summary>
        private Vector2 minimumVelocity;

        /// <summary>
        /// The maximum velocity, in display units, at which the entity should move.
        /// </summary>
        private Vector2 maximumVelocity;

        /// <summary>
        /// A value indicating whether the entity is active or not.
        /// </summary>
        private bool active;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollingDeath"/> class.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        /// <param name="displayHeight">The height of the display area.</param>
        /// <param name="minimumScrollSpeed">The minimum rate at which the entity should scroll.</param>
        /// <param name="maximumScrollSpeed">The maximum rate at which the entity should scroll.</param>
        /// <param name="scrollSpeed">The ideal rate at which the entity should scroll.</param>
        public ScrollingDeath(ref World physicsWorld, float displayHeight, float minimumScrollSpeed, float maximumScrollSpeed, float scrollSpeed)
        {
            this.active = false;
            float width = 5.0f;
            this.physicsBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(width), ConvertUnits.ToSimUnits(displayHeight * 100.0f), 0.0f);
            this.physicsBody.BodyType = BodyType.Dynamic;
            this.physicsBody.Position = ConvertUnits.ToSimUnits(new Vector2(width, displayHeight * 0.5f));
            this.physicsBody.CollisionCategories = EntityConstants.ScrollingDeathCategory;
            this.physicsBody.CollidesWith = EntityConstants.StickManCategory;
            this.physicsBody.IgnoreGravity = true;
            this.physicsBody.IgnoreCCD = true;
            this.physicsBody.LinearVelocity = Vector2.Zero;
            this.normalVelocity = new Vector2(scrollSpeed, 0.0f);
            this.minimumVelocity = new Vector2(minimumScrollSpeed, 0.0f);
            this.maximumVelocity = new Vector2(maximumScrollSpeed, 0.0f);
        }

        /// <summary>
        /// Activates the scrolling entity.
        /// </summary>
        public void Activate()
        {
            this.active = true;
            this.physicsBody.LinearVelocity = ConvertUnits.ToSimUnits(this.normalVelocity);
        }

        /// <summary>
        /// Updates the camera based on the scrolling death entity.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (this.active)
            {
                Camera2D.Update(ConvertUnits.ToDisplayUnits(this.physicsBody.LinearVelocity), gameTime);
            }
        }

        /// <summary>
        /// Disposes of the entity's physics body.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        public void Dispose(ref World physicsWorld)
        {
            if (this.physicsBody != null)
            {
                this.active = false;
                physicsWorld.RemoveBody(this.physicsBody);
                this.physicsBody = null;
            }
        }
    }
}
