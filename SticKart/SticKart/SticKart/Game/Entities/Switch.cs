﻿// -----------------------------------------------------------------------
// <copyright file="Switch.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Entities
{
    using Audio;
    using Display;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    
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
        /// The animated sprite to render for a switch.
        /// </summary>
        private AnimatedSprite sprite;

        /// <summary>
        /// The sound to play when the switch is tripped.
        /// </summary>
        private SoundEffect sound;

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
            this.sprite = new AnimatedSprite();
            this.activated = false;
            this.mineCart = mineCart;
            this.InitializeAndLoadSprites(spriteBatch, contentManager);
            this.SetUpPhysics(ref physicsWorld, position);
        }

        /// <summary>
        /// Updates a switch entity.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            if (!this.activated)
            {
                if ((bool)this.physicsBody.UserData == true)
                {
                    AudioManager.PlayEffect(this.sound);
                    this.activated = true;
                    this.mineCart.Activate();
                }
            }
            else
            {
                this.sprite.Update(gameTime);
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
            this.sprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Switch, 7, 0.03f, false);
            this.sound = contentManager.Load<SoundEffect>(EntityConstants.SoundEffectsFolderPath + EntityConstants.Switch);
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
            this.physicsBody = BodyFactory.CreateRectangle(physicsWorld, ConvertUnits.ToSimUnits(this.sprite.FrameWidth), ConvertUnits.ToSimUnits(this.sprite.FrameHeight), density, ConvertUnits.ToSimUnits(position));
            this.physicsBody.BodyType = BodyType.Static;
            this.physicsBody.Restitution = restitution;
            this.physicsBody.CollisionCategories = EntityConstants.SwitchCategory;
            this.physicsBody.CollidesWith = EntityConstants.StickManCategory;
            this.physicsBody.UserData = this.activated;
        }
    }
}
