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
        public static void Draw(SpriteBatch spriteBatch, int score, int[] upgrades)
        {
            spriteBatch.Draw(pixel, new Rectangle(50, 50, 710, 430), Color.Black * 0.6f); //change Color.Black to Color.Black * 0.8f
            spriteBatch.DrawString(Bangers, "Shop, spend your points on upgrades: " + score, new Vector2(100, 100), Color.Gold, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press ESC to continue", new Vector2(100, 150), Color.Gold, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press Q to upgrade power ups, current level: " + upgrades[0].ToString(), new Vector2(100, 200), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press W to upgrade pistol, current level: " + upgrades[1].ToString(), new Vector2(100, 250), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press E to upgrade shotgun, current level: " + upgrades[2].ToString(), new Vector2(100, 300), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press R to upgrade sniper, current level: " + upgrades[3].ToString(), new Vector2(100, 350), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}