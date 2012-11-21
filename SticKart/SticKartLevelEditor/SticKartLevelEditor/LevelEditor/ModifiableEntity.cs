// -----------------------------------------------------------------------
// <copyright file="ModifiableEntity.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.LevelEditor
{
    /// <summary>
    /// An enumeration of entities which can be modified by the level editor.
    /// </summary>
    public enum ModifiableEntity
    {
        /// <summary>
        /// A floor edge.
        /// </summary>
        Floor,

        /// <summary>
        /// The player's start position in the level.
        /// </summary>
        StartPosition,

        /// <summary>
        /// The position of the end of the level.
        /// </summary>
        ExitPosition,

        /// <summary>
        /// A platform.
        /// </summary>
        Platform,

        /// <summary>
        /// An invincible power-up entity.
        /// </summary>
        Invincible,

        /// <summary>
        /// A speed power-up entity.
        /// </summary>
        Speed,

        /// <summary>
        /// A jump power-up entity.
        /// </summary>
        Jump,

        /// <summary>
        /// A health power-up entity.
        /// </summary>
        Health,

        /// <summary>
        /// A fire obstacle.
        /// </summary>
        Fire,

        /// <summary>
        /// A rock obstacle.
        /// </summary>
        Rock,

        /// <summary>
        /// A spike obstacle.
        /// </summary>
        Spike
    }
}
