using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using TimeGame.Sprites;
using TimeGame.Collisions;
using System;
using TimeGame.Screens;
using TimeGame.Sprites.Items;
using TimeGame.Scoring;

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


        private List<SoundEffect> _soundEffects = new List<SoundEffect>();

        private GameState state = GameState.InPlay;
        private int lives = 3;
        private int scoreBucket;
        private int score;
        private int costModifier = 5;
        private int[] upgrades = { 1, 1, 1, 1 };
        private bool hasBeenHit = false;
        MouseState currentMouse;
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboard;

        public float bulletRot;
        public Vector2 bulletDir;




        public PlayerSprite player;
        public Tilemap _tilemap;

        public List<GruntSprite> enemies;

        public List<GruntSprite> deadEnemies;

        public List<ChargerSprite> charger;
        public List<ChargerSprite> chargerStandby;

        public double gunTimer = 0;
        public double shootTime = 2.0;

        public int difficulty = 2;
        private int riserCheck;

        public List<Bullet> bullets;
        public List<Bullet> shotBullets;

        public Random ran = new Random();

        /// <summary>
        /// The width of the game world
        /// </summary>
        public static int GAME_WIDTH = 64 * 12;

        /// <summary>
        /// The height of the game world
        /// </summary>
        public static int GAME_HEIGHT = 64 * 8;

        public BoundingRectangle gameBoundTop;
        public BoundingRectangle gameBoundBottom;
        public BoundingRectangle gameBoundFront;
        public BoundingRectangle gameBoundBack;

        public double chargerTimer = 0;
        public double chargeWaitTime = 4.5;
        public Leaderboard Leaderboard;


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

            //Pause.Width = GAME_WIDTH;
            //Pause.Width = GAME_HEIGHT;
            Lost.Width = GAME_WIDTH;
            Lost.Width = GAME_HEIGHT;
            Window.Title = "The Great Work";
        }

        protected override void Initialize()
        {
            player = new PlayerSprite();
            _tilemap = new Tilemap(GAME_WIDTH, GAME_HEIGHT);

            enemies = new List<GruntSprite>();
            deadEnemies = new List<GruntSprite>();

            bullets = new List<Bullet>();
            shotBullets = new List<Bullet>();

            Leaderboard = new Leaderboard();

            Leaderboard.Load();

            charger = new List<ChargerSprite>();
            chargerStandby = new List<ChargerSprite>();

            for(int i = 0; i < 10; i++)
            {
                ChargerSprite charge = new ChargerSprite();
                chargerStandby.Add(charge);
            }

            Random r = new Random();
            for (int i = 0; i < 10; i++)
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
            for (int i = 0; i < 50; i++)
            {
                Bullet b = new Bullet();
                shotBullets.Add(b);
            }
            gameBoundTop = new BoundingRectangle(0, -32, GAME_WIDTH + 128, 0);
            gameBoundBottom = new BoundingRectangle(0, GAME_HEIGHT - 128, GAME_WIDTH + 128, 0);
            gameBoundFront = new BoundingRectangle(-64, 0, 1, GAME_HEIGHT);
            gameBoundBack = new BoundingRectangle(64 + GAME_WIDTH,0,1,GAME_HEIGHT);
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


            _soundEffects.Add(Content.Load<SoundEffect>("Sounds/Pistol"));
            _soundEffects.Add(Content.Load<SoundEffect>("Sounds/Shotgun"));
            _soundEffects.Add(Content.Load<SoundEffect>("Sounds/Sniper"));
            _soundEffects.Add(Content.Load<SoundEffect>("Sounds/Upgrade"));


            foreach (GruntSprite e in enemies)
            {
                e.LoadContent(this.Content);
            }
            foreach (GruntSprite e in deadEnemies)
            {
                e.LoadContent(this.Content);
            }
            foreach (Bullet b in shotBullets)
            {
                b.LoadContent(Content);
            }
            foreach(ChargerSprite c in chargerStandby)
            {
                c.LoadContent(Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            previousKeyboard = keyboardState;
            keyboardState = Keyboard.GetState();

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

            if (lives < 1 && state != GameState.Lost)
            {
                state = GameState.Lost;
                UpdateLeaderboard(Environment.UserName, score);
            }
            else if (state == GameState.Pause) //logic for upgrades go here
            {
                if (keyboardState != previousKeyboard && keyboardState.IsKeyDown(Keys.Q))
                {
                    if (score >= upgrades[0] * costModifier * 2 && lives < 4)
                    {
                        lives++;
                        score -= upgrades[0] * costModifier * 2;
                        upgrades[0]++;
                        _soundEffects[3].Play();
                    }
                }
                else if (keyboardState != previousKeyboard && keyboardState.IsKeyDown(Keys.W))
                {
                    if (score >= upgrades[1] * costModifier)
                    {
                        score -= upgrades[1] * costModifier;
                        upgrades[1]++;
                        _soundEffects[3].Play();
                    }
                }
                else if (keyboardState != previousKeyboard && keyboardState.IsKeyDown(Keys.E))
                {
                    if (score >= upgrades[2] * costModifier)
                    {
                        score -= upgrades[2] * costModifier;
                        upgrades[2]++;
                        _soundEffects[3].Play();
                    }
                }
                else if (keyboardState != previousKeyboard && keyboardState.IsKeyDown(Keys.R))
                {
                    if (score >= upgrades[3] * costModifier)
                    {
                        score -= upgrades[3] * costModifier;
                        upgrades[3]++;
                        _soundEffects[3].Play();
                    }
                }
            }
            #region Gameplay
            else if (state == GameState.InPlay)
            {

                chargerTimer += gameTime.ElapsedGameTime.TotalSeconds;
                gunTimer += gameTime.ElapsedGameTime.TotalSeconds;
                currentMouse = Mouse.GetState();
                riserCheck = gameTime.TotalGameTime.Seconds / 15;
                if (riserCheck > difficulty && difficulty < 50) difficulty++;
                if(chargerTimer > chargeWaitTime) 
                {
                    chargerStandby[0].Position = new Vector2(-64,ran.Next(128, GAME_HEIGHT - 128));
                    chargerStandby[0].pokeTimer = 0;
                    chargerStandby[0].hasPlayed = false;
                    chargerStandby[0].hasPlayed2 = false;
                    charger.Add(chargerStandby[0]);
                    chargerStandby.RemoveAt(0);
                    chargerTimer = 0;
                }
                
                //if (enemies.Count < difficulty)
                //hasBeenHit = false;

                // 0 = Pistol
                // 1 = Shotgun
                // 2 = Sniper

                if (gunTimer > shootTime)
                {
                    bulletRot = player.Arms[player.armIndex].GetRot();
                    bulletDir = new Vector2(MathF.Cos(bulletRot), MathF.Sin(bulletRot));
                    Random r = new Random();
                    if (player.armIndex == 1)
                        for (int i = 0; i < 1 + upgrades[2]; i++) 
                        {
                            bulletRot = player.Arms[player.armIndex].GetRot();
                            double d = r.NextDouble() * (double)(Math.PI / 8);
                            if (i % 2 == 0)
                            {
                                d *= -1;
                            }
                            bulletRot += (float)d;

                            //bulletDir = ((Shotgun)player.Arms[player.armIndex]).CalculateBarrel(bulletRot);
                            bulletDir = new Vector2(MathF.Cos(bulletRot), MathF.Sin(bulletRot));
                            ShootGun(bulletRot, bulletDir);
                        }
                    else ShootGun(bulletRot, bulletDir);
                    _soundEffects[player.armIndex].Play();
                    // Call gun sound here
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
                            state = GameState.Pause;
                            i--;
                        }
                        else if (enemies[i].Bounds.CollidesWith(gameBoundFront))
                        {
                            enemies[i].Position = new Vector2(-10, -10);
                            enemies[i].Alive = false;

                            deadEnemies.Add(enemies[i]);
                            enemies.Remove(enemies[i]);
                            --i;
                        }
                        
                    }
                    if (i < 0) i = 0;
                    bool dead = false;
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        bullets[j].Update(gameTime);
                        if (bullets[j].Bounds.CollidesWith(enemies[i].Bounds) && !dead)
                        {
                            bullets[j].hitCount -= 1;
                            if (bullets[j].hitCount <= 0)
                            {
                                bullets[j].Shot = true;
                                bullets[j].Position = new Vector2(-10, -10);
                                shotBullets.Add(bullets[j]);
                                bullets.RemoveAt(j);
                                if(j > 0) j--;
                            }
                            dead = true;

                        }

                        else if((bullets[j].Bounds.CollidesWith(gameBoundTop) || bullets[j].Bounds.CollidesWith(gameBoundBack) 
                            || bullets[j].Bounds.CollidesWith(gameBoundFront) || bullets[j].Bounds.CollidesWith(gameBoundBottom)))
                        {
                            bullets[j].Position = new Vector2(-10, -10);
                            shotBullets.Add(bullets[j]);
                            bullets.RemoveAt(j);
                            if(j > 0) j--;
                        }
                        
                    }
                    for (int j = 0; j < charger.Count; j++)
                    {
                        charger[j].Update(gameTime);
                        if (charger[j].Bounds.CollidesWith(gameBoundBack))
                        {
                            chargerStandby.Add(charger[j]);
                            charger.RemoveAt(j);
                            if (j > 0) j--;
                        }
                        else if (charger[j].Bounds.CollidesWith(player.Bounds))
                        {
                            chargerStandby.Add(charger[j]);
                            charger.RemoveAt(j);
                            lives--;
                            state = GameState.Pause;
                            if (j > 0) j--;
                        }
                        else if (charger[j].Bounds.CollidesWith(enemies[i].Bounds) && !dead)
                        {
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
                scoreBucket += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (scoreBucket > 100)
                {
                    score += scoreBucket / 100;
                    scoreBucket -= score * 100;
                }
                base.Update(gameTime);
            } 
            #endregion
            else //game hasn't started or is over
            {

            }
        }

        private void UpdateLeaderboard(string name, int time)
        {
            Leaderboard.AddEntry(name, time);
            Leaderboard.Save();
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
            
            
            foreach(ChargerSprite c in charger)
            {
                c.Draw(gameTime, _spriteBatch);
            }
            player.Draw(gameTime, _spriteBatch);
            foreach (GruntSprite e in enemies)
            {
                if (e.Alive)
                    e.Draw(gameTime, _spriteBatch);
            }
            foreach (Bullet b in bullets)
            {
                b.Draw(gameTime, _spriteBatch);
            }
            switch (state)
            {
                case GameState.Pause:
                    Pause.Draw(_spriteBatch, score, upgrades);
                    break;
                case GameState.Lost:
                    Lost.Draw(_spriteBatch, Leaderboard);
                    break;
                case GameState.Unstarted:
                    break;
                case GameState.InPlay:
                    _spriteBatch.DrawString(_gameFont, "Score: " + score, new Vector2(2, 20), Color.Black);
                    _spriteBatch.DrawString(_gameFont, "Lives: " + lives, new Vector2(2, 40), Color.Black);
                    break;
                default:
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
                b.Position = player.Arms[player.armIndex].BarrelEnd;
                if (player.armIndex == 0)
                {
                    shootTime = 1 + (1 / upgrades[1]);
                }
                else if (player.armIndex == 1)
                {
                    shootTime = 4 - (2/upgrades[2]);
                }
                else if (player.armIndex == 2)
                {
                    shootTime = 3;
                    b.hitCount = 1 + upgrades[3];
                }
                bullets.Add(b);
                shotBullets.RemoveAt(0);
            }
        }
    }
}