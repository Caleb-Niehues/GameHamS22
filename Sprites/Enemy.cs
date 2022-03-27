using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TimeGame.Sprites
{
    public abstract class Enemy : Sprite
    {
        protected short animationFrame;
        protected Vector2 direction;
        protected Color Color { get; set; }

    }
}
