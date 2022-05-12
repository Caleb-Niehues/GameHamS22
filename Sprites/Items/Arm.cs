using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
    }
}
