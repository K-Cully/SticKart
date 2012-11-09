namespace SticKart.Game.Entities
{
    using Display;
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
        #region members

        /// <summary>
        /// The physics body at the top of the stickman.
        /// This is used for detecting collisions when standing.
        /// </summary>
        private Body topBody;

        /// <summary>
        /// The offset from the center of the stickman to the top body, in display space.
        /// </summary>
        private Vector2 topBodyOffset;

        /// <summary>
        /// The physics body in the middle of the stickman.
        /// This is used for detecting collisions when crouching or standing.
        /// </summary>
        private Body middleBody;

        /// <summary>
        /// The offset from the center of the stickman to the middle body, in display space.
        /// </summary>
        private Vector2 middleBodyOffset;

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
        /// A joint connecting the <see cref="topBody"/> to the <see cref="middleBody"/>.
        /// </summary>
        private WeldJoint upperBodyJoint;

        /// <summary>
        /// A motor joint connecting the <see cref="middleBody"/> to the <see cref="wheelBody"/>.
        /// </summary>
        private RevoluteJoint motorJoint;

        /// <summary>
        /// A joint which locks the angle of the stickman to prevent it falling over.
        /// </summary>
        private FixedAngleJoint angleUprightJoint;

        /// <summary>
        /// The sprite which represents the stickman's standing pose.
        /// </summary>
        private Sprite standingSprite; // TODO: add animated sprite class and other sprites

        private Sprite Wheelsprite; // TODO: remove

        /// <summary>
        /// The minimum velocity at which the stickman can move horizontally.
        /// </summary>
        private float minimumHorizontalVelocity;

        /// <summary>
        /// The maximum velocity at which the stickman can move horizontally.
        /// </summary>
        private float maximumHorizontalVelocity;

        /// <summary>
        /// The ideal velocity at which the stickman is moving horizontally.
        /// </summary>
        private float idealHorizontalVelocity;

        /// <summary>
        /// The acceleration which should be applied to the stickman upon receiving a run command.
        /// </summary>
        private float acceleration;

        /// <summary>
        /// The minimum health the stickman can have.
        /// </summary>
        private int minimumHealth;

        /// <summary>
        /// The maximum health the stickman can have.
        /// </summary>
        private int maximumHealth;

        /// <summary>
        /// The current health the stickman has.
        /// </summary>
        private int health;

        /// <summary>
        /// The impulse applied to the stickman when they jump.
        /// </summary>
        private float jumpImpulse;

        /// <summary>
        /// Whether the player is in the mining cart or not.
        /// </summary>
        private bool inCart;

        /// <summary>
        /// The current state the stickman is in.
        /// </summary>
        private PlayerState state;

        /// <summary>
        /// Whether the player is on the floor or not.
        /// </summary>
        private bool onFloor;

        // private PowerUp activePowerUp; // TODO: implement once power-ups are implemented

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
        public StickMan(ref World physicsWorld, float maximumHorizontalSpeed, int maximumHealth, float jumpImpulse, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.minimumHorizontalVelocity = 0.0f;
            this.idealHorizontalVelocity = 0.0f;
            this.maximumHorizontalVelocity = maximumHorizontalSpeed;
            this.minimumHealth = 0;
            this.health = maximumHealth;
            this.maximumHealth = maximumHealth;
            this.jumpImpulse = jumpImpulse;
            this.inCart = false;
            this.state = PlayerState.standing;
            this.onFloor = false;
            this.standingSprite = new Sprite();
            this.Wheelsprite = new Sprite();
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.topBodyOffset = new Vector2(0.0f, -this.standingSprite.Height / 4.0f);
            this.middleBodyOffset = new Vector2(0.0f, this.standingSprite.Height / 8.0f);
            this.wheelBodyOffset = new Vector2(0.0f, this.standingSprite.Height / 4.0f);
            this.SetUpPhysicsObjects(ref physicsWorld);
            this.acceleration = 9.0f;

            this.wheelBody.OnCollision += this.CollisionHandler;
        }

        #region accessors

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
                    return 0.5f * (ConvertUnits.ToDisplayUnits(this.wheelBody.Position) + ConvertUnits.ToDisplayUnits(this.topBody.Position));
                }
            }
        }

        #endregion

        #region initialization

        /// <summary>
        /// Initializes and loads the textures for all of the sprites in a StickMan object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            // TODO: rest of sprites
            this.standingSprite.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.StickManStanding);
            this.Wheelsprite.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.HandIcon);
        }

        /// <summary>
        /// Sets up all the physical properties of the StickMan object.
        /// </summary>
        /// <param name="physicsWorld">The physics world to set the objects up in.</param>
        private void SetUpPhysicsObjects(ref World physicsWorld)
        {
            float density = 1.25f;
            float restitution = 0.125f;

            // Upper body for standing
            this.topBody = BodyFactory.CreateBody(physicsWorld);
            Vertices playerTopBox = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(this.standingSprite.Width / 2.0f), ConvertUnits.ToSimUnits(this.standingSprite.Height / 4.0f)); // Top box is half of the standing height.
            PolygonShape playerTopShape = new PolygonShape(playerTopBox, density);
            Fixture playerTopFixture = this.topBody.CreateFixture(playerTopShape);
            this.topBody.BodyType = BodyType.Dynamic;
            this.topBody.Position = ConvertUnits.ToSimUnits(this.topBodyOffset);
            this.topBody.Restitution = restitution;

            // Middle body for crouching
            this.middleBody = BodyFactory.CreateBody(physicsWorld);
            Vertices playerMiddleBox = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(this.standingSprite.Width / 2.0f), ConvertUnits.ToSimUnits(this.standingSprite.Height / 8.0f)); // Lower box is a quater of the standing height.
            PolygonShape playerMiddleShape = new PolygonShape(playerMiddleBox, density);
            Fixture playerMiddleFixture = this.middleBody.CreateFixture(playerMiddleShape);
            this.middleBody.BodyType = BodyType.Dynamic;
            this.middleBody.IgnoreCollisionWith(this.topBody);
            this.middleBody.Position = ConvertUnits.ToSimUnits(this.middleBodyOffset);
            this.middleBody.Restitution = restitution;

            // Wheel for movement
            this.wheelBody = BodyFactory.CreateBody(physicsWorld);
            CircleShape playerBottomShape = new CircleShape(ConvertUnits.ToSimUnits(this.standingSprite.Width / 2.0f), density);
            Fixture playerBottomFixture = this.wheelBody.CreateFixture(playerBottomShape);
            this.wheelBody.BodyType = BodyType.Dynamic;
            this.wheelBody.IgnoreCollisionWith(this.middleBody);
            this.wheelBody.IgnoreCollisionWith(this.topBody);
            this.wheelBody.Position = ConvertUnits.ToSimUnits(this.wheelBodyOffset);
            this.wheelBody.Restitution = restitution;
            this.wheelBody.Friction = float.MaxValue; // TODO: set appropriatly
            this.wheelBody.CollisionCategories = Category.Cat31;

            // Joints to connect the bodies.
            this.upperBodyJoint = JointFactory.CreateWeldJoint(physicsWorld, this.topBody, this.middleBody, this.middleBody.Position);
            this.angleUprightJoint = JointFactory.CreateFixedAngleJoint(physicsWorld, this.middleBody);
            this.motorJoint = JointFactory.CreateRevoluteJoint(physicsWorld, this.middleBody, this.wheelBody, Vector2.Zero);
            this.motorJoint.MotorSpeed = 0.0f;
            this.motorJoint.MaxMotorTorque = 1000.0f; // TODO: set correctly.
            this.motorJoint.MotorEnabled = true;
        }

        #endregion
        
        /// <summary>
        /// Updates the state of the player and any time based elements of the player. 
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            // TODO: implement
            if (this.state == PlayerState.running)
            {
                this.idealHorizontalVelocity *= this.idealHorizontalVelocity < 0.2f ? 0.0f : 0.95f;

                if (this.middleBody.LinearVelocity.X > this.idealHorizontalVelocity)
                {
                    this.motorJoint.MotorSpeed -= this.acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (this.middleBody.LinearVelocity.X < this.idealHorizontalVelocity && this.motorJoint.MotorSpeed < this.maximumHorizontalVelocity * 2.0f)
                {
                    this.motorJoint.MotorSpeed += this.acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (this.idealHorizontalVelocity == 0.0f && this.middleBody.LinearVelocity.X < 1.0f)
                {
                    this.motorJoint.MotorSpeed = 0.0f;
                    this.state = PlayerState.standing;
                }
            }
            else if (this.state == PlayerState.jumping && this.middleBody.LinearVelocity.Y >= 0.0f)
            {
                this.state = PlayerState.falling;
                // TODO: switch on collisions with platforms
            }
        }

        /// <summary>
        /// Makes the the stick man run if it is not jumping.
        /// </summary>
        public void Run()
        {
            if (this.state != PlayerState.jumping && this.state != PlayerState.falling)
            {
                this.idealHorizontalVelocity += 5.0f; // TODO: set
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
                this.topBody.IsSensor = false;
                // TODO: test collisions with top box
            }
        }

        /// <summary>
        /// Makes the stick man crouch if it is standing.
        /// </summary>
        public void CrouchOrJumpDown()
        {
            if (this.state != PlayerState.crouching && this.inCart)
            {
                this.state = PlayerState.crouching;
                this.topBody.IsSensor = true;
                // TODO: test collisions with top box
            }
            else if (this.state == PlayerState.running || this.state == PlayerState.standing)
            {
                this.state = PlayerState.falling;
                // TODO: turn off collisions with the platform below, to allow movement through it (possibly indefinitly?).
            }
        }

        /// <summary>
        /// Makes the stick man jump if it is not already jumping.
        /// </summary>
        public void Jump()
        {
            if (this.state != PlayerState.jumping && this.state != PlayerState.falling)
            {
                this.middleBody.ApplyLinearImpulse(new Vector2(0.0f, this.jumpImpulse));
                this.state = PlayerState.jumping;
                this.motorJoint.MotorSpeed = 0.0f;
                // TODO: switch off collisions with platforms
            }
        }

        /// <summary>
        /// Sets the stick man's state to standing.
        /// This should be called on collision with anything.
        /// </summary>
        public void Land() // TODO: land on collision
        {
            this.state = PlayerState.standing; 
        }

        /// <summary>
        /// Draws the stick man to the screen.
        /// </summary>
        public void Draw()
        {
            switch (this.state)
            {
                case PlayerState.standing:
                    Sprite.Draw(this.standingSprite, this.Position, this.middleBody.Rotation);
                    //Sprite.Draw(this.Wheelsprite, ConvertUnits.ToDisplayUnits(this.wheelBody.Position), this.wheelBody.Rotation);
                    break;
                case PlayerState.crouching:
                    Sprite.Draw(this.Wheelsprite, ConvertUnits.ToDisplayUnits(this.wheelBody.Position), this.wheelBody.Rotation);
                    // TODO
                    break;
                case PlayerState.jumping:
                    Sprite.Draw(this.standingSprite, this.Position, this.middleBody.Rotation);
                    //Sprite.Draw(this.Wheelsprite, ConvertUnits.ToDisplayUnits(this.wheelBody.Position), this.wheelBody.Rotation);
                    // TODO
                    break;
                case PlayerState.running:
                    Sprite.Draw(this.standingSprite, this.Position, this.middleBody.Rotation);
                    //Sprite.Draw(this.Wheelsprite, ConvertUnits.ToDisplayUnits(this.wheelBody.Position), this.wheelBody.Rotation);
                    // TODO
                    break;
                case PlayerState.falling:
                    Sprite.Draw(this.standingSprite, this.Position, this.middleBody.Rotation);
                    //Sprite.Draw(this.Wheelsprite, ConvertUnits.ToDisplayUnits(this.wheelBody.Position), this.wheelBody.Rotation);
                    break;
                case PlayerState.dead:
                    //TODO
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
            this.topBody.Position = ConvertUnits.ToSimUnits(position + this.topBodyOffset);
            this.middleBody.Position = ConvertUnits.ToSimUnits(position + this.middleBodyOffset);
            this.wheelBody.Position = ConvertUnits.ToSimUnits(position + this.wheelBodyOffset);
            this.state = PlayerState.standing;
            this.idealHorizontalVelocity = 0.0f;
            this.inCart = false;
            this.onFloor = false;
            this.motorJoint.MotorSpeed = 0.0f;
            this.middleBody.Rotation = 0.0f;
            this.topBody.Rotation = 0.0f;
            this.wheelBody.Rotation = 0.0f;
            this.middleBody.LinearVelocity = Vector2.Zero;
            this.topBody.LinearVelocity = Vector2.Zero;
            this.wheelBody.LinearVelocity = Vector2.Zero;
        }

        /// <summary>
        /// Collision event handler for a stick man object.
        /// </summary>
        /// <param name="fixtureOne">The first colliding fixture.</param>
        /// <param name="fixtureTwo">The second colliding fixture.</param>
        /// <param name="contact">The contact object.</param>
        /// <returns>Whether the collision was accepted or not.</returns>
        private bool CollisionHandler(Fixture fixtureOne, Fixture fixtureTwo, Contact contact)
        {
            // TODO: implement fully
            if (fixtureOne.CollisionCategories == Category.Cat31)
            {
                if (this.motorJoint.MotorSpeed > 0.0f)
                {
                    this.state = PlayerState.running;
                }
                else
                {
                    this.state = PlayerState.standing;
                }
            }

            return true; // TODO: return false if collisions disabled
        }
    }
}
