using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TimeGame.Collisions;

namespace TimeGame.Sprites
{
    public class ObstacleSprite : Sprite
    {
        private int speed = 0;
        private Vector2 direction;

        private BoundingRectangle bounds;

        /// <summary>
        /// 
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="xDirection"></param>
        /// <param name="yDirection"></param>
        public ObstacleSprite(Vector2 position, Vector2 direction, int speed)
        {
            this.pixelWidth = 32;
            this.pixelHeight = 32;
            this.bounds = new BoundingRectangle(position - new Vector2(pixelWidth, pixelHeight), 32, 32);
            this.Position = position;
            this.direction = direction;
            this.speed = speed;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("64-64-sprite-pack");
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
            //Draw the sprite
            var source = new Rectangle(0, 32, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White);
        }
    }
}
