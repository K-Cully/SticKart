// -----------------------------------------------------------------------
// <copyright file="Camera2D.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Display
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a camera which can be used to render sprites with an offset.
    /// </summary>
    public static class Camera2D
    {
        /// <summary>
        /// A value indicating whether the camera has been initialized or not.
        /// </summary>
        private static bool initialized = false;

        /// <summary>
        /// The dimensions of the actual render area.
        /// </summary>
        private static Vector2 displayDimensions;

        /// <summary>
        /// The rectangle, inside which sprites should be rendered.
        /// </summary>
        private static Rectangle drawSpace;

        /// <summary>
        /// The offset of the culling rectangle from the camera.
        /// </summary>
        private static Point drawSpaceOffset;

        /// <summary>
        /// The current offset from the origin.
        /// </summary>
        private static Vector2 offset;

        /// <summary>
        /// Gets the offset position of the camera.
        /// </summary>
        public static Vector2 OffsetPosition 
        {
            get
            {
                return Camera2D.offset;
            }
        }

        /// <summary>
        /// Sets the x position of the camera.
        /// </summary>
        public static float X
        {
            set
            {
                Camera2D.offset.X = value;
                Camera2D.drawSpace.Location = new Point((int)Camera2D.offset.X + Camera2D.drawSpaceOffset.X, (int)Camera2D.offset.Y + Camera2D.drawSpaceOffset.Y);
            }
        }

        /// <summary>
        /// Sets the Y position of the camera.
        /// </summary>
        public static float Y
        {
            set
            {
                Camera2D.offset.Y = value;
                Camera2D.drawSpace.Location = new Point((int)Camera2D.offset.X + Camera2D.drawSpaceOffset.X, (int)Camera2D.offset.Y + Camera2D.drawSpaceOffset.Y);
            }
        }

        /// <summary>
        /// Initializes the camera.
        /// </summary>
        /// <param name="displayDimensions">The dimensions of the render area.</param>
        /// <returns>A value indicating whether the camera was initialized or not.</returns>
        public static bool Initialize(Vector2 displayDimensions)
        {
            if (Camera2D.initialized != true)
            {
                Camera2D.displayDimensions = displayDimensions;
                Camera2D.initialized = true;     
                Camera2D.Reset();           
            }

            return Camera2D.initialized;
        }

        /// <summary>
        /// Resets the camera's position.
        /// </summary>
        /// <returns>A value indicating whether the camera was reset or not.</returns>
        public static bool Reset()
        {
            if (Camera2D.initialized == true)
            {
                Camera2D.offset = Vector2.Zero;
                Camera2D.drawSpaceOffset = new Point(-(int)(Camera2D.displayDimensions.X * 0.2f), -(int)(Camera2D.displayDimensions.Y * 0.2f));
                Camera2D.drawSpace = new Rectangle(Camera2D.drawSpaceOffset.X, Camera2D.drawSpaceOffset.Y, (int)(Camera2D.displayDimensions.X * 1.4f), (int)(Camera2D.displayDimensions.Y * 1.4f));
            }

            return Camera2D.initialized;
        }

        /// <summary>
        /// Updates the position of the camera.
        /// </summary>
        /// <param name="velocity">The velocity to apply to the camera.</param>
        /// <param name="gameTime">The game time.</param>
        public static void Update(Vector2 velocity, GameTime gameTime)
        {
            Camera2D.offset += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Camera2D.drawSpace.Location = new Point((int)Camera2D.offset.X + Camera2D.drawSpaceOffset.X, (int)Camera2D.offset.Y + Camera2D.drawSpaceOffset.Y);            
        }

        /// <summary>
        /// Draws a sprite offset by the camera's position.
        /// </summary>
        /// <param name="sprite">The sprite to draw.</param>
        /// <param name="position">The position of the sprite.</param>
        /// <param name="rotation">The rotation of the sprite.</param>
        /// <param name="colour">The colour to draw the sprite in.</param>
        /// <param name="scale">The scale of the sprite.</param>
        /// <param name="effect">The effect to apply to the sprite before drawing.</param>
        /// <param name="layerDepth">The layer depth of the sprite.</param>
        public static void Draw(Sprite sprite, Vector2 position, float rotation, Color colour, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None, float layerDepth = 1.0f)
        {
            if (layerDepth > 1.0f || Camera2D.drawSpace.Contains((int)position.X, (int)position.Y))
            {
                Sprite.Draw(sprite, position - ((1.0f / layerDepth) * Camera2D.offset), rotation, colour, scale, effect, 1.0f);
            }
        }

        /// <summary>
        /// Draws a sprite offset by the camera's position.
        /// </summary>
        /// <param name="sprite">The sprite to draw.</param>
        /// <param name="position">The position of the sprite.</param>
        /// <param name="rotation">The rotation of the sprite.</param>
        public static void Draw(Sprite sprite, Vector2 position, float rotation)
        {
             if (Camera2D.drawSpace.Contains((int)position.X, (int)position.Y))
             {
                Sprite.Draw(sprite, position - Camera2D.offset, rotation);
             }
        }
    }
}
