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

            spriteBatch.Draw(pixel, new Rectangle(0, 0, Width, Height), Color.DarkRed * (float).8);
            spriteBatch.DrawString(bangers, "YOU LOSE", new Vector2(200, 50), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

            spriteBatch.DrawString(bangers, "LEADERBOARD", new Vector2(180, 100), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

            spriteBatch.DrawString(bangers, first, new Vector2(180, 120), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, second, new Vector2(180, 140), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, third, new Vector2(180, 160), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, fourth, new Vector2(180, 180), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, fifth, new Vector2(180, 200), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);


            spriteBatch.DrawString(bangers, "Press ESC or BACK to go back to main menu", new Vector2(100, 400), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }
    }
}