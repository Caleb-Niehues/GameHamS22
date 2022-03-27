using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TimeGame.Sprites.Items
{
    public abstract class Item : Sprite
    {
        protected Vector2 velocity;
        protected Vector2 acceleration;
        float accelerationTimer;
        protected float rotation = 0;
        protected float rotationdir = 0;
        protected float rotationspeed = 0;

        public float GetRot()
        {
            return rotation;
        }
    }
}
