// -----------------------------------------------------------------------
// <copyright file="MineCart.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Entities
{
    using Display;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics.Joints;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a mining cart entity.
    /// </summary>
    public class MineCart
    {
        #region physics

        /// <summary>
        /// The physics body representing the chassis of the cart.
        /// </summary>
        private Body cartBody;

        /// <summary>
        /// The physics body of the cart's left wheel.
        /// </summary>
        private Body wheelBodyLeft;

        /// <summary>
        /// The physics body of the cart's right wheel.
        /// </summary>
        private Body wheelBodyRight;

        /// <summary>
        /// The physics joint connecting the chassis of the cart to the left wheel.
        /// </summary>
        private RevoluteJoint wheelJointLeft;

        /// <summary>
        /// The physics joint connecting the chassis of the cart to the right wheel.
        /// </summary>
        private RevoluteJoint wheelJointRight;

        /// <summary>
        /// The offset of the cart chassis from the centre of the entity.
        /// </summary>
        private Vector2 cartOffset;

        /// <summary>
        /// The offset of the left wheel from the centre of the entity.
        /// </summary>
        private Vector2 wheelLeftOffset;

        /// <summary>
        /// The offset of the right wheel from the centre of the entity.
        /// </summary>
        private Vector2 wheelRightOffset;

        /// <summary>
        /// The minimum horizontal speed, in physics units, of a cart entity.
        /// </summary>
        private float minimumHorizontalSpeed;

        /// <summary>
        /// The maximum horizontal speed, in physics units, of a cart entity.
        /// </summary>
        private float maximumHorizontalSpeed;

        /// <summary>
        /// A value indicating whether the cart should be moving or not. 
        /// </summary>
        private bool moving;

        /// <summary>
        /// The acceleration rate, in physics units, of a mine cart.
        /// </summary>
        private float acceleration;

        /// <summary>
        /// The deceleration rate, in physics units, of a mine cart.
        /// </summary>
        private float deceleration;

        #endregion

        #region sprites

        /// <summary>
        /// The sprite representing the chassis of the cart.
        /// </summary>
        private Sprite cartSprite;

        /// <summary>
        /// The sprite representing a wheel of the cart.
        /// </summary>
        private Sprite wheelSprite;

        #endregion
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MineCart"/> class.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to render sprites with.</param>
        /// <param name="contentManager">The content manager to load assets with.</param>
        /// <param name="physicsWorld">The game's physics world.</param>
        /// <param name="position">The position of the mine cart.</param>
        /// <param name="minimumHorizontalSpeed">The minimum horizontal speed of the mine cart, in display units.</param>
        /// <param name="maximumHorizontalSpeed">The maximum horizontal speed of the mine cart, in display units.</param>
        /// <param name="acceleration">The acceleration of the mine cart, in display units.</param>
        /// <param name="deceleration">The deceleration of the mine cart, in display units.</param>
        public MineCart(SpriteBatch spriteBatch, ContentManager contentManager, ref World physicsWorld, Vector2 position, float minimumHorizontalSpeed, float maximumHorizontalSpeed, float acceleration, float deceleration)
        {
            this.minimumHorizontalSpeed = ConvertUnits.ToSimUnits(minimumHorizontalSpeed);
            this.maximumHorizontalSpeed = ConvertUnits.ToSimUnits(maximumHorizontalSpeed);
            this.acceleration = ConvertUnits.ToSimUnits(acceleration);
            this.deceleration = ConvertUnits.ToSimUnits(deceleration);
            this.cartOffset = new Vector2(0.0f, -3.0f);
            this.wheelLeftOffset = new Vector2(-16.0f, 14.0f);
            this.wheelRightOffset = new Vector2(16.0f, 14.0f);
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.SetUpPhysics(ref physicsWorld);
            this.SetPosition(position);
            this.moving = false;
            this.cartBody.BodyType = BodyType.Static; // TODO: test
        }

        /// <summary>
        /// Updates the mine cart entity.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="playerPosition">The player's position in physics units.</param>
        /// <param name="horizontalPlayerSpeed">The player's horizontal speed in physics units.</param>
        public void Update(GameTime gameTime, Vector2 playerPosition, float horizontalPlayerSpeed)
        {
            if (this.moving)
            {
                if (playerPosition.X > this.cartBody.Position.X)
                {
                    if (this.cartBody.LinearVelocity.X > this.maximumHorizontalSpeed)
                    {
                        this.cartBody.ApplyForce(new Vector2(this.deceleration, 0.0f));
                    }
                    else if (this.cartBody.LinearVelocity.X < horizontalPlayerSpeed)
                    {
                        this.cartBody.ApplyForce(new Vector2(this.acceleration, 0.0f));
                    }
                }
                else if (playerPosition.X < this.cartBody.Position.X)
                {
                    if (this.cartBody.LinearVelocity.X > this.minimumHorizontalSpeed)
                    {
                        this.cartBody.ApplyForce(new Vector2(this.deceleration, 0.0f));
                    }
                    else
                    {
                        this.cartBody.ApplyForce(new Vector2(this.acceleration, 0.0f));
                    }
                }
            }
        }

        /// <summary>
        /// Draws a cart entity.
        /// </summary>
        public void Draw()
        {
            Camera2D.Draw(this.cartSprite, ConvertUnits.ToDisplayUnits(this.cartBody.Position), this.cartBody.Rotation);
            Camera2D.Draw(this.wheelSprite, ConvertUnits.ToDisplayUnits(this.wheelBodyLeft.Position), this.wheelBodyLeft.Rotation);
            Camera2D.Draw(this.wheelSprite, ConvertUnits.ToDisplayUnits(this.wheelBodyRight.Position), this.wheelBodyRight.Rotation);
        }

        /// <summary>
        /// Activates the mine cart entity.
        /// </summary>
        public void Activate()
        {
            this.moving = true;
            this.cartBody.BodyType = BodyType.Dynamic; // TODO: test
        }

        /// <summary>
        /// Destroys the physics objects associated with a <see cref="MineCart"/> instance.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        public void Dispose(ref World physicsWorld)
        {
            if (physicsWorld != null)
            {
                physicsWorld.RemoveJoint(this.wheelJointRight);
                physicsWorld.RemoveJoint(this.wheelJointLeft);
                physicsWorld.RemoveBody(this.wheelBodyRight);
                physicsWorld.RemoveBody(this.wheelBodyLeft);
                physicsWorld.RemoveBody(this.cartBody);
            }
        }

        /// <summary>
        /// Sets the position of the physics bodies.
        /// </summary>
        /// <param name="position">The centre position of the cart entity.</param>
        private void SetPosition(Vector2 position)
        {
            this.cartBody.Position = ConvertUnits.ToSimUnits(position + this.cartOffset);
            this.wheelBodyLeft.Position = ConvertUnits.ToSimUnits(position + this.wheelLeftOffset);
            this.wheelBodyRight.Position = ConvertUnits.ToSimUnits(position + this.wheelRightOffset);
        }

        /// <summary>
        /// Initializes and loads the textures for all of the sprites in a <see cref="MineCart"/> object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.cartSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.CartBody);
            this.cartSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.CartWheel);
        }

        /// <summary>
        /// Sets up the physics bodies and joints used by the entity.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        private void SetUpPhysics(ref World physicsWorld)
        {
            float density = 1.1f;
            float restitution = 0.0f;

            // Main chassis
            this.cartBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(this.cartSprite.Width), ConvertUnits.ToSimUnits(this.cartSprite.Height), density, ConvertUnits.ToSimUnits(this.cartOffset));
            this.cartBody.BodyType = BodyType.Dynamic;
            this.cartBody.Restitution = restitution;
            this.cartBody.CollisionCategories = EntityConstants.MineCartCategory;

            // Left wheel
            this.wheelBodyLeft = BodyFactory.CreateCircle(physicsWorld, ConvertUnits.ToSimUnits(this.wheelSprite.Width / 2.0f), density, ConvertUnits.ToSimUnits(this.wheelLeftOffset));
            this.wheelBodyLeft.BodyType = BodyType.Dynamic;
            this.wheelBodyLeft.Restitution = restitution;
            this.wheelBodyLeft.Friction = float.MaxValue;
            this.wheelBodyLeft.IgnoreCollisionWith(this.cartBody);
            this.wheelBodyLeft.CollisionCategories = EntityConstants.MineCartCategory;

            // Right wheel
            this.wheelBodyRight = BodyFactory.CreateCircle(physicsWorld, ConvertUnits.ToSimUnits(this.wheelSprite.Width / 2.0f), density, ConvertUnits.ToSimUnits(this.wheelRightOffset));
            this.wheelBodyRight.BodyType = BodyType.Dynamic;
            this.wheelBodyRight.Restitution = restitution;
            this.wheelBodyRight.Friction = float.MaxValue;
            this.wheelBodyRight.IgnoreCollisionWith(this.cartBody);
            this.wheelBodyRight.CollisionCategories = EntityConstants.MineCartCategory;

            // Wheel joints
            this.wheelJointLeft = JointFactory.CreateRevoluteJoint(physicsWorld, this.cartBody, this.wheelBodyLeft, Vector2.Zero);
            this.wheelJointRight = JointFactory.CreateRevoluteJoint(physicsWorld, this.cartBody, this.wheelBodyRight, Vector2.Zero);
        }
    }
}
