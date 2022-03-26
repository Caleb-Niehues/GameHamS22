using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TimeGame.Collisions;
using System.Collections.Generic;
using System.Text;

namespace TimeGame.Sprites
{
    public class EnemySprite : Sprite
    {
        public Texture2D enemyText;

        private BoundingCircle bounds = new BoundingCircle(new Vector2(50 - 16, 200 - 16), 16);

        private int speed;
        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        private short animationFrame;

        public Color Color { get; set; } = Color.White;

        private Vector2 direction = new Vector2(1, 0);

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
            enemyText = content.Load<Texture2D>("64-64-sprite-pack");
        }

        public override void Update(GameTime gameTime)
        {
            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(0, Direction.Y * speed);
            bounds.Center.X = Position.X - 16;
            bounds.Center.Y = Position.Y - 16;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Direction.Y < 0) animationFrame = 0;
            else animationFrame = 1;

            //Draw the sprite
            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(enemyText, Position, source, Color);
        }
    }
}
