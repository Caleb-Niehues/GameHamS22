using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TimeGame.Collisions;
using System.Collections.Generic;
using System.Text;

namespace TimeGame.Sprites
{
    public class ChargerSprite : Enemy
    {

        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }
               

        private BoundingCircle bounds = new BoundingCircle(new Vector2(50 - 16, 200 - 16), 16);

        private int speed;
        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        public ChargerSprite()
        {
            direction = new Vector2(1, 1);
            pixelWidth = 128;
            pixelHeight = 96;
            Color = Color.White;
        } 

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Pinky");
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
            var source = new Rectangle(animationFrame * pixelWidth, 0, pixelWidth, pixelHeight);
            spriteBatch.Draw(texture, Position, source, Color, 0, new Vector2(0,0), 1, SpriteEffects.FlipHorizontally, 0);
        }
    }
}