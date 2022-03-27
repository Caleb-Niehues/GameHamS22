using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TimeGame.Collisions;
using TimeGame.Sprites.Items;
using System.Collections.Generic;

namespace TimeGame.Sprites
{
    /// <summary>
    /// A class representing the player
    /// </summary>
    public class PlayerSprite : Sprite
    {
        public Item Arm;
        public PlayerSprite()
        {
            Position = new Vector2(250, 225);
            this.pixelWidth = 64;
            this.pixelHeight = 128;
            Arm = new StartingGun(Position, new Vector2(32, 39));
            bounds = new BoundingRectangle(Position.X - 16, Position.Y - 32, pixelWidth - 32, pixelHeight - 32);
        }

        private MouseState mouseState;
        private MouseState previousMouseState;

        public bool Up;

        private Vector2 direction = new Vector2(0, 1);

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        private int speed = 0;

        public float GetRotation()
        {
            return this.Arm.GetRot();
        }

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

        private BoundingRectangle bounds;

        /// <summary>
        /// 
        /// </summary>
        public BoundingRectangle Bounds => bounds;

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
            texture = content.Load<Texture2D>("Player");
            Arm.LoadContent(content);
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
                
                if (Up)
                {
                    direction.Y = 1;
                    Up = false;
                }
                else
                {
                    direction.Y = -1;
                    Up = true;
                }

                speed = 100;
            }
            else if (previousMouseState != mouseState && mouseState.RightButton == ButtonState.Pressed)
            {

            }

            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(0, Direction.Y * speed);
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 32;


            if (Arm is StartingGun sg)
            {
                sg.BodyPosition = Position;
                sg.ArmPowerUp = powerUp;
            }
            else if (Arm is Shotgun shg)
            {
                shg.BodyPosition = Position;
                shg.ArmPowerUp = powerUp;
            }
            else if (Arm is Sniper sng)
            {
                sng.BodyPosition = Position;
                sng.ArmPowerUp = powerUp;
            }
            Arm.Update(gameTime);
        }


        private short animationFrame;
        private double animationTime;
        private short powerUp;
        /// <summary>
        /// Draws the animated ball
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Update animation frame
            animationTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTime > .3)
            {
                animationFrame++;
                animationTime = 0;
            }
            if (animationFrame > 2)
            {
                animationFrame = 0;
            }
            //if (Direction.Y < 0) animationFrame = 0;
            //else animationFrame = 1;

            //Draw the sprite
            var source = new Rectangle(animationFrame * this.pixelWidth, powerUp * this.pixelHeight, this.pixelWidth, this.pixelHeight);
            spriteBatch.Draw(texture, Position, source, Color);
            Arm.Draw(gameTime, spriteBatch);
        }

        
    }
}