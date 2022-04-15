using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TimeGame.Collisions
{
    /// <summary>
    /// A struct representing circular bounds
    /// </summary>
    public struct BoundingRectangle : IBounding
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        /// <summary>
        /// Constructs a new BoundingRectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param> 
        public BoundingRectangle(float x, float y, float width, float height) : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        /// <summary>
        /// Constructs a new BoundingRectangle
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param> 
        public BoundingRectangle(Vector2 position, float width, float height) : this()
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// The texture of the sprite
        /// </summary>
        Texture2D texture;
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("PIXEL");
        }

        /// <summary>
        /// Detects a collision between two Bounding items
        /// </summary>
        /// <param name="other">The other bounding items</param>
        /// <returns>true = collision, false = no collison</returns>
        public bool CollidesWith(IBounding other)
        {
            return CollisionHelper.Collides(this, other);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)X, (int)Y, (int)Width, (int)Height), Color.DarkRed * (float).8);
        }
    }
}