using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TimeGame.Sprites;
using TimeGame.Collisions;
using System;


namespace TimeGame
{
    public enum GameState
    {
        Unstarted, //useful for a title screen
        InPlay,
        Pause, //used for the menu/pause screen
        Menu,
        Lost
    }

    public class TimeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState state = GameState.InPlay;
        private int lives = 3;
        private bool hasBeenHit = false;
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboard;

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

        public void Unpause()
        {
            state = GameState.InPlay;
        }

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
            gameBoundBottom = new BoundingRectangle(0, GAME_HEIGHT-64, GAME_WIDTH, 0);
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
            previousKeyboard = keyboardState;
            keyboardState = Keyboard.GetState();
            //holding onto incase we want to use a controller
            //GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed

            if (keyboardState != previousKeyboard && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                lives--;
                if (lives > 0)
                {
                    state = GameState.Pause;
                }
                else
                {
                    state = GameState.Lost;
                    Exit();
                }
            }
            if (state == GameState.Pause) //logic for upgrades go here
            {

            }
            else if (state == GameState.InPlay) //Gameplay occurs here
            {
                hasBeenHit = false;
                if (enemies.Count < 5)
                {
                    Random r = new Random();
                    int outerBounds = GAME_WIDTH + 60;
                    Vector2 pos = new Vector2(r.Next(outerBounds, outerBounds + 100), r.Next(0, GAME_HEIGHT));
                    EnemySprite enemy = new EnemySprite(pos, player);
                    enemy.LoadContent(this.Content);
                    enemies.Add(enemy);
                }
                if (player.Bounds.CollidesWith(gameBoundTop) || player.Bounds.CollidesWith(gameBoundBottom))
                {
                    player.Direction = new Vector2(player.Direction.X, -player.Direction.Y);
                    if (player.Up) player.Up = false;
                    else player.Up = true;
                }
                player.Update(gameTime);
                foreach (EnemySprite e in enemies)
                {
                    e.Update(gameTime);
                    if (e.Bounds.CollidesWith(player.Bounds))
                    {
                        hasBeenHit = true;
                        //e.Deactivate
                    }
                }
                if (hasBeenHit)
                {
                    lives--;
                    if (lives > 0)
                    {
                        state = GameState.Pause;
                    }
                    else
                    {
                        state = GameState.Lost;
                    }
                }
            }
            else //game hasn't started or is over
            {
            
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            switch (state)
            {
                case GameState.Lost:
                    break;
                case GameState.Pause:
                    break;
                case GameState.Unstarted:
                    break;
                default:
                    player.Draw(gameTime, _spriteBatch);
                    foreach (EnemySprite e in enemies)
                    {
                        e.Draw(gameTime, _spriteBatch);
                    }
                    break;
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}