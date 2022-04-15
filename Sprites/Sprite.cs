using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeGame.Sprites
{
    public abstract class Sprite
    {
        /// <summary>
        /// The position of the sprite
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Whether or not this sprite is active and on screen
        /// </summary>
        public bool IsActive;

        /// <summary>
        /// The texture of the sprite
        /// </summary>
        protected Texture2D texture;

        /// <summary>
        /// The Frame of the sprite animation
        /// </summary>
        protected short animationFrame;

        /// <summary>
        /// The timer for the sprite animation
        /// </summary> 
        protected double animationTime;

        /// <summary>
        /// Width of the Sprite
        /// </summary>
        public int pixelWidth { get; protected set; }

        /// <summary>
        /// Height of the Sprite
        /// </summary>
        public int pixelHeight { get; protected set; }

        /// <summary>
        /// Loads the sprite texture
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public abstract void LoadContent(ContentManager content);

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}