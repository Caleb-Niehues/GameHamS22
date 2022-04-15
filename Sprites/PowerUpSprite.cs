using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TimeGame.Collisions;

namespace TimeGame.Sprites
{
    public class PowerUpSprite : Enemy
    {
        private int speed = 0;

        private Rectangle source = new Rectangle(0, 0, 32, 32);

        private BoundingCircle bounds;

        /// <summary>
        /// 
        /// </summary>
        public BoundingCircle Bounds => bounds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="xDirection"></param>
        /// <param name="yDirection"></param>
        public PowerUpSprite(Vector2 position, Vector2 direction, int speed)
        {
            this.pixelWidth = 47;
            this.bounds = new BoundingCircle(position - new Vector2(pixelWidth, pixelWidth), pixelWidth);
            this.Position = position;
            this.direction = direction;
            this.speed = speed;
            this.IsActive = true;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("HeartPowerUp");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public override void Update(GameTime gameTime)
        {
            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(direction.X * speed, 0);
            bounds.Center.X = Position.X - pixelWidth;
            bounds.Center.Y = Position.Y - pixelWidth;
        }

        /// <summary>
        /// Draws the wall
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw the sprite
            spriteBatch.Draw(texture, Position, source, Color.White);
        }
    }
}
