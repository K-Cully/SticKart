// -----------------------------------------------------------------------
// <copyright file="PositionInformer.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a graphical display of a position in 2D space.
    /// </summary>
    public static class PositionInformer
    {
        /// <summary>
        /// A value indicating whether the Position informer is initialized or not.
        /// </summary>
        private static bool initialized = false;

        /// <summary>
        /// A white pixel texture.
        /// </summary>
        private static Texture2D whitePixel = null;

        /// <summary>
        /// The region which the informer covers.
        /// </summary>
        private static Rectangle region = Rectangle.Empty;

        /// <summary>
        /// The shaded region of the informer.
        /// </summary>
        private static Rectangle shadedRegion = Rectangle.Empty;

        /// <summary>
        /// The area covered by the tracked object.
        /// </summary>
        private static Rectangle trackedObject = Rectangle.Empty;

        /// <summary>
        /// Initializes the position informer.
        /// </summary>
        /// <param name="contentManager">The game's content manager.</param>
        /// <param name="position">The position of the informer.</param>
        /// <param name="size">The size of the informer.</param>
        /// <param name="percentShaded">The percentage which should be shaded.</param>
        /// <param name="verticalShading">A value indicating whether shading should be vertical or horizontal.</param>
        public static void Initialize(ContentManager contentManager, Vector2 position, Vector2 size, float percentShaded, bool verticalShading)
        {
            percentShaded = percentShaded > 100.0f ? 100.0f : percentShaded < 0.0f ? 0.0f : percentShaded;
            percentShaded *= 0.01f;
            PositionInformer.initialized = true;
            position -= size / 2.0f;
            PositionInformer.whitePixel = contentManager.Load<Texture2D>(ContentLocations.HudPath + ContentLocations.WhitePixel);
            PositionInformer.region = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            PositionInformer.trackedObject = new Rectangle(0, 0, PositionInformer.region.Height / 10, PositionInformer.region.Height / 10);
            if (percentShaded > 0.0f)
            {
                if (verticalShading)
                {
                    PositionInformer.shadedRegion = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)(size.Y * percentShaded));
                }
                else
                {
                    PositionInformer.shadedRegion = new Rectangle((int)position.X, (int)position.Y, (int)(size.X * percentShaded), (int)size.Y);
                }
            }
        }

        /// <summary>
        /// Draws the position informer to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to render with.</param>
        /// <param name="objectXOffset">The x offset from the centre of the informer. Acceptable values are in the range -1.0f to 1.0f.</param>
        /// <param name="objectYOffset">The y offset from the centre of the informer. Acceptable values are in the range -1.0f to 1.0f.</param>
        public static void Draw(SpriteBatch spriteBatch, float objectXOffset, float objectYOffset)
        {
            if (PositionInformer.initialized)
            {
                objectXOffset = objectXOffset > 1.0f ? 1.0f : objectXOffset < -1.0f ? -1.0f : objectXOffset;
                objectYOffset = objectYOffset > 1.0f ? 1.0f : objectYOffset < -1.0f ? -1.0f : objectYOffset;
                spriteBatch.Draw(PositionInformer.whitePixel, PositionInformer.region, Color.Green);
                PositionInformer.trackedObject.X = (PositionInformer.region.X - (PositionInformer.trackedObject.Width / 2)) + (PositionInformer.region.Width / 2) + (int)(objectXOffset * (PositionInformer.region.Width / 2.0f));
                PositionInformer.trackedObject.Y = (PositionInformer.region.Y - (PositionInformer.trackedObject.Height / 2)) + (PositionInformer.region.Height / 2) + (int)(objectYOffset * (PositionInformer.region.Height / 2.0f));
                spriteBatch.Draw(PositionInformer.whitePixel, PositionInformer.shadedRegion, Color.Red);
                spriteBatch.Draw(PositionInformer.whitePixel, PositionInformer.trackedObject, Color.Blue);
            }
            else
            {
                throw new Exception("PositionInformer not initialized.");
            }
        }
    }
}
