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
    public class Pistol : Arm
    {
        public Pistol(Vector2 position, Vector2 origin, Texture2D texture)
        {
            if(texture != null)
                this.texture = texture;
            BodyOrigin = origin;
            BodyPosition = position;
            this.Position = BodyPosition + BodyOrigin;
            this.pixelWidth = 64;
            this.pixelHeight = 16;
        }
        public override void CalculateBarrel()
        {
            this.BarrelEnd = this.Position + new Vector2((pixelWidth - this.pixelHeight / 2) * (float)Math.Cos(rotationdir), (pixelWidth - this.pixelHeight / 2) * (float)Math.Sin(rotationdir));
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ArmPistol");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var source = new Rectangle(0, ArmPowerUp * this.pixelHeight, this.pixelWidth, this.pixelHeight);
            spriteBatch.Draw(texture, Position, source, Color.White, rotationdir, new Vector2(this.pixelHeight / 2, this.pixelHeight / 2), 1, SpriteEffects.None, 0);
            //spriteBatch.Draw(texture2, BarrelEnd, new Rectangle(0, 0, 1, 1), Color.Red, 0, new Vector2(0, 0), 5, SpriteEffects.None, 0);
        }
    }
}
