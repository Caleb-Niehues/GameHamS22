using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TimeGame.Collisions;

namespace TimeGame.Sprites
{
    public class Bullet : Enemy
    {

        public Bullet()
        {

        }

        public Vector2 Origin;
        public float Rotation
        {
            get => rotation;
            set => rotation = value;
        }

        private BoundingCircle bounds = new BoundingCircle(new Vector2(50 - 16, 200 - 16), 16);
        public BoundingCircle Bounds => bounds;

        private int speed;
        public int Speed
        {
            get => speed;
            set => speed = value;
        }
        public int hitCount = 1;

        private short animationFrame;

        private int shootingSpeed;
        public int ShootingSpeed
        {
            get => speed;
            set => speed = value;
        }

        public Color Color { get; set; } = Color.Red;


        private float rotation;


        /// <summary>
        /// 
        /// </summary>
        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("64-64-sprite-pack");
            Origin = new Vector2(0, texture.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            speed = 10;
            Position += Direction * speed;
            bounds.Center.X = Position.X - 16;
            bounds.Center.Y = Position.Y - 16;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Direction.Y < 0) animationFrame = 0;
            else animationFrame = 1;

            //Draw the sprite
            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color, rotation, Origin, .25f, SpriteEffects.None, 0);
        }
    }
}
