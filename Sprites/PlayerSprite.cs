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

        private GraphicsDeviceManager _graphics;
        private TimeGame tg;
        /// <summary>
        /// A constrcutor that can alos be used to reset a player sprite
        /// </summary>
        /// <param name="texture">The texture used to draw the player, used to remember the texture when used as a reset - if null, it is assumed to be handled by LoadContent</param>
        /// <param name="armTextures">The various textures used for the gun arms constructors</param>
        public PlayerSprite(Texture2D texture, Texture2D[] armTextures, TimeGame TG, GraphicsDeviceManager graphics)
        {
            if (texture != null)
                this.texture = texture;
            Arms[0] = new Pistol(Position, new Vector2(32, 39), armTextures[0]);
            Arms[1] = new Shotgun(Position, new Vector2(32, 39), armTextures[1]);
            Arms[2] = new Sniper(Position, new Vector2(32, 39), armTextures[2]);
            Position = new Vector2(250, 225);
            this.pixelWidth = 64;
            this.pixelHeight = 128;
            bounds = new BoundingRectangle(Position.X - 16, Position.Y - 32, pixelWidth - 40, pixelHeight - 32);
            armIndex = 0;

            tg = TG;
            _graphics = graphics;
        }

        private MouseState mouseState;
        private MouseState previousMouseState;

        public bool Up;

        public int armIndex = 0;

        public Arm[] Arms = new Arm[3];

        private Vector2 direction = new Vector2(0, 1);

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        private int speed = 90;

        public float GetRotation()
        {
            return this.Arms[0].GetRot();
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
            foreach (Arm arm in Arms)
                arm.LoadContent(content);
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public override void Update(GameTime gameTime)
        {
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
            speed = 60;

            //maybe flip "fast" direction to push you towards the dead zone?
            if (previousMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
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
            }
            if (previousMouseState.RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed)
            {
                
                if (armIndex == 0)
                {
                    armIndex = 1;
                    speed = 80 + TimeGame.extraPistolMovementSpeed;
                }
                else if (armIndex == 1)
                {
                    armIndex = 2;
                    speed = 80;
                }
                else if (armIndex == 2)
                {
                    armIndex = 0;
                    speed = 90;
                }
               
            }

            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(0, Direction.Y * speed);
            Position.Y = Math.Clamp(Position.Y, 0, 448);
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 32;

            Arms[armIndex].BodyPosition = Position;
            Arms[armIndex].ArmPowerUp = powerUp;
            
            Arms[armIndex].Update(gameTime, (_graphics.PreferredBackBufferWidth / tg.virtualWidth), (_graphics.PreferredBackBufferHeight / tg.virtualHeight));
        }

        public void Debug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.Red });
            spriteBatch.Draw(rect, new Rectangle((int)bounds.X + 38, (int)bounds.Y + 32, (int)bounds.Width + 10, (int)bounds.Height + 20), Color.DarkRed * (float).8);
        }


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
            Arms[armIndex].Draw(gameTime, spriteBatch);
            // Debug(gameTime, spriteBatch);
        }


    }
}