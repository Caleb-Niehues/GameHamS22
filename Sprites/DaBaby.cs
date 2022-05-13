using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TimeGame.Collisions;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace TimeGame.Sprites.Items
{
    public class DaBaby : Enemy
    {

        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        private List<SoundEffect> soundEffects = new List<SoundEffect>();

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;
        public bool bounceX = false;
        public bool bounceY = false;

        private int speed;
        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        public DaBaby()
        {
            pixelHeight = 128;
            pixelWidth = 64;
            bounds = new BoundingRectangle(Position.X -16, Position.Y - 32, pixelWidth -40, pixelHeight - 32);
            speed = 50;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTime += gameTime.ElapsedGameTime.TotalSeconds;
            if(animationTime > .3)
            {
                animationFrame++;
                animationTime = 0;
            }
            if(animationFrame > 2)
            {
                animationFrame = 0;
            }

            var source = new Rectangle(animationFrame * pixelWidth, 0, pixelWidth, pixelHeight);
            spriteBatch.Draw(texture, Position, source, Color);
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("TimeChild");
        }

        public override void Update(GameTime gameTime)
        {
            if (bounceX)
            {
                direction.X = 1;
            }
            else direction.X = -1;
            if (bounceY)
            {
                direction.Y = 1;
            }
            else direction.Y = -1;


            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(Direction.X * speed, Direction.Y * speed);
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 32;
        }
    }
}
