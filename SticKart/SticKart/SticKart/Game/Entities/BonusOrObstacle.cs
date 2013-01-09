// -----------------------------------------------------------------------
// <copyright file="BonusOrObstacle.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Entities
{
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a bonus or an obstacle.
    /// </summary>
    public class BonusOrObstacle : InteractiveEntity
    {
        /// <summary>
        /// The name of the entity.
        /// </summary>
        private string name;

        /// <summary>
        /// The value of the entity.
        /// </summary>
        private float value;

        /// <summary>
        /// The type of the entity.
        /// </summary>
        private InteractiveEntityType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="BonusOrObstacle"/> class.
        /// </summary>
        /// <param name="physicsWorld">The physics world.</param>
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        /// <param name="description">The entity description.</param>
        /// <param name="setting">The entity's settings.</param>
        public BonusOrObstacle(ref World physicsWorld, SpriteBatch spriteBatch, ContentManager contentManager, InteractiveEntityDescription description, ObstacleOrBonusSetting setting)
            : base(ref physicsWorld, description)
        {            
            this.type = setting.IsBonus ? InteractiveEntityType.Bonus : InteractiveEntityType.Obstacle;
            this.name = setting.Name;
            this.value = setting.Value;
            this.physicsBody.UserData = new InteractiveEntityUserData(this.type, this.value);
            this.InitializeAndLoad(spriteBatch, contentManager);
        }

        /// <summary>
        /// Initializes and loads any assets used by the entity.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        protected override void InitializeAndLoad(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            string path = string.Empty;
            if (this.type == InteractiveEntityType.Bonus)
            {
                path = EntityConstants.SpritesFolderPath + EntityConstants.BonusFolderSubPath;
                this.sound = contentManager.Load<SoundEffect>(EntityConstants.SoundEffectsFolderPath + EntityConstants.BonusSound);
            }
            else
            {
                path = EntityConstants.SpritesFolderPath + EntityConstants.ObstacleFolderSubPath;
                this.sound = contentManager.Load<SoundEffect>(EntityConstants.SoundEffectsFolderPath + this.name);
            }

            this.sprite.InitializeAndLoad(spriteBatch, contentManager, path + this.name);
        }
    }
}
