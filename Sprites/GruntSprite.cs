using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TimeGame.Collisions;
using System.Collections.Generic;
using System.Text;

namespace TimeGame.Sprites
{
    public class GruntSprite : Enemy
    {
        private PlayerSprite player;

        private BoundingCircle bounds;
        public BoundingCircle Bounds => bounds;

        public bool Alive = true;

        private double waitTimerX = 0;
        private double waitTimerY = 0;

        private int speed;
        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        public GruntSprite(Vector2 position, PlayerSprite p, Texture2D texture)
        {
            if(texture != null)
                this.texture = texture;
            Position = position;
            player = p;
            Random r = new Random();
            speed = r.Next(50, 125);
            Color = Color.White;
            direction = new Vector2(1, 0);
            pixelHeight = 64;
            pixelWidth = 64;
            bounds = new BoundingCircle(new Vector2(Position.X + pixelWidth / 2, Position.Y + pixelHeight / 2), pixelWidth);
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Grunt");
        }

        public override void Update(GameTime gameTime)
        {
            float x = -1;
            float y = 1;
            //waitTimerX += gameTime.ElapsedGameTime.TotalSeconds;
            //waitTimerY += gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X > player.Position.X)
                x = -1;
            else 
                y = 0;

            if (Position.Y > player.Position.Y && y != 0)
                y = -1;
            else if(y != 0)
                y = 1;

            Direction = new Vector2(x, y);

            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(Direction.X * speed, Direction.Y * speed);
            bounds.Center.X = Position.X + pixelWidth /2;
            bounds.Center.Y = Position.Y + pixelHeight /2;
        }

        public void Debug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.Red });
            spriteBatch.Draw(rect, new Rectangle((int)bounds.Center.X - (bounds.Radius / 2), (int)bounds.Center.Y - (bounds.Radius / 2), bounds.Radius, bounds.Radius), Color.DarkRed * (float).8);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Update animation frame
            animationTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTime > .3)
            {
                animationFrame++;
                animationTime = 0;
            }
            if (animationFrame > 1)
            {
                animationFrame = 0;
            }
            //if (Direction.Y < 0) animationFrame = 0;
            //else animationFrame = 1;

            //Draw the sprite
            var source = new Rectangle(animationFrame * pixelWidth, 0, pixelWidth, pixelHeight);
            // Debug(gameTime, spriteBatch);
            spriteBatch.Draw(texture, Position, source, Color);
        }
    }
}