namespace SticKart.Game.Entities
{
    using Display;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics.Joints;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    
    /// <summary>
    /// Defines an in game player representation.
    /// </summary>
    public class StickMan
    {
        /// <summary>
        /// The physics body at the top of the stickman.
        /// This is used for detecting collisions when standing.
        /// </summary>
        private Body topBody;

        /// <summary>
        /// The physics body in the middle of the stickman.
        /// This is used for detecting collisions when crouching or standing.
        /// </summary>
        private Body middleBody;

        /// <summary>
        /// The physics body at the bottom of the stickman.
        /// This is used to control the movement of the stickman and to allow the stickman to traverse bumpy terrain easily.
        /// </summary>
        private Body wheelBody;

        /// <summary>
        /// A joint connecting the <see cref="topBody"/> to the <see cref="middleBody"/>.
        /// </summary>
        private WeldJoint upperBodyJoint;

        /// <summary>
        /// A motor joint connecting the <see cref="middleBody"/> to the <see cref="wheelBody"/>.
        /// </summary>
        private RevoluteJoint lowerBodyJoint;

        /// <summary>
        /// A joint which locks the angle of the stickman to prevent it falling over.
        /// </summary>
        private FixedAngleJoint angleUprightJoint;

        /// <summary>
        /// The sprite which represents the stickman's standing pose.
        /// </summary>
        private Sprite standingSprite; // TODO: add animated sprite class and other sprites

        /// <summary>
        /// The minimum velocity at which the stickman can move horizontally.
        /// </summary>
        private float minimumHorizontalVelocity;

        /// <summary>
        /// The maximum velocity at which the stickman can move horizontally.
        /// </summary>
        private float maximumHorizontalVelocity;

        /// <summary>
        /// The current velocity at which the stickman is moving horizontally.
        /// </summary>
        private float horizontalVelocity;

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
            this.horizontalVelocity = 0.0f;
            this.maximumHorizontalVelocity = maximumHorizontalSpeed;
            this.minimumHealth = 0;
            this.health = maximumHealth;
            this.maximumHealth = maximumHealth;
            this.jumpImpulse = jumpImpulse;
            this.inCart = false;
            this.state = PlayerState.standing;
            this.onFloor = false;

            this.standingSprite = new Sprite();
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            
            this.SetUpPhysicsObjects(ref physicsWorld);
        }

        /// <summary>
        /// Initializes and loads the textures for all of the sprites in a StickMan object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.standingSprite.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.StickManStanding);
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
            this.topBody.BodyType = BodyType.Dynamic;
            this.topBody.Position = ConvertUnits.ToSimUnits(new Vector2(0.0f, -this.standingSprite.Height / 4.0f));
            this.topBody.Restitution = restitution;
            Vertices playerTopBox = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(this.standingSprite.Width / 2.0f), ConvertUnits.ToSimUnits(this.standingSprite.Height / 4.0f)); // Top box is half of the standing height.
            PolygonShape playerTopShape = new PolygonShape(playerTopBox, density);
            Fixture playerTopFixture = this.topBody.CreateFixture(playerTopShape);

            // Middle body for crouching
            this.middleBody = BodyFactory.CreateBody(physicsWorld);
            this.middleBody.BodyType = BodyType.Dynamic;
            this.middleBody.Position = ConvertUnits.ToSimUnits(new Vector2(0.0f, this.standingSprite.Height / 8.0f));
            this.middleBody.Restitution = restitution;
            Vertices playerMiddleBox = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(this.standingSprite.Width / 2.0f), ConvertUnits.ToSimUnits(this.standingSprite.Height / 8.0f)); // Lower box is a quater of the standing height.
            PolygonShape playerMiddleShape = new PolygonShape(playerMiddleBox, density);
            Fixture playerMiddleFixture = this.middleBody.CreateFixture(playerMiddleShape);

            // Wheel for movement
            this.wheelBody = BodyFactory.CreateBody(physicsWorld);
            this.wheelBody.BodyType = BodyType.Dynamic;
            this.wheelBody.Position = ConvertUnits.ToSimUnits(new Vector2(0.0f, this.standingSprite.Height / 4.0f));
            this.wheelBody.Restitution = restitution;
            CircleShape playerBottomShape = new CircleShape(ConvertUnits.ToSimUnits(this.standingSprite.Width / 2.0f), density);
            Fixture playerBottomFixture = this.wheelBody.CreateFixture(playerBottomShape);

            // Joints to connect the bodies.
            this.upperBodyJoint = new WeldJoint(this.topBody, this.middleBody, ConvertUnits.ToSimUnits(Vector2.Zero), ConvertUnits.ToSimUnits(Vector2.Zero));
            physicsWorld.AddJoint(this.upperBodyJoint);

            this.angleUprightJoint = new FixedAngleJoint(this.middleBody); // TODO: Set target angle.
            physicsWorld.AddJoint(this.angleUprightJoint);

            this.lowerBodyJoint = new RevoluteJoint(this.middleBody, this.wheelBody, ConvertUnits.ToSimUnits(new Vector2(0.0f, this.standingSprite.Height / 8.0f)), ConvertUnits.ToSimUnits(Vector2.Zero));
            this.lowerBodyJoint.MotorSpeed = 0.0f;
            this.lowerBodyJoint.MaxMotorTorque = 20.0f; // TODO: set correctly.
            this.lowerBodyJoint.MotorEnabled = true;
            physicsWorld.AddJoint(this.lowerBodyJoint);
        }
    }
}
