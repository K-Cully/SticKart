// -----------------------------------------------------------------------
// <copyright file="StickMan.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Entities
{
    using Display;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics.Contacts;
    using FarseerPhysics.Dynamics.Joints;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Input;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    
    /// <summary>
    /// Defines an in game player representation.
    /// </summary>
    public class StickMan
    {
        /// <summary>
        /// The max length of time which wheel collisions should be disabled for.
        /// </summary>
        private const float MaxWheelDisabledTime = 1.0f;

        /// <summary>
        /// The amount to augment the player's jump with the jump power up.
        /// </summary>
        private const float JumpPowerModifier = 2.0f;

        #region physics

        /// <summary>
        /// The game's physics world.
        /// </summary>
        private World physicsWorld;

        /// <summary>
        /// The physics body representing the standing stickman.
        /// </summary>
        private Body fullBody;

        /// <summary>
        /// The offset from the center of the stickman to the full body, in display space.
        /// </summary>
        private Vector2 fullBodyOffset;

        /// <summary>
        /// The physics body representing the crouching stickman.
        /// </summary>
        private Body smallBody;

        /// <summary>
        /// The offset from the center of the stickman to the small body, in display space.
        /// </summary>
        private Vector2 smallBodyOffset;

        /// <summary>
        /// The physics body at the bottom of the stickman.
        /// This is used to control the movement of the stickman and to allow the stickman to traverse bumpy terrain easily.
        /// </summary>
        private Body wheelBody;

        /// <summary>
        /// The offset from the center of the stickman to the wheel body, in display space.
        /// </summary>
        private Vector2 wheelBodyOffset;

        /// <summary>
        /// A joint connecting the <see cref="fullBody"/> to the <see cref="smallBody"/>.
        /// </summary>
        private WeldJoint bodyJoint;

        /// <summary>
        /// A motor joint connecting the <see cref="smallBody"/> to the <see cref="wheelBody"/>.
        /// </summary>
        private RevoluteJoint motorJoint;

        /// <summary>
        /// A joint which locks the angle of the stickman to prevent it falling over.
        /// </summary>
        private FixedAngleJoint angleUprightJoint;

        /// <summary>
        /// A joint used to tie the stickman to a mine cart.
        /// </summary>
        private RevoluteJoint cartJoint;

        /// <summary>
        /// The minimum velocity at which the stickman can move horizontally.
        /// </summary>
        private float minimumHorizontalVelocity;

        /// <summary>
        /// The maximum velocity at which the stickman can move horizontally.
        /// </summary>
        private float maximumHorizontalVelocity;

        /// <summary>
        /// The maximum velocity at which the stickman can move horizontally under normal circumstances.
        /// </summary>
        private float standardHorizontalVelocity;

        /// <summary>
        /// The maximum velocity at which the stickman can move horizontally while the speed power up is active.
        /// </summary>
        private float speedHorizontalVelocity;

        /// <summary>
        /// The maximum velocity at which the stickman can move horizontally while on the floor.
        /// </summary>
        private float floorHorizontalVelocity;

        /// <summary>
        /// The ideal velocity at which the stickman is moving horizontally.
        /// </summary>
        private float idealHorizontalVelocity;

        /// <summary>
        /// The acceleration which should be applied to the stickman upon receiving a run command.
        /// </summary>
        private float acceleration;

        #endregion

        #region sprites

        /// <summary>
        /// The sprite which represents the stickman's standing pose.
        /// </summary>
        private Sprite standingSprite; // TODO: add animated sprite class and other sprites
        
        #endregion

        #region state_managment

        /// <summary>
        /// The minimum health the stickman can have.
        /// </summary>
        private float minimumHealth;

        /// <summary>
        /// The maximum health the stickman can have.
        /// </summary>
        private float maximumHealth;

        /// <summary>
        /// The current health the stickman has.
        /// </summary>
        private float health;

        /// <summary>
        /// The impulse applied to the stickman when they jump.
        /// </summary>
        private float jumpImpulse;

        /// <summary>
        /// The current state the stickman is in.
        /// </summary>
        private PlayerState state;

        /// <summary>
        /// Whether the player is on the floor or not.
        /// </summary>
        private bool onFloor;

        /// <summary>
        /// Whether the player is jumping down or not.
        /// </summary>
        private bool jumpingDown;

        /// <summary>
        /// Whether the wheel should collide or not.
        /// </summary>
        private bool wheelCollisionDisabled;

        /// <summary>
        /// The time which the wheel collisions have been disabled.
        /// </summary>
        private float wheelDisabledTimer;
        
        /// <summary>
        /// The player's active power up.
        /// </summary>
        private PowerUpType activePowerUp;

        /// <summary>
        /// The power up timer.
        /// </summary>
        private float powerUpTimer;

        /// <summary>
        /// The length of time the current power up should be active for.
        /// </summary>
        private float powerUpLength;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StickMan"/> class.
        /// </summary>
        /// <param name="physicsWorld">The physics world to create physics objects in.</param>
        /// <param name="maximumHorizontalSpeed">The maximum horizontal speed at which the stickman can travel.</param>
        /// <param name="maximumHealth">The maximum health the stickman can have.</param>
        /// <param name="jumpImpulse">The impulse to apply to the stickman when jumping.</param>
        /// <param name="spriteBatch">The sprite batch to use for rendering the stickman.</param>
        /// <param name="contentManager">The content manager to use in loading sprites.</param>
        public StickMan(ref World physicsWorld, float maximumHorizontalSpeed, float maximumHealth, float jumpImpulse, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.physicsWorld = physicsWorld;
            this.activePowerUp = PowerUpType.None;
            this.powerUpTimer = 0.0f;
            this.powerUpLength = 0.0f;
            this.minimumHorizontalVelocity = 0.0f;
            this.idealHorizontalVelocity = 0.0f;
            this.standardHorizontalVelocity = maximumHorizontalSpeed;
            this.floorHorizontalVelocity = maximumHorizontalSpeed * 0.25f;
            this.speedHorizontalVelocity = maximumHorizontalSpeed * 1.25f;
            this.maximumHorizontalVelocity = maximumHorizontalSpeed;
            this.minimumHealth = 0.0f;
            this.Score = 0;
            this.health = maximumHealth;
            this.maximumHealth = maximumHealth;
            this.jumpImpulse = jumpImpulse;
            this.InCart = false;
            this.state = PlayerState.standing;
            this.onFloor = false;
            this.jumpingDown = false;
            this.wheelCollisionDisabled = false;
            this.wheelDisabledTimer = 0.0f;
            this.standingSprite = new Sprite();
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.fullBodyOffset = new Vector2(0.0f, -this.standingSprite.Height / 8.0f);
            this.smallBodyOffset = new Vector2(0.0f, this.standingSprite.Height / 8.0f);
            this.wheelBodyOffset = new Vector2(0.0f, this.standingSprite.Height / 4.0f);
            this.SetUpPhysicsObjects(ref physicsWorld);
            this.cartJoint = null;
            this.acceleration = 9.0f;
            this.wheelBody.OnCollision += this.CollisionHandlerWheel;
            this.smallBody.OnCollision += this.CollisionHandlerSmallBody;
            this.fullBody.OnCollision += this.CollisionHandlerFullBody;
        }

        #region accessors

        /// <summary>
        /// Gets the player's score.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the player is in the mining cart or not.
        /// </summary>
        public bool InCart { get; private set; }

        /// <summary>
        /// Gets the stick man's display coordinates.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                if (this.state == PlayerState.crouching)
                {
                    return ConvertUnits.ToDisplayUnits(this.wheelBody.Position);
                }
                else
                {
                    Vector2 pos = ConvertUnits.ToDisplayUnits(this.wheelBody.Position);
                    pos.Y -= this.standingSprite.Height / 4.0f;
                    return pos;
                }
            }
        }

        /// <summary>
        /// Gets the stickman's position in physics world coordinates.
        /// </summary>
        public Vector2 PhysicsPosition
        {
            get
            {
                if (this.state == PlayerState.crouching)
                {
                    return this.wheelBody.Position;
                }
                else
                {
                    Vector2 pos = this.wheelBody.Position;
                    pos.Y -= ConvertUnits.ToSimUnits(this.standingSprite.Height / 4.0f);
                    return pos;
                }
            }
        }

        /// <summary>
        /// Gets the stickman's horizontal speed in physics units. 
        /// </summary>
        public float HorizontalSpeed
        {
            get
            {
                return this.fullBody.LinearVelocity.X;
            }
        }

        /// <summary>
        /// Gets the player's remaining health as a percentage of its maximum.
        /// </summary>
        public float PercentHealth
        {
            get
            {
                if (this.health <= 0)
                {
                    return 0.0f;
                }
                else
                {
                    return 100.0f * (this.health / this.maximumHealth);
                }
            }
        }

        /// <summary>
        /// Gets the player's active power up.
        /// </summary>
        public PowerUpType ActivePowerUp
        {
            get
            {
                return this.activePowerUp;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the player is dead or not.
        /// </summary>
        public bool IsDead
        {
            get
            {
                return this.state == PlayerState.dead;
            }
        }

        #endregion
        
        /// <summary>
        /// Updates the state of the player and any time based elements of the player. 
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            if (this.activePowerUp != PowerUpType.None)
            {
                this.UpdatePowerUp(gameTime);
            }

            if (this.wheelCollisionDisabled)
            {
                this.wheelDisabledTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.wheelDisabledTimer > StickMan.MaxWheelDisabledTime)
                {
                    this.wheelCollisionDisabled = false;
                }
            }

            if (!this.InCart)
            {
                if (this.state == PlayerState.running)
                {
                    this.idealHorizontalVelocity *= this.idealHorizontalVelocity < 0.2f ? 0.0f : 0.95f;
                    if (this.smallBody.LinearVelocity.X > this.idealHorizontalVelocity)
                    {
                        if (this.motorJoint.MotorSpeed > 1.0f)
                        {
                            this.motorJoint.MotorSpeed *= 0.875f;
                        }
                        else
                        {
                            this.motorJoint.MotorSpeed -= this.acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                    }
                    else if (this.smallBody.LinearVelocity.X < this.idealHorizontalVelocity && this.motorJoint.MotorSpeed < this.maximumHorizontalVelocity * 2.0f)
                    {
                        this.motorJoint.MotorSpeed += this.acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if (this.idealHorizontalVelocity == 0.0f && this.smallBody.LinearVelocity.X < 1.0f)
                    {
                        this.motorJoint.MotorSpeed = 0.0f;
                        this.state = PlayerState.standing;
                    }
                }
                else if (this.state == PlayerState.jumping && this.smallBody.LinearVelocity.Y >= 0.0f)
                {
                    this.state = PlayerState.falling;
                }
            }
        }

        /// <summary>
        /// Makes the the stick man run if it is not jumping.
        /// </summary>
        public void Run()
        {
            if (!this.InCart && this.state != PlayerState.jumping && this.state != PlayerState.falling)
            {
                this.idealHorizontalVelocity += 5.0f;
                if (this.idealHorizontalVelocity > this.maximumHorizontalVelocity)
                {
                    this.idealHorizontalVelocity = this.maximumHorizontalVelocity;
                }

                this.state = PlayerState.running;
            }
        }

        /// <summary>
        /// Makes the stick man stand if it is crouching.
        /// </summary>
        public void Stand()
        {
            if (this.state == PlayerState.crouching)
            {
                this.state = PlayerState.standing;
            }
        }

        /// <summary>
        /// Makes the stick man crouch if it is standing.
        /// </summary>
        public void CrouchOrJumpDown()
        {
            if (this.state != PlayerState.crouching && this.InCart)
            {
                this.state = PlayerState.crouching;
                // TODO: test collisions with top box
            }
            else if (this.state == PlayerState.running || this.state == PlayerState.standing)
            {
                this.smallBody.ApplyLinearImpulse(new Vector2(0.0f, this.jumpImpulse / 8.0f));
                this.state = PlayerState.falling;
                this.wheelCollisionDisabled = true;
                this.wheelDisabledTimer = 0.0f;
                this.jumpingDown = true;
            }
        }

        /// <summary>
        /// Makes the stick man jump if it is not already jumping.
        /// </summary>
        public void Jump()
        {
            if (this.state != PlayerState.jumping && this.state != PlayerState.falling)
            {
                if (this.InCart && this.cartJoint != null)
                {
                    this.InCart = false;
                    this.physicsWorld.RemoveJoint(this.cartJoint);
                    this.cartJoint = null;
                }

                this.smallBody.ApplyLinearImpulse(new Vector2(0.0f, this.jumpImpulse));
                this.state = PlayerState.jumping;
                if (this.motorJoint.MotorSpeed > 1.0f)
                {
                    this.motorJoint.MotorSpeed = this.motorJoint.MotorSpeed * 0.75f;
                }
                else
                {
                    this.motorJoint.MotorSpeed = 0.0f;
                }
            }
        }

        /// <summary>
        /// Draws the stick man to the screen.
        /// </summary>
        public void Draw()
        {
            switch (this.state)
            {
                case PlayerState.standing:
                    Camera2D.Draw(this.standingSprite, this.Position, this.smallBody.Rotation);
                    break;
                case PlayerState.crouching:

                    // TODO
                    break;
                case PlayerState.jumping:
                    Camera2D.Draw(this.standingSprite, this.Position, this.smallBody.Rotation);

                    // TODO
                    break;
                case PlayerState.running:
                    Camera2D.Draw(this.standingSprite, this.Position, this.smallBody.Rotation);

                    // TODO
                    break;
                case PlayerState.falling:
                    Camera2D.Draw(this.standingSprite, this.Position, this.smallBody.Rotation);
                    break;
                case PlayerState.dead:

                    // TODO
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Resets the stick man. 
        /// </summary>
        /// <param name="position">The display coordinate to place the player at.</param>
        public void Reset(Vector2 position)
        {
            if (this.cartJoint != null)
            {
                this.physicsWorld.RemoveJoint(this.cartJoint);
                this.cartJoint = null;
            }

            this.activePowerUp = PowerUpType.None;
            this.powerUpTimer = 0.0f;
            this.fullBody.Position = ConvertUnits.ToSimUnits(position + this.fullBodyOffset);
            this.smallBody.Position = ConvertUnits.ToSimUnits(position + this.smallBodyOffset);
            this.wheelBody.Position = ConvertUnits.ToSimUnits(position + this.wheelBodyOffset);
            this.state = PlayerState.standing;
            this.idealHorizontalVelocity = 0.0f;
            this.InCart = false;
            this.onFloor = false;
            this.motorJoint.MotorSpeed = 0.0f;
            this.smallBody.Rotation = 0.0f;
            this.fullBody.Rotation = 0.0f;
            this.wheelBody.Rotation = 0.0f;
            this.smallBody.LinearVelocity = Vector2.Zero;
            this.fullBody.LinearVelocity = Vector2.Zero;
            this.wheelBody.LinearVelocity = Vector2.Zero;
            this.wheelCollisionDisabled = false;
            this.wheelDisabledTimer = 0.0f;
            this.Score = 0;
        }

        #region initialization

        /// <summary>
        /// Initializes and loads the textures for all of the sprites in a StickMan object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            // TODO: rest of sprites
            this.standingSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.StickManSubPath + EntityConstants.StickManStanding);
        }

        /// <summary>
        /// Sets up all the physical properties of the StickMan object.
        /// </summary>
        /// <param name="physicsWorld">The physics world to set the objects up in.</param>
        private void SetUpPhysicsObjects(ref World physicsWorld)
        {
            float density = 1.1f;
            float restitution = 0.125f;

            // Upper body for standing
            this.fullBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(this.standingSprite.Width), ConvertUnits.ToSimUnits(this.standingSprite.Height * 0.75f), density, ConvertUnits.ToSimUnits(this.fullBodyOffset));
            this.fullBody.BodyType = BodyType.Dynamic;
            this.fullBody.Restitution = restitution;
            this.fullBody.CollisionCategories = EntityConstants.StickManCategory;

            // Middle body for crouching
            this.smallBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(this.standingSprite.Width), ConvertUnits.ToSimUnits(this.standingSprite.Height * 0.25f), density, ConvertUnits.ToSimUnits(this.smallBodyOffset));
            this.smallBody.BodyType = BodyType.Dynamic;
            this.smallBody.IgnoreCollisionWith(this.fullBody);
            this.smallBody.Restitution = restitution;
            this.smallBody.CollisionCategories = EntityConstants.StickManCategory;

            // Wheel for movement
            this.wheelBody = BodyFactory.CreateCircle(physicsWorld, ConvertUnits.ToSimUnits(this.standingSprite.Width / 2.0f), density, ConvertUnits.ToSimUnits(this.wheelBodyOffset));
            this.wheelBody.BodyType = BodyType.Dynamic;
            this.wheelBody.IgnoreCollisionWith(this.smallBody);
            this.wheelBody.IgnoreCollisionWith(this.fullBody);
            this.wheelBody.Restitution = restitution;
            this.wheelBody.Friction = float.MaxValue;
            this.wheelBody.CollisionCategories = EntityConstants.StickManCategory;

            // Joints to connect the bodies.
            this.bodyJoint = JointFactory.CreateWeldJoint(physicsWorld, this.fullBody, this.smallBody, this.smallBody.Position);
            this.angleUprightJoint = JointFactory.CreateFixedAngleJoint(physicsWorld, this.smallBody);
            this.motorJoint = JointFactory.CreateRevoluteJoint(physicsWorld, this.smallBody, this.wheelBody, Vector2.Zero);
            this.motorJoint.MotorSpeed = 0.0f;
            this.motorJoint.MaxMotorTorque = 1000.0f; // TODO: set correctly.
            this.motorJoint.MotorEnabled = true;
        }

        #endregion

        /// <summary>
        /// Updates the state of the active power up.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        private void UpdatePowerUp(GameTime gameTime)
        {
            this.powerUpTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (this.activePowerUp)
            {
                case PowerUpType.Health:
                    if (this.health < this.maximumHealth)
                    {
                        this.health += 0.02f;
                    }

                    break;
                case PowerUpType.Speed:
                    if (this.onFloor)
                    {
                        this.maximumHorizontalVelocity = 0.5f * (this.speedHorizontalVelocity + this.floorHorizontalVelocity);
                    }
                    else
                    {
                        this.maximumHorizontalVelocity = this.speedHorizontalVelocity;
                    }

                    break;
                default:
                    break;
            }

            if (this.powerUpTimer > this.powerUpLength)
            {
                this.ResetPowerUpModifiers();
                this.activePowerUp = PowerUpType.None;
                this.powerUpTimer = 0.0f;
            }
        }

        /// <summary>
        /// resets any values which are actively modified by power ups.
        /// </summary>
        private void ResetPowerUpModifiers()
        {
            if (this.activePowerUp == PowerUpType.Jump)
            {
                this.jumpImpulse /= StickMan.JumpPowerModifier;
            }

            if (this.onFloor)
            {
                this.maximumHorizontalVelocity = this.floorHorizontalVelocity;
            }
            else
            {
                this.maximumHorizontalVelocity = this.standardHorizontalVelocity;
            }
        }

        /// <summary>
        /// Sets the stick man's state to standing.
        /// This should be called on collision between the wheel and anything except the cart.
        /// </summary>
        private void Land()
        {
            this.jumpingDown = false;
            if (this.motorJoint.MotorSpeed > 0.0f)
            {
                this.state = PlayerState.running;
            }
            else
            {
                this.state = PlayerState.standing;
            }
        }

        /// <summary>
        /// Attaches the player to the cart with a joint.
        /// </summary>
        /// <param name="cartBody">The cart body.</param>
        private void LandInCart(Body cartBody)
        {
            this.InCart = true;
            this.state = PlayerState.standing;
            this.motorJoint.MotorSpeed = 0.0f;
            Vector2 cartTopPosition = cartBody.Position + ConvertUnits.ToSimUnits(-24.0f * Vector2.UnitY);
            this.fullBody.Position = cartTopPosition + ConvertUnits.ToSimUnits(this.fullBodyOffset);
            this.smallBody.Position = cartTopPosition + ConvertUnits.ToSimUnits(this.smallBodyOffset);
            this.wheelBody.Position = cartTopPosition + ConvertUnits.ToSimUnits(this.wheelBodyOffset);
            if (this.cartJoint == null)
            {
                this.cartJoint = JointFactory.CreateRevoluteJoint(this.physicsWorld, this.fullBody, cartBody, Vector2.Zero);
            }
        }

        /// <summary>
        /// Performs collision response with an interactive entity.
        /// </summary>
        /// <param name="entityData">The interactive entity's user data.</param>
        /// <returns>A value indicating whether the collision response should be computed by the physics engine or not.</returns>
        private bool CollideWithInteractiveEntity(InteractiveEntityUserData entityData)
        {
            bool collided = true;
            switch (entityData.EntityType)
            {
                case InteractiveEntityType.Obstacle:
                    if (this.activePowerUp == PowerUpType.Invincibility)
                    {
                        collided = false;
                    }
                    else
                    {
                        this.health -= (int)entityData.Value;
                        if (this.health <= this.minimumHealth)
                        {
                            this.state = PlayerState.dead;
                        }

                        collided = true;
                    }

                    break;
                case InteractiveEntityType.PowerUp:
                    this.ResetPowerUpModifiers();
                    if (entityData.PowerUpType == PowerUpType.Jump)
                    {
                        this.jumpImpulse *= StickMan.JumpPowerModifier;
                    }

                    this.activePowerUp = entityData.PowerUpType;
                    this.powerUpTimer = 0.0f;
                    this.powerUpLength = entityData.Value;
                    collided = false;
                    break;
                case InteractiveEntityType.Bonus:
                    this.Score += (int)entityData.Value;
                    collided = false;
                    break;
                default:
                    break;
            }

            return collided;
        }

        /// <summary>
        /// Collision event handler for the stick man's wheel.
        /// </summary>
        /// <param name="fixtureOne">The first colliding fixture.</param>
        /// <param name="fixtureTwo">The second colliding fixture.</param>
        /// <param name="contact">The contact object.</param>
        /// <returns>Whether the collision was accepted or not.</returns>
        private bool CollisionHandlerWheel(Fixture fixtureOne, Fixture fixtureTwo, Contact contact)
        {
            bool collided = true;
            if (this.InCart)
            {
                collided = false;
            }
            else
            {
                switch (fixtureTwo.CollisionCategories)
                {
                    case EntityConstants.PlatformCategory:
                        collided = false;
                        if (this.wheelCollisionDisabled != true && this.state != PlayerState.jumping && fixtureOne.Body.Position.Y < fixtureTwo.Body.Position.Y)
                        {
                            this.Land();
                            this.maximumHorizontalVelocity = this.standardHorizontalVelocity;
                            this.onFloor = false;
                            collided = true;
                        }

                        break;
                    case EntityConstants.FloorCategory:
                        this.Land();
                        this.maximumHorizontalVelocity = this.floorHorizontalVelocity;
                        this.onFloor = true;
                        collided = true;
                        break;
                    case EntityConstants.InteractiveEntityCategory:
                        InteractiveEntityUserData entityData = (InteractiveEntityUserData)fixtureTwo.Body.UserData;
                        if (entityData.IsActive)
                        {
                            collided = this.CollideWithInteractiveEntity(entityData);
                            entityData.IsActive = false;
                            fixtureTwo.Body.UserData = entityData;
                        }
                        else
                        {
                            collided = false;
                        }

                        break;
                    case EntityConstants.ScrollingDeathCategory:
                        collided = true;
                        this.health = this.minimumHealth;
                        this.state = PlayerState.dead;
                        break;
                    case EntityConstants.MineCartCategory:
                        if (this.state != PlayerState.jumping)
                        {
                            this.LandInCart(fixtureTwo.Body);
                        }

                        collided = false;
                        break;
                    default:
                        collided = true;
                        break;
                }
            }

            return collided;
        }

        /// <summary>
        /// Collision event handler for the crouching representation of the stick man's body.
        /// </summary>
        /// <param name="fixtureOne">The first colliding fixture.</param>
        /// <param name="fixtureTwo">The second colliding fixture.</param>
        /// <param name="contact">The contact object.</param>
        /// <returns>Whether the collision was accepted or not.</returns>
        private bool CollisionHandlerSmallBody(Fixture fixtureOne, Fixture fixtureTwo, Contact contact)
        {
            bool collided = true;
            if (this.state != PlayerState.crouching)
            {
                collided = false;
            }
            else
            {
                switch (fixtureTwo.CollisionCategories)
                {
                    case EntityConstants.InteractiveEntityCategory:
                        InteractiveEntityUserData entityData = (InteractiveEntityUserData)fixtureTwo.Body.UserData;
                        if (entityData.IsActive)
                        {
                            collided = this.CollideWithInteractiveEntity(entityData);
                            entityData.IsActive = false;
                            fixtureTwo.Body.UserData = entityData;
                        }
                        else
                        {
                            collided = false;
                        }

                        break;
                    case EntityConstants.ScrollingDeathCategory:
                        collided = true;
                        this.health = this.minimumHealth;
                        this.state = PlayerState.dead;
                        break;
                    default:
                        collided = true;
                        break;
                }
            }

            return collided;
        }

        /// <summary>
        /// Collision event handler for the standing stick man's body.
        /// </summary>
        /// <param name="fixtureOne">The first colliding fixture.</param>
        /// <param name="fixtureTwo">The second colliding fixture.</param>
        /// <param name="contact">The contact object.</param>
        /// <returns>Whether the collision was accepted or not.</returns>
        private bool CollisionHandlerFullBody(Fixture fixtureOne, Fixture fixtureTwo, Contact contact)
        {
            bool collided = true;
            if (this.state == PlayerState.crouching)
            {
                collided = false;
            }
            else
            {
                switch (fixtureTwo.CollisionCategories)
                {
                    case EntityConstants.PlatformCategory:
                        if (this.state == PlayerState.jumping || this.jumpingDown)
                        {
                            collided = false;
                        }
                        else
                        {
                            collided = true;
                        }

                        break;
                    case EntityConstants.InteractiveEntityCategory:
                        InteractiveEntityUserData entityData = (InteractiveEntityUserData)fixtureTwo.Body.UserData;
                        if (entityData.IsActive)
                        {
                            collided = this.CollideWithInteractiveEntity(entityData);
                            entityData.IsActive = false;
                            fixtureTwo.Body.UserData = entityData;
                        }
                        else
                        {
                            collided = false;
                        }

                        break;
                    case EntityConstants.ScrollingDeathCategory:
                        collided = true;
                        this.health = this.minimumHealth;
                        this.state = PlayerState.dead;
                        break;
                    case EntityConstants.MineCartCategory:
                        if (!this.InCart && this.state != PlayerState.jumping)
                        {
                            this.LandInCart(fixtureTwo.Body);
                        }

                        collided = false;
                        break;
                    default:
                        collided = true;
                        break;
                }
            }

            return collided;
        }
    }
}
