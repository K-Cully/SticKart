namespace SticKart.Game.Entities
{
    using System.Collections.Generic;
    using Display;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a platform entity.
    /// </summary>
    public class Platform
    {
        /// <summary>
        /// The list of sprite offsets for drawing a platform longer than the platform image.
        /// </summary>
        private List<Vector2> spriteOffsets;

        /// <summary>
        /// The platform's sprite.
        /// </summary>
        private Sprite sprite;

        /// <summary>
        /// The platform's physics body.
        /// </summary>
        private Body body;

        /// <summary>
        /// Initializes a new instance of the <see cref="Platform"/> class.
        /// </summary>
        /// <param name="description">The platform description.</param>
        /// <param name="physicsWorld">The physics world.</param>
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        public Platform(PlatformDescription description, ref World physicsWorld, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.spriteOffsets = new List<Vector2>();
            this.sprite = new Sprite();
            this.InitializeAndLoadSprites(spriteBatch, contentManager, description);
            this.SetUpPhysics(ref physicsWorld, description);
        }

        /// <summary>
        /// Disposes of the body associated with the platform.
        /// </summary>
        /// <param name="physicsWorld">The physics world containing the body.</param>
        public void Dispose(ref World physicsWorld)
        {
            physicsWorld.RemoveBody(this.body);
        }

        /// <summary>
        /// Draws a platform.
        /// </summary>
        public void Draw()
        {
            Vector2 position = ConvertUnits.ToDisplayUnits(this.body.Position);
            foreach (Vector2 offset in this.spriteOffsets)
            {
                Camera2D.Draw(this.sprite, position + offset, 0.0f);
            }
        }

        /// <summary>
        /// Initializes and loads the textures for the sprites in a platform object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for rendering the sprites.</param>
        /// <param name="contentManager">The content manager to use for loading the sprites.</param>
        /// <param name="description">The description of the platform object.</param>
        private void InitializeAndLoadSprites(SpriteBatch spriteBatch, ContentManager contentManager, PlatformDescription description)
        {
            this.sprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Platform);
            this.spriteOffsets.Add(Vector2.Zero);
            if (description.Length > this.sprite.Width)
            {
                int count = 0;
                // Useable area on middle sprites (removing rounded ends)
                float offset = this.sprite.Width - this.sprite.Height;
                float halfLeftOver = (description.Length - offset) * 0.5f;
                // Leftover greater than useable area on end sprites
                while (halfLeftOver > this.sprite.Width - (this.sprite.Height / 2.0f))
                {
                    count++;
                    this.spriteOffsets.Add(new Vector2(offset * count, 0.0f));
                    this.spriteOffsets.Add(new Vector2(-offset * count, 0.0f));
                    halfLeftOver -= offset;
                }

                // Fill in ends
                if (halfLeftOver > 0.0f)
                {
                    this.spriteOffsets.Add(new Vector2((description.Length / 2.0f) - (this.sprite.Width / 2.0f), 0.0f));
                    this.spriteOffsets.Add(new Vector2(-(description.Length / 2.0f) + (this.sprite.Width / 2.0f), 0.0f));
                }
            }
        }

        /// <summary>
        /// Sets up the physics body for a platform.
        /// </summary>
        /// <param name="physicsWorld">The physics world to create the body in.</param>
        /// <param name="description">The description of the platform.</param>
        private void SetUpPhysics(ref World physicsWorld, PlatformDescription description)
        {
            this.body = BodyFactory.CreateRoundedRectangle(physicsWorld, ConvertUnits.ToSimUnits(description.Length), ConvertUnits.ToSimUnits(this.sprite.Height), ConvertUnits.ToSimUnits(this.sprite.Height / 2.0f), ConvertUnits.ToSimUnits(this.sprite.Height / 2.0f), 8, 3.0f, ConvertUnits.ToSimUnits(description.Position));
            this.body.CollisionCategories = EntityConstants.PlatformCategory;
        }     
    }
}
