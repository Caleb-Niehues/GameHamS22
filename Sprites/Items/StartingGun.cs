using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TimeGame.Sprites.Items
{
    public class StartingGun : Arm
    {

        //private Texture2D texture2;

     
        public StartingGun(Vector2 position, Vector2 origin)
        {
            BodyOrigin = origin;
            BodyPosition = position;
            this.Position = BodyPosition + BodyOrigin;
            this.pixelWidth = 64;
            this.pixelHeight = 16;
        }
        public void CalculateBarrel()
        {
            this.BarrelEnd = this.Position + new Vector2((pixelWidth - this.pixelHeight / 2) * (float)Math.Cos(rotationdir), (pixelWidth - this.pixelHeight / 2) * (float)Math.Sin(rotationdir));
        }
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ArmPistol");
            //texture2 = content.Load<Texture2D>("PIXEL");
        }

        public override void Update(GameTime gameTime)
        {
            this.Position = BodyPosition + BodyOrigin;
            //https://stackoverflow.com/questions/7339574/xna-rotating-a-sprite-to-face-the-cursor-exactly
            MouseState ms = Mouse.GetState(); ;
            rotationdir = (float)Math.Atan2((ms.Y - Position.Y), (ms.X - Position.X));
            //Debug.WriteLine(rotationdir);
            CalculateBarrel();
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var source = new Rectangle(0, ArmPowerUp * this.pixelHeight, this.pixelWidth, this.pixelHeight);
            spriteBatch.Draw(texture, Position, source, Color.White, rotationdir, new Vector2(this.pixelHeight / 2, this.pixelHeight / 2), 1, SpriteEffects.None, 0);
            //spriteBatch.Draw(texture2, BarrelEnd, new Rectangle(0, 0, 1, 1), Color.Red, 0, new Vector2(0, 0), 5, SpriteEffects.None, 0);
        }
    }
}
