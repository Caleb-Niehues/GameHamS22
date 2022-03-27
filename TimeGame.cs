using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TimeGame.Sprites;
using TimeGame.Collisions;
using System;
using TimeGame.Screens;

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
        private SpriteFont _gameFont;

        private GameState state = GameState.InPlay;
        private int lives = 3;
        private int score;
        private int[] upgrades = { 1, 1, 1, 1 };
        private bool hasBeenHit = false;
        MouseState currentMouse;
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboard;

        public float bulletRot;
        public Vector2 bulletDir;

        public PlayerSprite player;
        public Tilemap _tilemap;

        public List<GruntSprite> enemies2;

        public List<GruntSprite> enemies;

        public List<GruntSprite> deadEnemies;

        public double gunTimer = 0;
        public double shootTime = 2.0;

        public int difficulty = 2;

        /// <summary>
        /// The width of the game world
        /// </summary>
        public static int GAME_WIDTH = 64* 12;

        /// <summary>
        /// The height of the game world
        /// </summary>
        public static int GAME_HEIGHT = 64 * 8;

        public BoundingRectangle gameBoundTop;
        public BoundingRectangle gameBoundBottom;


        Matrix translation = new Matrix();
        double translationTimer;

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
            _tilemap = new Tilemap(GAME_WIDTH, GAME_HEIGHT);


            enemies = new List<GruntSprite>();
            deadEnemies = new List<GruntSprite>();
            
            Random r = new Random();
            for(int i = 0; i < 10; i++)
            {
                int outerBounds = GAME_WIDTH + 60;
                Vector2 pos = new Vector2(r.Next(outerBounds, outerBounds + 100), r.Next(0, GAME_HEIGHT));
                GruntSprite ene = new GruntSprite(pos, player);
                if (i < difficulty)
                    enemies.Add(ene);
                else
                {
                    ene.Alive = false;
                    deadEnemies.Add(ene);
                }
                
            }
            gameBoundTop = new BoundingRectangle(0, -32, GAME_WIDTH, 0);
            gameBoundBottom = new BoundingRectangle(0, GAME_HEIGHT-128, GAME_WIDTH, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(this.Content);
            _tilemap.LoadContent(this.Content);
            _gameFont = this.Content.Load<SpriteFont>("Bangers");
            Pause.LoadContent(this.Content);
            Lost.LoadContent(this.Content);

            foreach (GruntSprite e in enemies)
            {
                e.LoadContent(this.Content);
            }
            foreach (GruntSprite e in deadEnemies)
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

            if (keyboardState != previousKeyboard && keyboardState.IsKeyDown(Keys.Escape))
            {
                switch (state)
                {
                    case GameState.Pause:
                        state = GameState.InPlay;
                        break;
                    case GameState.Lost:
                        Exit();
                        break;
                    default:
                        lives--;
                        state = GameState.Pause;
                        break;
                }
            }

            if (lives < 1)
            {
                state = GameState.Lost;
            }
            else if (state == GameState.Pause) //logic for upgrades go here
            {
                if (keyboardState != previousKeyboard && keyboardState.IsKeyDown(Keys.Q))
                {
                    if (score > upgrades[0] * 1000)
                    {
                        score -= upgrades[0] * 1000;
                        upgrades[0]++;
                    }
                }
                else if (keyboardState != previousKeyboard && keyboardState.IsKeyDown(Keys.W))
                {
                    if (score > upgrades[1] * 1000)
                    {
                        score -= upgrades[1] * 1000;
                        upgrades[1]++;
                    }
                }
                else if (keyboardState != previousKeyboard && keyboardState.IsKeyDown(Keys.E))
                {
                    if (score > upgrades[2] * 1000)
                    {
                        score -= upgrades[2] * 1000;
                        upgrades[2]++;
                    }
                }
                else if (keyboardState != previousKeyboard && keyboardState.IsKeyDown(Keys.R))
                {
                    if (score > upgrades[3] * 1000)
                    {
                        score -= upgrades[3] * 1000;
                        upgrades[3]++;
                    }
                }
            }
            #region Gameplay
            else if (state == GameState.InPlay)
            {
                gunTimer += gameTime.ElapsedGameTime.TotalSeconds;
                currentMouse = Mouse.GetState();
                bulletRot = player.Arm.GetRot();
                bulletDir = new Vector2(MathF.Cos(bulletRot), MathF.Sin(bulletRot));
                //if (enemies.Count < difficulty)
                //hasBeenHit = false;
                if (gunTimer > shootTime)
                {

                }
                if (enemies.Count < difficulty)
                {
                    Random r = new Random();
                    int outerBounds = GAME_WIDTH + 60;
                    Vector2 pos = new Vector2(r.Next(outerBounds, outerBounds + 100), r.Next(0, GAME_HEIGHT));

                    while (enemies.Count < difficulty)
                    {
                        deadEnemies[0].Position = pos;
                        deadEnemies[0].Alive = true;
                        enemies.Add(deadEnemies[0]);
                        deadEnemies.RemoveAt(0);
                    }
                }
                if (player.Bounds.CollidesWith(gameBoundTop) || player.Bounds.CollidesWith(gameBoundBottom))
                {
                    player.Direction = new Vector2(player.Direction.X, -player.Direction.Y);
                    if (player.Up) player.Up = false;
                    else player.Up = true;
                }
                player.Update(gameTime);

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].Alive)
                    {
                        enemies[i].Update(gameTime);
                        if (enemies[i].Bounds.CollidesWith(player.Bounds))
                        {

                            enemies[i].Position = new Vector2(-10, -10);
                            enemies[i].Alive = false;

                            deadEnemies.Add(enemies[i]);
                            enemies.Remove(enemies[i]);
                            lives--;
                            i--;
                        }
                    }
                }

                if (enemies.Count < difficulty)
                {
                    Random r = new Random();
                    int outerBounds = GAME_WIDTH + 60;
                    Vector2 pos = new Vector2(r.Next(outerBounds, outerBounds + 100), r.Next(0, GAME_HEIGHT));
                    while (enemies.Count - difficulty < 0)
                    {
                        deadEnemies[0].Position = pos;
                        deadEnemies[0].Alive = true;
                        enemies.Add(deadEnemies[0]);
                        deadEnemies.RemoveAt(0);
                    }
                }
                if (player.Bounds.CollidesWith(gameBoundTop) || player.Bounds.CollidesWith(gameBoundBottom))
                {
                    player.Direction = new Vector2(player.Direction.X, -player.Direction.Y);
                    if (player.Up) player.Up = false;
                    else player.Up = true;
                }

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].Alive)
                    {
                        enemies[i].Update(gameTime);
                        if (enemies[i].Bounds.CollidesWith(player.Bounds))
                        {

                            enemies[i].Position = new Vector2(-10, -10);
                            enemies[i].Alive = false;

                            deadEnemies.Add(enemies[i]);
                            enemies.Remove(enemies[i]);
                            lives--;
                            i--;
                        }
                    }
                }

                score += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                player.Update(gameTime);
                base.Update(gameTime);
            }
            #endregion
            else //game hasn't started or is over
            {

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            translationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (translationTimer > .1 && state == GameState.InPlay)
            {
                translation = Matrix.CreateTranslation(translation.Translation.X - 1, 0, 0);
                if (translation.Translation.X <= -64)
                {
                    _tilemap.newFrame();
                    translation = Matrix.CreateTranslation(0, 0, 0);
                }
            }
            _spriteBatch.Begin(transformMatrix: translation);
            _tilemap.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin();
            player.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(_gameFont, "Score: " + score, new Vector2(2, 20), Color.Black);
            _spriteBatch.DrawString(_gameFont, "Lives: " + lives, new Vector2(2, 40), Color.Black);
            foreach (GruntSprite e in enemies)
            {
                if (e.Alive)
                    e.Draw(gameTime, _spriteBatch);
            }
            switch (state)
            {
                case GameState.Pause:
                    Pause.Draw(_spriteBatch, score, upgrades);
                    break;
                case GameState.Lost:
                    Lost.Draw(_spriteBatch);
                    break;
                case GameState.Unstarted:
                    break;
                case GameState.InPlay:
                    //_spriteBatch.DrawString(_gameFont, "Score: " + score, new Vector2(2, 20), Color.Black);
                    //_spriteBatch.DrawString(_gameFont, "Lives: " + lives, new Vector2(2, 40), Color.Black);
                    break;
                default:

                    break;
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}