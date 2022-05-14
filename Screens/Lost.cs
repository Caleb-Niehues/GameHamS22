using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using TimeGame.Scoring;

namespace TimeGame.Screens
{
    public static class Lost
    {
        static Texture2D pixel;
        static SpriteFont bangers;
        //static SoundEffect lose;
        //static bool showing = false;

        public static int Width = 710;
        public static int Height = 430;

        public static void LoadContent(ContentManager content)
        {
            pixel = content.Load<Texture2D>("PIXEL");
            bangers = content.Load<SpriteFont>("bangers");
            //lose = content.Load<SoundEffect>("Explosion7");
        }
        public static void Draw(SpriteBatch spriteBatch, Leaderboard leaderboard)
        {
            //if (!showing)
            //{
            //    lose.Play();
            //    showing = true;
            //}

            var tmp = leaderboard.Formatted();

            var first = tmp[0];
            var second = tmp[1];
            var third = tmp[2];
            var fourth = tmp[3];
            var fifth = tmp[4];

            spriteBatch.Draw(pixel, new Rectangle(0, 0, Width + 300, Height + 200), Color.DarkRed * (float).8);
            spriteBatch.DrawString(bangers, "YOU LOSE", new Vector2(290, 40), Color.White, 0, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0);

            spriteBatch.DrawString(bangers, "LEADERBOARD", new Vector2(270, 100), Color.White, 0, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0);

            spriteBatch.DrawString(bangers, first, new Vector2(230, 160), Color.White, 0, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, second, new Vector2(230, 210), Color.White, 0, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, third, new Vector2(230, 260), Color.White, 0, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, fourth, new Vector2(230, 310), Color.White, 0, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, fifth, new Vector2(230, 360), Color.White, 0, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0);


            spriteBatch.DrawString(bangers, "Press ESC or BACK to go back to main menu", new Vector2(150, 440), Color.White, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
        }
    }
}