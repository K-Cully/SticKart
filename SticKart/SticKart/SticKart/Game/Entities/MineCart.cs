// -----------------------------------------------------------------------
// <copyright file="MineCart.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Entities
{
    using System;
    using Display;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics.Contacts;
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
        /// The physics world.
        /// </summary>
        private World physicsWorld;

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
        /// The physics body of the cart's left stabilizer.
        /// </summary>
        private Body stabilizerBodyLeft;

        /// <summary>
        /// The physics body of the cart's right stabilizer.
        /// </summary>
        private Body stabilizerBodyRight;

        /// <summary>
        /// The physics body to anchor the mine cart to before activation.
        /// </summary>
        private Body anchorBody;

        /// <summary>
        /// The physics joint connecting the chassis of the cart to the left wheel.
        /// </summary>
        private RevoluteJoint wheelJointLeft;

        /// <summary>
        /// The physics joint connecting the chassis of the cart to the right wheel.
        /// </summary>
        private RevoluteJoint wheelJointRight;

        /// <summary>
        /// The physics joint connecting the chassis of the cart to the left stabilizer.
        /// </summary>
        private RevoluteJoint stabilizerJointLeft;

        /// <summary>
        /// The physics joint connecting the chassis of the cart to the right stabilizer.
        /// </summary>
        private RevoluteJoint stabilizerJointRight;

        /// <summary>
        /// The physics joint connecting the chassis to the anchor body. 
        /// </summary>
        private RevoluteJoint anchorJoint;

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
        /// The offset of the left stabilizer wheel from the centre of the entity.
        /// </summary>
        private Vector2 stabilizerLeftOffset;

        /// <summary>
        /// The offset of the right stabilizer wheel from the centre of the entity.
        /// </summary>
        private Vector2 stabilizerRightOffset;

        /// <summary>
        /// The minimum horizontal speed, in physics units, of a cart entity.
        /// </summary>
        private float minimumHorizontalSpeed;
        
        /// <summary>
        /// The minimum horizontal speed, in physics units, of a cart entity, while carrying the player.
        /// </summary>
        private float minimumHorizontalSpeedPlayer;        

        /// <summary>
        /// The maximum horizontal speed, in physics units, of a cart entity.
        /// </summary>
        private float maximumHorizontalSpeed;

        /// <summary>
        /// A value indicating whether the cart is free of its anchor or not. 
        /// </summary>
        private bool moving;

        /// <summary>
        /// A value indicating whether the cart has reached the end of the level or not. 
        /// </summary>
        private bool reachedEnd;

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
        /// <param name="minimumHorizontalSpeedPlayer">The minimum horizontal speed of the mine cart, while carrying the player, in display units.</param>
        /// <param name="maximumHorizontalSpeed">The maximum horizontal speed of the mine cart, in display units.</param>
        /// <param name="acceleration">The acceleration of the mine cart, in display units.</param>
        /// <param name="deceleration">The deceleration of the mine cart, in display units.</param>
        public MineCart(SpriteBatch spriteBatch, ContentManager contentManager, ref World physicsWorld, Vector2 position, float minimumHorizontalSpeed, float minimumHorizontalSpeedPlayer, float maximumHorizontalSpeed, float acceleration, float deceleration)
        {
            this.physicsWorld = physicsWorld;
            this.cartSprite = new Sprite();
            this.wheelSprite = new Sprite();
            this.minimumHorizontalSpeed = ConvertUnits.ToSimUnits(minimumHorizontalSpeed);
            this.minimumHorizontalSpeedPlayer = ConvertUnits.ToSimUnits(minimumHorizontalSpeedPlayer);
            this.maximumHorizontalSpeed = ConvertUnits.ToSimUnits(maximumHorizontalSpeed);
            this.acceleration = ConvertUnits.ToSimUnits(acceleration);
            this.deceleration = ConvertUnits.ToSimUnits(deceleration);
            this.cartOffset = new Vector2(0.0f, -3.0f);
            this.wheelLeftOffset = new Vector2(-16.0f, 14.0f);
            this.wheelRightOffset = new Vector2(16.0f, 14.0f);
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.stabilizerLeftOffset = new Vector2(-this.cartSprite.Width * 1.25f, -this.cartSprite.Height * 0.6f);
            this.stabilizerRightOffset = new Vector2(this.cartSprite.Width * 1.25f, -this.cartSprite.Height * 0.6f);
            this.SetUpPhysics(ref physicsWorld, position);
            this.stabilizerBodyRight.OnCollision += this.CollisionHandlerStabilizer;
            this.stabilizerBodyLeft.OnCollision += this.CollisionHandlerStabilizer;
            this.wheelBodyLeft.OnCollision += this.CollisionHandlerWheel;
            this.wheelBodyRight.OnCollision += this.CollisionHandlerWheel;
            this.moving = false;
            this.reachedEnd = false;
        }

        /// <summary>
        /// Updates the mine cart entity.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="playerPosition">The player's position in physics units.</param>
        /// <param name="horizontalPlayerSpeed">The player's horizontal speed in physics units.</param>
        /// <param name="containsPlayer">A value indicating if whether the cart contains a player or not.</param>
        public void Update(GameTime gameTime, Vector2 playerPosition, float horizontalPlayerSpeed, bool containsPlayer)
        {
            if (this.moving && !this.reachedEnd)
            {
                Vector2 accelerationVector = this.CalculateDirectionOftravel();
                if (containsPlayer)
                {
                    if (this.cartBody.LinearVelocity.X < this.minimumHorizontalSpeedPlayer)
                    {
                        this.cartBody.ApplyForce(accelerationVector * this.acceleration * 3.0f);
                    }
                    else if (this.cartBody.LinearVelocity.X < this.maximumHorizontalSpeed)
                    {
                        this.cartBody.ApplyForce(accelerationVector * this.acceleration);
                    }
                }
                else if (playerPosition.X > this.cartBody.Position.X)
                {
                    if (this.cartBody.LinearVelocity.X > this.maximumHorizontalSpeed)
                    {
                        this.cartBody.ApplyForce(accelerationVector * this.deceleration);
                    }
                    else if (this.cartBody.LinearVelocity.X < horizontalPlayerSpeed || this.cartBody.LinearVelocity.X < this.maximumHorizontalSpeed)
                    {
                        this.cartBody.ApplyForce(accelerationVector * this.acceleration);
                    }
                }
                else if (playerPosition.X < this.cartBody.Position.X)
                {
                    if (this.cartBody.LinearVelocity.X > this.minimumHorizontalSpeed)
                    {
                        this.cartBody.ApplyForce(accelerationVector * this.deceleration);
                    }
                    else
                    {
                        this.cartBody.ApplyForce(accelerationVector * this.acceleration);
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
            this.physicsWorld.RemoveJoint(this.anchorJoint);
            this.physicsWorld.RemoveBody(this.anchorBody);
            this.moving = true;
        }

        /// <summary>
        /// Destroys the physics objects associated with a <see cref="MineCart"/> instance.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        public void Dispose(ref World physicsWorld)
        {
            if (physicsWorld != null)
            {
                if (this.moving == false)
                {
                    this.physicsWorld.RemoveJoint(this.anchorJoint);
                    this.physicsWorld.RemoveBody(this.anchorBody);
                }

                physicsWorld.RemoveJoint(this.stabilizerJointRight);
                physicsWorld.RemoveJoint(this.stabilizerJointLeft);
                physicsWorld.RemoveJoint(this.wheelJointRight);
                physicsWorld.RemoveJoint(this.wheelJointLeft);
                physicsWorld.RemoveBody(this.stabilizerBodyRight);
                physicsWorld.RemoveBody(this.stabilizerBodyLeft);
                physicsWorld.RemoveBody(this.wheelBodyRight);
                physicsWorld.RemoveBody(this.wheelBodyLeft);
                physicsWorld.RemoveBody(this.cartBody);
            }
        }
        
        /// <summary>
        /// Initializes and loads the textures for all of the sprites in a <see cref="MineCart"/> object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.cartSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.CartBody);
            this.wheelSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.CartWheel);
        }

        /// <summary>
        /// Sets up the physics bodies and joints used by the entity.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        /// <param name="position">The position of the entity in display units.</param>
        private void SetUpPhysics(ref World physicsWorld, Vector2 position)
        {
            float density = 1.1f;
            float restitution = 0.0f;

            // Main chassis
            this.cartBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(this.cartSprite.Width), ConvertUnits.ToSimUnits(this.cartSprite.Height), density, ConvertUnits.ToSimUnits(position + this.cartOffset));
            this.cartBody.BodyType = BodyType.Dynamic;
            this.cartBody.Restitution = restitution;
            this.cartBody.CollisionCategories = EntityConstants.MineCartCategory;
            this.cartBody.CollidesWith = EntityConstants.StickManCategory;
            this.cartBody.AngularDamping = 0.01f;

            // Anchor
            this.anchorBody = BodyFactory.CreateBody(physicsWorld, ConvertUnits.ToSimUnits(position));
            this.anchorBody.BodyType = BodyType.Static;
            this.anchorBody.CollidesWith = Category.None;

            // Left wheel
            this.wheelBodyLeft = BodyFactory.CreateCircle(physicsWorld, ConvertUnits.ToSimUnits(this.wheelSprite.Width / 2.0f), density, ConvertUnits.ToSimUnits(position + this.wheelLeftOffset));
            this.wheelBodyLeft.BodyType = BodyType.Dynamic;
            this.wheelBodyLeft.Restitution = restitution;
            this.wheelBodyLeft.Friction = 1.0f;
            this.wheelBodyLeft.CollisionCategories = EntityConstants.MineCartCategory;
            this.wheelBodyLeft.CollidesWith = EntityConstants.FloorCategory;

            // Right wheel
            this.wheelBodyRight = BodyFactory.CreateCircle(physicsWorld, ConvertUnits.ToSimUnits(this.wheelSprite.Width / 2.0f), density, ConvertUnits.ToSimUnits(position + this.wheelRightOffset));
            this.wheelBodyRight.BodyType = BodyType.Dynamic;
            this.wheelBodyRight.Restitution = restitution;
            this.wheelBodyRight.Friction = 1.0f;
            this.wheelBodyRight.CollisionCategories = EntityConstants.MineCartCategory;
            this.wheelBodyRight.CollidesWith = EntityConstants.FloorCategory;

            // Left stabilizer
            this.stabilizerBodyLeft = BodyFactory.CreateCircle(physicsWorld, ConvertUnits.ToSimUnits(this.wheelSprite.Width / 2.0f), density, ConvertUnits.ToSimUnits(position + this.stabilizerLeftOffset));
            this.stabilizerBodyLeft.Mass = 0.0f;
            this.stabilizerBodyLeft.BodyType = BodyType.Dynamic;            
            this.stabilizerBodyLeft.Restitution = restitution;
            this.stabilizerBodyLeft.Friction = 1.0f;
            this.stabilizerBodyLeft.CollisionCategories = EntityConstants.MineCartCategory;
            this.stabilizerBodyLeft.CollidesWith = EntityConstants.FloorCategory;

            // Right stabilizer
            this.stabilizerBodyRight = BodyFactory.CreateCircle(physicsWorld, ConvertUnits.ToSimUnits(this.wheelSprite.Width / 2.0f), density, ConvertUnits.ToSimUnits(position + this.stabilizerRightOffset));
            this.stabilizerBodyRight.Mass = 0.0f;
            this.stabilizerBodyRight.BodyType = BodyType.Dynamic;
            this.stabilizerBodyRight.Restitution = restitution;
            this.stabilizerBodyRight.Friction = 1.0f;
            this.stabilizerBodyRight.CollisionCategories = EntityConstants.MineCartCategory;
            this.stabilizerBodyRight.CollidesWith = EntityConstants.FloorCategory;

            // Anchor joint
            this.anchorJoint = JointFactory.CreateRevoluteJoint(physicsWorld, this.anchorBody, this.cartBody, Vector2.Zero);

            // Wheel joints
            this.wheelJointLeft = JointFactory.CreateRevoluteJoint(physicsWorld, this.cartBody, this.wheelBodyLeft, Vector2.Zero);
            this.wheelJointRight = JointFactory.CreateRevoluteJoint(physicsWorld, this.cartBody, this.wheelBodyRight, Vector2.Zero);

            // Stabilizer joints
            this.stabilizerJointLeft = JointFactory.CreateRevoluteJoint(physicsWorld, this.cartBody, this.stabilizerBodyLeft, Vector2.Zero);
            this.stabilizerJointRight = JointFactory.CreateRevoluteJoint(physicsWorld, this.cartBody, this.stabilizerBodyRight, Vector2.Zero);
        }

        /// <summary>
        /// Calculates the direction that the cart should move, based on the floor direction.
        /// </summary>
        /// <returns>The direction the cart should travel.</returns>
        private Vector2 CalculateDirectionOftravel()
        {
            Vector2 direction = Vector2.UnitX;
            if (this.wheelBodyRight.ContactList != null && this.wheelBodyRight.ContactList.Contact.FixtureA.CollisionCategories == EntityConstants.FloorCategory && this.wheelBodyRight.ContactList.Contact.FixtureA.UserData == null)
            {
                direction = (this.wheelBodyRight.ContactList.Contact.FixtureA.Shape as EdgeShape).Vertex2 - (this.wheelBodyRight.ContactList.Contact.FixtureA.Shape as EdgeShape).Vertex1;
            }
            else if (this.wheelBodyLeft.ContactList != null && this.wheelBodyLeft.ContactList.Contact.FixtureA.CollisionCategories == EntityConstants.FloorCategory && this.wheelBodyLeft.ContactList.Contact.FixtureA.UserData == null)
            {
                direction = (this.wheelBodyLeft.ContactList.Contact.FixtureA.Shape as EdgeShape).Vertex2 - (this.wheelBodyLeft.ContactList.Contact.FixtureA.Shape as EdgeShape).Vertex1;
            }
            else if (this.stabilizerBodyRight.ContactList != null && this.stabilizerBodyRight.ContactList.Contact.FixtureA.CollisionCategories == EntityConstants.FloorCategory && this.stabilizerBodyRight.ContactList.Contact.FixtureA.UserData == null)
            {
                direction = (this.stabilizerBodyRight.ContactList.Contact.FixtureA.Shape as EdgeShape).Vertex2 - (this.stabilizerBodyRight.ContactList.Contact.FixtureA.Shape as EdgeShape).Vertex1;
            }
            else if (this.stabilizerBodyLeft.ContactList != null && this.stabilizerBodyLeft.ContactList.Contact.FixtureA.CollisionCategories == EntityConstants.FloorCategory && this.stabilizerBodyLeft.ContactList.Contact.FixtureA.UserData == null)
            {
                direction = (this.stabilizerBodyLeft.ContactList.Contact.FixtureA.Shape as EdgeShape).Vertex2 - (this.stabilizerBodyLeft.ContactList.Contact.FixtureA.Shape as EdgeShape).Vertex1;
            }

            direction.Normalize();
            return direction;
        }

        /// <summary>
        /// Collision event handler for the stabilizer wheels.
        /// </summary>
        /// <param name="fixtureOne">The first colliding fixture.</param>
        /// <param name="fixtureTwo">The second colliding fixture.</param>
        /// <param name="contact">The contact object.</param>
        /// <returns>Whether the collision was accepted or not.</returns>
        private bool CollisionHandlerStabilizer(Fixture fixtureOne, Fixture fixtureTwo, Contact contact)
        {
            bool collided = true;
            if (fixtureTwo.CollisionCategories == EntityConstants.FloorCategory && fixtureTwo.Body.UserData != null)
            {
                collided = false;
            }

            return collided;
        }

        /// <summary>
        /// Collision event handler for the wheels.
        /// </summary>
        /// <param name="fixtureOne">The first colliding fixture.</param>
        /// <param name="fixtureTwo">The second colliding fixture.</param>
        /// <param name="contact">The contact object.</param>
        /// <returns>Whether the collision was accepted or not.</returns>
        private bool CollisionHandlerWheel(Fixture fixtureOne, Fixture fixtureTwo, Contact contact)
        {
            bool collided = true;
            if (fixtureTwo.CollisionCategories == EntityConstants.FloorCategory && fixtureTwo.Body.UserData != null)
            {
                this.reachedEnd = false;
                this.cartBody.LinearVelocity = Vector2.Zero;
                this.wheelBodyLeft.LinearVelocity = Vector2.Zero;
                this.wheelBodyRight.LinearVelocity = Vector2.Zero;
                this.stabilizerBodyLeft.LinearVelocity = Vector2.Zero;
                this.stabilizerBodyRight.LinearVelocity = Vector2.Zero;
            }

            return collided;
        }
    }
}
