using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SticKart_Windows
{
    class Sprite
    {
        private Vector2 origin;
        private Texture2D texture;

        public Sprite(Texture2D texture, Vector2 origin)
        {
            this.texture = texture;
            this.origin = origin;
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            this.origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }

        public static void Draw(SpriteBatch spriteBatch, Sprite sprite, Vector2 position, float rotation, Color colour, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None, float layerDepth = 1.0f)
        {
            spriteBatch.Draw(sprite.texture, position, null, colour, rotation, sprite.origin, scale, effect, layerDepth);
        }

        public static void Draw(SpriteBatch spriteBatch, Sprite sprite, Vector2 position, float rotation)
        {
            spriteBatch.Draw(sprite.texture, position, null, Color.White, rotation, sprite.origin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
