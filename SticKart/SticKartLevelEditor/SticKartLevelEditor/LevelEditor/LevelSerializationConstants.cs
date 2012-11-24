// -----------------------------------------------------------------------
// <copyright file="LevelSerializationConstants.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.LevelEditor
{
    /// <summary>
    /// Defines constants used by the <see cref="Level"/> class for serialization.
    /// </summary>
    public class LevelSerializationConstants
    {
        /// <summary>
        /// The name of the xna content element tag.
        /// </summary>
        public const string XnaContentTag = "XnaContent";

        /// <summary>
        /// The name of the asset element tag.
        /// </summary>
        public const string AssetTag = "Asset";

        /// <summary>
        /// The name of an item element tag.
        /// </summary>
        public const string ItemTag = "Item";

        /// <summary>
        /// The name of a position element tag.
        /// </summary>
        public const string PositionTag = "Position";

        /// <summary>
        /// The name of a length element tag.
        /// </summary>
        public const string LengthTag = "Length";

        /// <summary>
        /// The name of the type attribute, within the asset element.
        /// </summary>
        public const string TypeAttributeName = "Type";
                
        /// <summary>
        /// The content of the tpye field for a vector array.
        /// </summary>
        public const string VectorArrayTypeField = "Microsoft.Xna.Framework.Vector2[]";
                
        /// <summary>
        /// The content of the tpye field for a platform description array.
        /// </summary>
        public const string PlatformArrayTypeField = "SticKart.PlatformDescription[]";
    }
}
