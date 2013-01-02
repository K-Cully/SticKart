// -----------------------------------------------------------------------
// <copyright file="SceneryRenderer.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Game.Level;

    /// <summary>
    /// A wrapper for scenery rendering logic.
    /// </summary>
    public static class SceneryRenderer
    {
        /// <summary>
        /// Draws a single segment of terrain below an edge.
        /// The sprite batch's <code>Begin()</code> method must be called before this.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to render with.</param>
        /// <param name="texture">The texture to render to the terrain.</param>
        /// <param name="xTextureStart">The mapping offset, along x, from the texture.</param>
        /// <param name="firstTerrainPoint">The coordinate of the first point on the terrain edge.</param>
        /// <param name="endTerrainPoint">The coordinate of the end point on the terrain edge.</param>
        /// <param name="yBottom">The y coordinate of the bottom of the rendered terrain.</param>
        private static void DrawTerrainSegment(SpriteBatch spriteBatch, Texture2D texture, ref int xTextureStart, Vector2 firstTerrainPoint, Vector2 endTerrainPoint, float yBottom)
        {
            Vector2 edge = endTerrainPoint - firstTerrainPoint;
            int end = (int)edge.X;
            float stepRate = edge.Y != 0.0f ? edge.X / edge.Y : 0.0f;
            Vector2 offset = -Camera2D.OffsetPosition;
            Rectangle sourceRectangle = new Rectangle();
            Rectangle destinationRectangle = new Rectangle();
            for (int count = 0; count <= end; ++count, xTextureStart += 1, offset.X += 1.0f)
            {
                if (xTextureStart >= texture.Width)
                {
                    xTextureStart -= texture.Width;
                }

                sourceRectangle = new Rectangle(xTextureStart, 0, 1, texture.Height);
                destinationRectangle = new Rectangle((int)(firstTerrainPoint.X + offset.X), (int)(firstTerrainPoint.Y + offset.Y), 1, (int)(yBottom - (firstTerrainPoint.Y + offset.Y)));
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
                offset.Y += stepRate != 0.0f ? 1.0f / stepRate : 0.0f;
            }

            if (xTextureStart >= texture.Width)
            {
                xTextureStart -= texture.Width;
            }
        }

        /// <summary>
        /// Draws the edge based terrain to the screen.
        /// The sprite batch's <code>Begin()</code> method must be called before this.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to render with.</param>
        /// <param name="texture">The texture to apply to the terrain.</param>
        /// <param name="terrainEdges">The list of terrain edges.</param>
        /// <param name="terrainBottomY">The y coordinate of the bottom of the rendered terrain.</param>
        public static void DrawTerrain(SpriteBatch spriteBatch, Texture2D texture, List<VisualEdge> terrainEdges, float terrainBottomY)
        {
            int xTextureMapOffset = 0;
            foreach (VisualEdge edge in terrainEdges)
            {
                if (edge.Position.Y > 0.0f)
                {
                    SceneryRenderer.DrawTerrainSegment(spriteBatch, texture, ref xTextureMapOffset, edge.StartPoint, edge.EndPoint, -Camera2D.OffsetPosition.Y + terrainBottomY);
                }
            }
        }
    }
}
