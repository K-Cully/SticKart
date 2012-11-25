// -----------------------------------------------------------------------
// <copyright file="InteractiveEntityUserData.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Entities
{
    /// <summary>
    /// Defines the user data for an interactive entity.
    /// </summary>
    public class InteractiveEntityUserData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveEntityUserData"/> class.
        /// </summary>
        public InteractiveEntityUserData()
        {
            this.EntityType = InteractiveEntityType.Bonus;
            this.PowerUpType = PowerUpType.None;
            this.Value = 0.0f;
            this.IsActive = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveEntityUserData"/> class.
        /// </summary>
        /// <param name="entityType">The type of entity.</param>
        /// <param name="value">The value of the entity.</param>
        /// <param name="powerType">The power up type of the entity, if any.</param>
        public InteractiveEntityUserData(InteractiveEntityType entityType, float value, PowerUpType powerType = PowerUpType.None)
        {
            this.EntityType = entityType;
            this.PowerUpType = powerType;
            this.Value = value;
            this.IsActive = true;
        }

        /// <summary>
        /// Gets or sets the entity type.
        /// </summary>
        public InteractiveEntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the entity value.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Gets or sets the power up type.
        /// </summary>
        public PowerUpType PowerUpType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active or not.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
