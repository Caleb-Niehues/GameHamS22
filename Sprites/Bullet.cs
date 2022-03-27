using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TimeGame.Collisions;

namespace TimeGame.Sprites
{
    public class Bullet : Sprite
    {

        public Bullet(PlayerSprite p)
        {
            player = p;
        }

        private PlayerSprite player;

        private BoundingCircle bounds = new BoundingCircle(new Vector2(50 - 16, 200 - 16), 16);
        public BoundingCircle Bounds => bounds;

        private int speed;
        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        private short animationFrame;

        private int shootingSpeed;
        public int ShootingSpeed
        {
            get => speed;
            set => speed = value;
        }

        public Color Color { get; set; } = Color.White;

        private Vector2 direction = new Vector2(1, 0);

        private float rotation;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("64-64-sprite-pack");
        }

        public override void Update(GameTime gameTime)
        {
           
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Direction.Y < 0) animationFrame = 0;
            else animationFrame = 1;

            //Draw the sprite
            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(bulletText, Position, source, Color);
        }
    }
}
