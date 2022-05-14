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
            pixelHeight = 256;
            pixelWidth = 192;
            direction = new Vector2(-1, 1);
            bounds = new BoundingRectangle(Position.X -16, Position.Y - 32, pixelWidth -40, pixelHeight - 32);
            speed = 75;
            Color = Color.White;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationFrame = 0;
            

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
                Direction = new Vector2(1,Direction.Y);
            }
            else Direction = new Vector2(-1,Direction.Y);
            if (bounceY)
            {
                Direction = new Vector2(Direction.X, 1);
            }
            else Direction = new Vector2(Direction.X,-1);

            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(Direction.X * speed, Direction.Y * speed);
            Position.Y = Math.Clamp(Position.Y, 0, 448);
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 32;
        }
    }
}
