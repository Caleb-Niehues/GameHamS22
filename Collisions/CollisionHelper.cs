using System;
using Microsoft.Xna.Framework;

namespace TimeGame.Collisions
{
    public static class CollisionHelper
    {
        /// <summary>
        /// Detects a collision between two BoundingCircles
        /// </summary>
        /// <param name="a">The first BoundingCircle</param>
        /// <param name="b">The second BoundingCircle</param>
        /// <returns>true = collision, false = no collison</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >=
                Math.Pow(a.Center.X - b.Center.X, 2) +
                Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects a collision between two BoundingRectangles
        /// </summary>
        /// <param name="a">The first BoundingRectangles</param>
        /// <param name="b">The second BoundingRectangles</param>
        /// <returns>true = collision, false = no collison</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left ||
                a.Left > b.Right ||
                a.Bottom < b.Top ||
                a.Top > b.Bottom);
        }
        /// <summary>
        /// Detects a collision between a BoundingCircle and BoundingRectangle
        /// </summary>
        /// <param name="c">The BoundingCircle</param>
        /// <param name="r">The BoundingRectangle</param>
        /// <returns>true = collision, false = no collison</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >=
               Math.Pow(c.Center.X - nearestX, 2) +
               Math.Pow(c.Center.Y - nearestY, 2);
        }
        /// <summary>
        /// Detects a collision between a BoundingRectangle and BoundingCircle 
        /// </summary>
        /// <param name="r">The BoundingRectangle</param>
        /// <param name="c">The BoundingCircle</param>
        /// <returns>true = collision, false = no collison</returns>
        public static bool Collides(BoundingRectangle r, BoundingCircle c) => Collides(c, r);

        public static bool Collides(IBounding a, IBounding b)
        {
            if (a is BoundingCircle abc)
            {
                if (b is BoundingCircle bbc)
                {
                    return Collides(abc, bbc);
                }
                else if (b is BoundingRectangle bbr)
                {
                    return Collides(abc, bbr);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (a is BoundingRectangle abr)
            {

                if (b is BoundingCircle bbc)
                {
                    return Collides(abr, bbc);
                }
                else if (b is BoundingRectangle bbr)
                {
                    return Collides(abr, bbr);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}