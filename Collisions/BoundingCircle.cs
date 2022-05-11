using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TimeGame.Collisions
{
    /// <summary>
    /// A struct representing circular bounds
    /// </summary>
    public struct BoundingCircle : IBounding
    {
        /// <summary>
        /// The center of the BoundingCircle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The Radius of the BoundingCircle
        /// </summary>
        public int Radius;


        /// <summary>
        /// Constructs a new BoundingCircle
        /// </summary>
        /// <param name="center">Center</param>
        /// <param name="radius">Radius</param>
        public BoundingCircle(Vector2 center, int radius)
        {
            Center = center;
            Radius = radius;
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
    }
}