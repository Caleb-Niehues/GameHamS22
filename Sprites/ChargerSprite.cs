using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TimeGame.Collisions;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace TimeGame.Sprites
{
    public class ChargerSprite : Enemy
    {

        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        private List<SoundEffect> _soundEffects = new List<SoundEffect>();

        public List<SoundEffect> SoundEffects => _soundEffects;

        public double pokeTimer = 0;
        public double pokeTiming = 3.2;
        public bool hasPlayed = false;
        public bool hasPlayed2 = false;

        public double peakTimer = 0;
        public double peakTiming = 1;

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        private int speed;
        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        public ChargerSprite(Texture2D texture, List<SoundEffect> sounds)
        {
            if (texture != null && sounds != null)
            {
                this.texture = texture;
                this._soundEffects = sounds;
            }
            direction = new Vector2(1, 1);
            pixelWidth = 128;
            pixelHeight = 96;
            Color = Color.White;
            speed = 300;
            bounds = new BoundingRectangle(50 - 16, 200 - 16, pixelWidth, pixelHeight);
            pokeTimer = 0;
            Random r = new Random();
            Position = new Vector2(-64, r.Next(64, TimeGame.GAME_HEIGHT - 64));
        } 

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Pinky");
            _soundEffects.Add(content.Load<SoundEffect>("Sounds/arrival"));
            _soundEffects.Add(content.Load<SoundEffect>("Sounds/charge"));
        }

        public override void Update(GameTime gameTime)
        {

            if (Position.X < 16) 
            {
                Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(Direction.X * speed, 0);
                if (!hasPlayed2) 
                {
                    _soundEffects[0].Play();
                    hasPlayed2 = true;
                }
            }
            else
            {
                pokeTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (pokeTimer > pokeTiming)
                {
                    Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(Direction.X * speed, 0);
                    if (!hasPlayed)
                    {
                        _soundEffects[1].Play();
                        hasPlayed = true;
                    }
                }
            }

            bounds.X = Position.X - 32;
            bounds.Y = Position.Y;
        }

        public void Debug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.Red });
            spriteBatch.Draw(rect, new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height), Color.DarkRed * (float).8);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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

            //Draw the sprite
            var source = new Rectangle(animationFrame * pixelWidth, 0, pixelWidth, pixelHeight);
            // Debug(gameTime, spriteBatch);
            spriteBatch.Draw(texture, Position, source, Color, 0, new Vector2(0,0), 1, SpriteEffects.FlipHorizontally, 0);
        }
    }
}