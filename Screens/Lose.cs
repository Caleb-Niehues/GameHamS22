using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TimeGame.Screens
{
    public static class Lose
    {
        static Texture2D pixel;
        static SpriteFont bangers;
        //static SoundEffect lose;
        //static bool showing = false;

        public static void LoadContent(ContentManager content)
        {
            pixel = content.Load<Texture2D>("PIXEL");
            bangers = content.Load<SpriteFont>("bangers");
            //lose = content.Load<SoundEffect>("Explosion7");
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            //if (!showing)
            //{
            //    lose.Play();
            //    showing = true;
            //}

            spriteBatch.Draw(pixel, new Rectangle(0, 0, 760, 480), Color.DarkRed * (float).8);
            spriteBatch.DrawString(bangers, "YOU LOSE", new Vector2(200, 50), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, "Press ESC or BACK to go back to main menu", new Vector2(100, 250), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}