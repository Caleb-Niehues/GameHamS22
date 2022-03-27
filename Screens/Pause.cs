using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TimeGame.Screens
{
    public static class Pause
    {
        static Texture2D pixel;
        static SpriteFont Bangers;
        public static void LoadContent(ContentManager Content)
        {
            pixel = Content.Load<Texture2D>("PIXEL");
            Bangers = Content.Load<SpriteFont>("Bangers");
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, new Rectangle(0, 0, 760, 480), Color.Black * 0.8f); //change Color.Black to Color.Black * 0.8f
            spriteBatch.DrawString(Bangers, "PAUSE", new Vector2(200, 50), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press ESC to continue", new Vector2(100, 150), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press 1 to upgrade power ups", new Vector2(100, 200), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press 2 to upgrade pistol", new Vector2(100, 250), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press 3 to upgrade shotgun", new Vector2(100, 300), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press 4 to upgrade sniper", new Vector2(100, 350), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}