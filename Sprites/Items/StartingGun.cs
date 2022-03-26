using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeGame.Sprites.Items
{
    public class StartingGun : Item
    {
        public StartingGun(Vector2 position)
        {
            this.Position = position;
            this.pixelWidth = 64;
            this.pixelHeight = 32; 
        }
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ArmPistol");
        }

        public override void Update(GameTime gameTime)
        {
            //https://stackoverflow.com/questions/7339574/xna-rotating-a-sprite-to-face-the-cursor-exactly
            MouseState ms = Mouse.GetState(); ;
            rotationdir = (float)Math.Atan2((ms.Y - Position.Y), (ms.X - Position.X));
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var source = new Rectangle(0, 0, this.pixelWidth, this.pixelHeight);
            spriteBatch.Draw(texture, Position, source, Color.White, rotation, new Vector2(this.pixelHeight / 2, this.pixelHeight / 2), 1, SpriteEffects.None, 0);
        }
    }
}
