namespace TimeGame.Collisions
{
    public interface IBounding
    {
        /// <summary>
        /// Detects a collision between two Bounding items
        /// </summary>
        /// <param name="other">The other bounding item</param>
        /// <returns>true = collision, false = no collison</returns>
        public bool CollidesWith(IBounding other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}