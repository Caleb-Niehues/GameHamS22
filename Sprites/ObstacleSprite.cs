using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TimeGame.Collisions;

namespace TimeGame.Sprites
{
    public enum Obstacle
    {
        Concrete = 0,
        Crate = 1,
        Cone = 2,
        SunPowerup = 3
    }
    public class ObstacleSprite : Sprite
    {
        private int speed = 0;
        private Vector2 direction;

        private BoundingRectangle bounds;
        private Rectangle source;

        /// <summary>
        /// 
        /// </summary>
        public BoundingRectangle Bounds => bounds;
        public Obstacle obstacle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="xDirection"></param>
        /// <param name="yDirection"></param>
        public ObstacleSprite(Vector2 position, Vector2 direction, int speed, Obstacle ob)
        {
            this.bounds = new BoundingRectangle(position - new Vector2(pixelWidth, pixelHeight), 32, 32);
            this.Position = position;
            this.direction = direction;
            this.speed = speed;
            obstacle = ob;

            switch (obstacle)
            {
                case Obstacle.Concrete:
                    this.pixelWidth = 41;
                    this.pixelHeight = 100;
                    source = new Rectangle(0, 0, pixelWidth, pixelHeight);
                    break;
                case Obstacle.Crate:
                    this.pixelWidth = 41;
                    this.pixelHeight = 41;
                    source = new Rectangle(41, 0, pixelWidth, pixelHeight);
                    break;
                case Obstacle.Cone:
                    this.pixelWidth = 41;
                    this.pixelHeight = 59;
                    source = new Rectangle(41, 41, pixelWidth, pixelHeight);
                    break;
                case Obstacle.SunPowerup:
                    this.pixelWidth = 41;
                    this.pixelHeight = 50;
                    break;
            }
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Obstacles");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public override void Update(GameTime gameTime)
        {
            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(direction.X * speed, direction.Y * speed);
            bounds.X = Position.X - pixelWidth;
            bounds.Y = Position.Y - pixelHeight;
        }

        /// <summary>
        /// Draws the wall
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Update animation frame
            if (obstacle == Obstacle.SunPowerup)
            {
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
                source = new Rectangle(41 * 3, animationFrame * pixelHeight, pixelWidth, pixelHeight);
            }
            //if (Direction.Y < 0) animationFrame = 0;
            //else animationFrame = 1;

            //Draw the sprite
            spriteBatch.Draw(texture, Position, source, Color.White);
        }
    }
}