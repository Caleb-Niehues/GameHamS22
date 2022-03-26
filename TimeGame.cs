using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TimeGame.Sprites;
using TimeGame.Collisions;
using System;


namespace TimeGame
{
    public class TimeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public PlayerSprite player;

        public List<EnemySprite> enemies;

        

        /// <summary>
        /// The width of the game world
        /// </summary>
        public static int GAME_WIDTH = 760;

        /// <summary>
        /// The height of the game world
        /// </summary>
        public static int GAME_HEIGHT = 480;

        public BoundingRectangle gameBoundTop;

        public BoundingRectangle gameBoundBottom;

        public TimeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = GAME_WIDTH;
            _graphics.PreferredBackBufferHeight = GAME_HEIGHT;
            _graphics.ApplyChanges();

            Window.Title = "The Great Work";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new PlayerSprite();
            enemies = new List<EnemySprite>();
            gameBoundTop = new BoundingRectangle(0, -32, GAME_WIDTH, 0);
            gameBoundBottom = new BoundingRectangle(0, GAME_HEIGHT -32 , GAME_WIDTH, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(this.Content);
            foreach (EnemySprite e in enemies)
            {
                e.LoadContent(this.Content);
            }
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (enemies.Count < 5)
            {
                Random r = new Random();
                int outerBounds = GAME_WIDTH + 60;
                Vector2 pos = new Vector2(r.Next(outerBounds, outerBounds + 100), r.Next(0, GAME_HEIGHT));
                EnemySprite enemy = new EnemySprite(pos, player);
                enemy.LoadContent(this.Content);
                enemies.Add(enemy);
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (player.Bounds.CollidesWith(gameBoundTop) || player.Bounds.CollidesWith(gameBoundBottom))
            {
                player.Direction = new Vector2(player.Direction.X, -player.Direction.Y);
                if (player.Up) player.Up = false;
                else player.Up = true;
            }
            // TODO: Add your update logic here
            foreach(EnemySprite e in enemies)
            {
                e.Update(gameTime);
            }
            player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            player.Draw(gameTime,_spriteBatch);
            foreach(EnemySprite e in enemies)
            {
                e.Draw(gameTime, _spriteBatch);
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
