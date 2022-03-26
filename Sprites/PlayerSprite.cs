using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TimeGame.Collisions;

namespace TimeGame.Sprites
{
    /// <summary>
    /// A class representing a ball
    /// </summary>
    public class PlayerSprite : Sprite
    {

        public PlayerSprite()
        {
            Position = new Vector2(600, 300);
        }

        private MouseState mouseState;
        private MouseState previousMouseState;

        private bool up;

        private Vector2 direction = new Vector2(1, -1);

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        private int speed = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public double Distance => Math.Sqrt(Math.Pow(speed * direction.X, 2) + Math.Pow(speed * direction.Y, 2));

        private BoundingCircle bounds = new BoundingCircle(new Vector2(50 - 16, 200 - 16), 16);

        /// <summary>
        /// 
        /// </summary>
        public BoundingCircle Bounds => bounds;

        private short animationFrame;

        /// <summary>
        /// 
        /// </summary>
        public Color Color { get; set; } = Color.White;

        public void Bounce()
        {
            Position.Y = Math.Clamp(Position.Y, 0, 448);
            direction.Y *= -1;
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
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            //maybe flip "fast" direction to push you towards the dead zone?
            if (previousMouseState != mouseState && mouseState.LeftButton == ButtonState.Pressed)
            {

                direction.X = 0;
                if (up) direction.Y = 1;
                else direction.Y = -1;

                speed = 50;
            }
            else if (previousMouseState != mouseState && mouseState.RightButton == ButtonState.Pressed)
            {

            }

            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(0, Direction.Y * speed);
            bounds.Center.X = Position.X - 16;
            bounds.Center.Y = Position.Y - 16;
        }

        /// <summary>
        /// Draws the animated ball
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Update animation frame
            if (Direction.Y < 0) animationFrame = 0;
            else animationFrame = 1;

            //Draw the sprite
            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color);
        }
    }
}