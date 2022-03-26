using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TimeGame.Collisions;
using System.Collections.Generic;
using System.Text;

namespace TimeGame.Sprites
{
    public class ChargerSprite : Sprite
    {
        private short animationFrame;

        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }
        private Vector2 direction = new Vector2(1, 1);

        public Color Color { get; set; } = Color.White;

        private BoundingCircle bounds = new BoundingCircle(new Vector2(50 - 16, 200 - 16), 16);

        private int speed;
        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        private Texture2D chargeText;
        public override void LoadContent(ContentManager content)
        {
            chargeText = content.Load<Texture2D>("64-64-sprite-pack");
        }

        public override void Update(GameTime gameTime)
        {
            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(Direction.X * speed, 0);
            bounds.Center.X = Position.X - 16;
            bounds.Center.Y = Position.Y - 16;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {


            //Draw the sprite
            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(chargeText, Position, source, Color);
        }
    }
}