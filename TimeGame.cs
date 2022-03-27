using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TimeGame.Sprites;
using TimeGame.Collisions;
using System;
using TimeGame.Screens;
using TimeGame.Sprites.Items;

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
        private bool hasBeenHit = false;
        private int score;
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

        public List<Bullet> bullets;
        public List<Bullet> shotBullets;

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

            bullets = new List<Bullet>();
            shotBullets = new List<Bullet>();

            
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
            for(int i = 0; i < 50; i++)
            {
                Bullet b = new Bullet();
                shotBullets.Add(b);
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
            Lose.LoadContent(this.Content);

            foreach (GruntSprite e in enemies)
            {
                e.LoadContent(this.Content);
            }
            foreach (GruntSprite e in deadEnemies)
            {
                e.LoadContent(this.Content);
            }
            foreach(Bullet b in shotBullets)
            {
                b.LoadContent(Content);
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

            if (state == GameState.Pause && lives > 0) //logic for upgrades go here
            {

            }
            else if (state == GameState.InPlay) //Gameplay occurs here
            {
                gunTimer += gameTime.ElapsedGameTime.TotalSeconds;
                MouseState currentMouse = Mouse.GetState();

                //if (enemies.Count < difficulty)
                    //hasBeenHit = false;
                if(gunTimer > shootTime)
                {
                    bulletRot = player.Arm.GetRot();
                    bulletDir = new Vector2(MathF.Cos(bulletRot), MathF.Sin(bulletRot));
                    ShootGun(bulletRot, bulletDir);
                    gunTimer = 0;
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
                            if (lives > 0)
                            {
                                //state = GameState.Pause;
                            }
                            else
                            {
                                state = GameState.Lost;
                            }
                        }
                        if (i < 0) i = 0;
                        bool dead = false;
                        for (int j = 0; j < bullets.Count; j++)
                        {
                            bullets[j].Update(gameTime);
                            if (bullets[j].Bounds.CollidesWith(enemies[i].Bounds))
                            {
                                bullets[j].hitCount -= 1;
                                if (bullets[j].hitCount <= 0)
                                {
                                    bullets[j].Position = new Vector2(-10, -10);
                                    shotBullets.Add(bullets[j]);
                                    bullets.RemoveAt(0);
                                    j--;
                                }
                                dead = true;
                                
                            }
                        }
                        if (dead)
                        {
                            enemies[i].Position = new Vector2(-10, -10);
                            enemies[i].Alive = false;
                            deadEnemies.Add(enemies[i]);
                            enemies.RemoveAt(i);
                            i--;
                        }

                    }
                }




            }




            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here
            
            else //game hasn't started or is over
            {

            }
            base.Update(gameTime);
        }

        Matrix translation = new Matrix();
        double translationTimer;
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

            _spriteBatch.DrawString(_gameFont, "Score: " + score, new Vector2(2, 20), Color.Gold);
            player.Draw(gameTime, _spriteBatch);
            foreach (GruntSprite e in enemies)
            {
                if (e.Alive)
                    e.Draw(gameTime, _spriteBatch);
            }
            switch (state)
            {
                case GameState.Pause:
                    Pause.Draw(_spriteBatch);
                    break;
                case GameState.Lost:
                    Lose.Draw(_spriteBatch);
                    break;
                case GameState.Unstarted:
                    break;
                default:
                    player.Draw(gameTime, _spriteBatch);
                    foreach (GruntSprite e in enemies)
                    {
                        if (e.Alive)
                        e.Draw(gameTime, _spriteBatch);
                    }
                    foreach(Bullet b in bullets)
                    {
                        b.Draw(gameTime, _spriteBatch);
                    }
                    break;
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void ShootGun(float rot, Vector2 dir)
        {
            if (shotBullets.Count > 0)
            {
                Bullet b = shotBullets[0];
                b.Direction = dir;
                b.Rotation = rot;
                b.Position = player.Arm.Position;
                if (player.Arm is StartingGun sg)
                {
                    b.Position = sg.BarrelEnd;
                }
                else if (player.Arm is Shotgun shg)
                {
                    b.Position = shg.BarrelEnd;
                }
                else if (player.Arm is Sniper sng)
                {
                    b.Position = sng.BarrelEnd;
                }
                bullets.Add(b);
                shotBullets.RemoveAt(0);
            }
        }
    }
}