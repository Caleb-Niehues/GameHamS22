using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TimeGame.Sprites.Items
{
    public abstract class Arm : Sprite
    {
        protected Vector2 velocity;
        protected Vector2 acceleration;
        protected float rotationdir = 0;
        protected float rotationspeed = 0;

        public Vector2 BodyOrigin;
        public Vector2 BodyPosition;
        public Vector2 BarrelEnd;

        public short ArmPowerUp;

        public float GetRot()
        {
            return rotationdir;
        }
        public virtual void CalculateBarrel()
        {
            this.BarrelEnd = this.Position + new Vector2((pixelWidth - this.pixelHeight / 2) * (float)Math.Cos(rotationdir), (pixelWidth - this.pixelHeight / 2) * (float)Math.Sin(rotationdir));
        }
        public override void Update(GameTime gameTime)
        {
            this.Position = BodyPosition + BodyOrigin;
            //https://stackoverflow.com/questions/7339574/xna-rotating-a-sprite-to-face-the-cursor-exactly
            MouseState ms = Mouse.GetState(); ;
            rotationdir = (float)Math.Atan2((ms.Y - (Position.Y)), (ms.X - (Position.X)));
            CalculateBarrel();
            //Mouse.SetPosition((int)BarrelEnd.X, (int) BarrelEnd.Y);
            //Debug.WriteLine(rotationdir);
        }
        public void Update(GameTime gameTime, float xScale, float yScale)
        {
            this.Position = BodyPosition + BodyOrigin;
            //https://stackoverflow.com/questions/7339574/xna-rotating-a-sprite-to-face-the-cursor-exactly
            MouseState ms = Mouse.GetState(); ;
            rotationdir = (float)Math.Atan2((ms.Y - (Position.Y * yScale)), (ms.X - (Position.X * xScale)));
            CalculateBarrel();
            //Mouse.SetPosition((int)BarrelEnd.X, (int) BarrelEnd.Y);
            //Debug.WriteLine(rotationdir);
        }
    }
}
