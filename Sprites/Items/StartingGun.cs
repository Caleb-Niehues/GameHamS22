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
    public class StartingGun : Item
    {
        public Vector2 BodyOrigin;
        public Vector2 BodyPosition;

        public short ArmPowerUp;
        public StartingGun(Vector2 position, Vector2 origin)
        {
            BodyOrigin = origin;
            BodyPosition = position;
            this.Position = BodyPosition + BodyOrigin;
            this.pixelWidth = 64;
            this.pixelHeight = 16; 
        }
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ArmPistol");
        }

        public override void Update(GameTime gameTime)
        {
            this.Position = BodyPosition + BodyOrigin;
            //https://stackoverflow.com/questions/7339574/xna-rotating-a-sprite-to-face-the-cursor-exactly
            MouseState ms = Mouse.GetState(); ;
            rotationdir = (float)Math.Atan2((ms.Y - Position.Y), (ms.X - Position.X));
            Debug.WriteLine(rotationdir);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var source = new Rectangle(0, ArmPowerUp * this.pixelHeight, this.pixelWidth, this.pixelHeight);
            spriteBatch.Draw(texture, Position, source, Color.White, rotation, new Vector2(this.pixelHeight / 2, this.pixelHeight / 2), 1, SpriteEffects.None, 0);
        }
    }
}
