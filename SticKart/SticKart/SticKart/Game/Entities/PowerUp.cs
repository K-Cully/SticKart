// -----------------------------------------------------------------------
// <copyright file="PowerUp.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Entities
{
    using System;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a power up entity.
    /// </summary>
    public class PowerUp : InteractiveEntity
    {
        /// <summary>
        /// The name of the power up.
        /// </summary>
        private string name;

        /// <summary>
        /// The time of effect of the power up.
        /// </summary>
        private float timeOfEffect;

        /// <summary>
        /// the type of power up.
        /// </summary>
        private PowerUpType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerUp"/> class.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        /// <param name="description">The entity description.</param>
        /// <param name="settings">The power up settings.</param>
        public PowerUp(ref World physicsWorld, SpriteBatch spriteBatch, ContentManager contentManager, InteractiveEntityDescription description, PowerUpSetting settings)
            : base(ref physicsWorld, description)
        {
            this.name = settings.Name;
            this.timeOfEffect = settings.TimeOfEffect;

            switch (this.name)
            {
                case EntityConstants.InvincibleName:
                    this.type = PowerUpType.Invincibility;
                    break;
                case EntityConstants.HealthName:
                    this.type = PowerUpType.Health;
                    break;
                case EntityConstants.JumpName:
                    this.type = PowerUpType.Jump;
                    break;
                case EntityConstants.SpeedName:
                    this.type = PowerUpType.Speed;
                    break;
                default:
                    this.type = PowerUpType.None;
                    break;
            }

            this.physicsBody.UserData = new InteractiveEntityUserData(InteractiveEntityType.PowerUp, this.timeOfEffect, this.type);
            this.InitializeAndLoad(spriteBatch, contentManager);
        }

        /// <summary>
        /// Gets the object type.
        /// </summary>
        /// <returns>The object type.</returns>
        public override Type ObjectType()
        {
            return typeof(PowerUp);
        }

        /// <summary>
        /// Initializes and loads any assets used by the power up.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        protected override void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.sprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.PowerUpFolderSubPath + this.name);
            this.sound = contentManager.Load<SoundEffect>(EntityConstants.SoundEffectsFolderPath + EntityConstants.PowerUpSound);
        }
    }
}
