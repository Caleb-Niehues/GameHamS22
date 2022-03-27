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

        private BoundingCircle bounds = new BoundingCircle(new Vector2(50 - 16, 200 - 16), 16);
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
        public GruntSprite(Vector2 position, PlayerSprite p)
        {
            Position = position;
            player = p;
            Random r = new Random(); 
            speed = r.Next(50,125);
            Color = Color.White;
            direction = new Vector2(1, 0);
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("64-64-sprite-pack");
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
            bounds.Center.X = Position.X - 16;
            bounds.Center.Y = Position.Y - 16;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Direction.Y < 0) animationFrame = 0;
            else animationFrame = 1;

            //Draw the sprite
            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color);
        }
    }
}