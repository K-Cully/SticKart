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
    public class ScrollingDeath
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
        /// The ideal velocity, in display units, at which the entity should move.
        /// </summary>
        private Vector2 idealVelocity;

        /// <summary>
        /// The acceleration of the entity, in display units.
        /// </summary>
        private Vector2 acceleration;

        /// <summary>
        /// The minimum acceleration of the entity, in display units.
        /// </summary>
        private Vector2 minimumAcceleration;

        /// <summary>
        /// The deceleration of the entity, in display units.
        /// </summary>
        private Vector2 deceleration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollingDeath"/> class.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        /// <param name="displayHeight">The height of the display area.</param>
        /// <param name="minimumScrollSpeed">The minimum rate at which the entity should scroll.</param>
        /// <param name="maximumScrollSpeed">The maximum rate at which the entity should scroll.</param>
        /// <param name="scrollSpeed">The ideal rate at which the entity should scroll.</param>
        /// <param name="acceleration">The acceleration of the entity.</param>
        /// <param name="deceleration">The deceleration of the entity.</param>
        public ScrollingDeath(ref World physicsWorld, float displayHeight, float minimumScrollSpeed, float maximumScrollSpeed, float scrollSpeed, float acceleration, float deceleration)
        {
            this.Active = false;
            float width = 10.0f;
            this.physicsBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(width), ConvertUnits.ToSimUnits(displayHeight * 100.0f), 0.0f);
            this.physicsBody.BodyType = BodyType.Dynamic;
            this.physicsBody.Position = ConvertUnits.ToSimUnits(new Vector2(0.0f, displayHeight * 0.5f));
            this.physicsBody.CollisionCategories = EntityConstants.ScrollingDeathCategory;
            this.physicsBody.CollidesWith = EntityConstants.StickManCategory;
            this.physicsBody.IgnoreGravity = true;
            this.physicsBody.LinearVelocity = Vector2.Zero;
            this.normalVelocity = new Vector2(scrollSpeed, 0.0f);
            this.minimumVelocity = new Vector2(minimumScrollSpeed, 0.0f);
            this.maximumVelocity = new Vector2(maximumScrollSpeed, 0.0f);
            this.idealVelocity = this.minimumVelocity;
            this.minimumAcceleration = new Vector2(acceleration, 0.0f);
            this.acceleration = new Vector2(acceleration, 0.0f);
            this.deceleration = new Vector2(deceleration, 0.0f);
        }

        /// <summary>
        /// Gets a value indicating whether the entity is active or not.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Activates the scrolling entity.
        /// </summary>
        public void Activate()
        {
            this.Active = true;
            this.physicsBody.LinearVelocity = Vector2.Zero;
        }

        /// <summary>
        /// Updates the camera based on the scrolling death entity.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            float currentXVelocity = this.physicsBody.LinearVelocity.X;
            if (currentXVelocity < this.idealVelocity.X)
            {
                this.physicsBody.ApplyForce(ConvertUnits.ToSimUnits(this.acceleration));
            }
            else if (currentXVelocity > this.idealVelocity.X)
            {
                this.physicsBody.ApplyForce(ConvertUnits.ToSimUnits(this.deceleration));
            }

            if (this.Active)
            {
                Camera2D.X = ConvertUnits.ToDisplayUnits(this.physicsBody.Position.X);
            }
        }

        /// <summary>
        /// Sets the scroll rate to the normal scroll rate.
        /// </summary>
        public void GoToNormalScrollRate()
        {
            this.idealVelocity = this.normalVelocity;
            this.acceleration = this.minimumAcceleration;
        }

        /// <summary>
        /// Sets the scroll rate to the slow scroll rate.
        /// </summary>
        public void GoToSlowScrollRate()
        {
            this.idealVelocity = this.minimumVelocity;
            this.acceleration = this.minimumAcceleration;
        }

        /// <summary>
        /// Sets the scroll rate to the fast scroll rate.
        /// </summary>
        public void GoToFastScrollRate()
        {
            this.idealVelocity = this.maximumVelocity;
            this.acceleration = 2.0f * this.minimumAcceleration;
        }

        /// <summary>
        /// Disposes of the entity's physics body.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        public void Dispose(ref World physicsWorld)
        {
            if (this.physicsBody != null)
            {
                this.Active = false;
                physicsWorld.RemoveBody(this.physicsBody);
                this.physicsBody = null;
            }
        }
    }
}
