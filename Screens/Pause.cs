using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TimeGame.Screens
{
    public static class Pause
    {
        //static Texture2D pixel;
        static SpriteFont Bangers;
        public static void LoadContent(ContentManager Content)
        {
            //pixel = Content.Load<Texture2D>("PIXEL");
            Bangers = Content.Load<SpriteFont>("Bangers");
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(pixel, new Rectangle(0, 0, 760, 480), Color.Black); //change Color.Black to Color.Black * 0.8f
            spriteBatch.DrawString(Bangers, "PAUSE", new Vector2(200, 50), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press ENTER or START to continue", new Vector2(100, 150), Color.Green, 0, new Vector2(0, 0), .5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Bangers, "Press ESC or BACK to go back to main menu", new Vector2(100, 250), Color.Red, 0, new Vector2(0, 0), .5f, SpriteEffects.None, 0);
        }
    }
}