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
        InPlay,
        Pause, //used for the menu/pause screen
        GameOver
    }

    public class TimeGame : Game
    {
        #region Variables and Constructor
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        SpriteFont _gameFont;
        List<SoundEffect> _soundEffects = new List<SoundEffect>();

        /// <summary>
        /// The width of the game world
        /// </summary>
        public static int GAME_WIDTH = 64 * 12; //we want to scale game to be full screen

        /// <summary>
        /// The height of the game world
        /// </summary>
        public static int GAME_HEIGHT = 64 * 8;

        BoundingRectangle gameBoundTop = new BoundingRectangle(0, -32, GAME_WIDTH + 128, 0);
        BoundingRectangle gameBoundBottom = new BoundingRectangle(0, GAME_HEIGHT - 128, GAME_WIDTH + 128, 0);
        BoundingRectangle gameBoundFront = new BoundingRectangle(-64, 0, 1, GAME_HEIGHT);
        BoundingRectangle gameBoundBack = new BoundingRectangle(64 + GAME_WIDTH, 0, 1, GAME_HEIGHT);

        public bool HasBeenInitialized = false;

        KeyboardState keyboardState;
        KeyboardState previousKeyboard;

        Leaderboard Leaderboard;

        Matrix translation = new Matrix();
        double translationTimer;

        PlayerSprite player;
        Tilemap _tilemap;

        List<GruntSprite> enemies;
        List<GruntSprite> deadEnemies;

        List<ChargerSprite> charger;
        List<ChargerSprite> chargerStandby;

        List<PowerUpSprite> powerUps;
        List<PowerUpSprite> standbyPowerUps;

        List<Crate> crates;
        List<Crate> standbyCrates;

        List<Bullet> bullets;
        List<Bullet> shotBullets;

        int lives;
        int score;
        int scoreBucket;
        double gunTimer;
        double shootTime;
        int difficulty;
        double difficultyTimer;
        double chargerTimer;
        float bulletRot;
        Vector2 bulletDir;

        GameState state = GameState.InPlay;
        int costModifier = 5;
        int[] upgrades = { 1, 1, 1, 1 };
        double chargerWaitTime = 4.5;
        Random ran = new Random();

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
            Window.Title = "Gun Runner";
        }
        #endregion

        #region New Game Methods
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
            // [4] = Block Break
            _soundEffects.Add(Content.Load<SoundEffect>("Sounds/Block Break 1"));
            // [5] = Health Lost
            _soundEffects.Add(Content.Load<SoundEffect>("Sounds/error_style_4_echo_001"));
            // [6] = Powerup Gained
            _soundEffects.Add(Content.Load<SoundEffect>("Sounds/SFX_Powerup_34"));


            foreach (GruntSprite e in enemies)
            {
                e.LoadContent(this.Content);
            }
            foreach (GruntSprite e in deadEnemies)
            {
                e.LoadContent(this.Content);
            }
            foreach (ChargerSprite c in chargerStandby)
            {
                c.LoadContent(Content);
            }
            foreach (Bullet b in shotBullets)
            {
                b.LoadContent(Content);
            }
            foreach (PowerUpSprite p in standbyPowerUps)
            {
                p.LoadContent(Content);
            }
            foreach (Crate c in crates)
            {
                c.LoadContent(Content);
            }
            foreach (Crate c in standbyCrates)
            {
                c.LoadContent(Content);
            }

        }

        /// <summary>
        /// can also be used for restart
        /// </summary>
        protected override void Initialize()
        {
            NewGame();
            HasBeenInitialized = true;
            base.Initialize();
        }

        /// <summary>
        /// Used in initializing original game, as well as restarting
        /// </summary>
        private void NewGame()
        {
            state = GameState.InPlay;
            lives = 3;
            score = 0;
            scoreBucket = 0;
            for (int i = 0; i < upgrades.Length; i++)
                upgrades[i] = 1;
            keyboardState = new KeyboardState();
            previousKeyboard = new KeyboardState();
            gunTimer = 0;
            shootTime = 2;
            difficulty = 2;
            chargerTimer = 0;
            ran = new Random();

            Texture2D[] armTextures = new Texture2D[3];
            if (HasBeenInitialized)
            {
                armTextures[0] = player.Arms[0].Texture;
                armTextures[1] = player.Arms[1].Texture;
                armTextures[2] = player.Arms[2].Texture;
                player = new PlayerSprite(player.Texture, armTextures);
            }
            else
            {
                armTextures[0] = null;
                armTextures[1] = null;
                armTextures[2] = null;
                player = new PlayerSprite(null, armTextures);
                _tilemap = new Tilemap(GAME_WIDTH, GAME_HEIGHT);
            }

            Leaderboard = new Leaderboard();
            Leaderboard.Load();

            NewGruntsHelper();
            NewChargersHelper();
            NewBulletsHelper();
            NewPowerUpsHelper();
            NewCratesHelper();
        }

        private void NewGruntsHelper()
        {
            Random r = new Random();
            GruntSprite ene;
            Texture2D texture = null;
            if (HasBeenInitialized)
                texture = enemies[0].Texture;
            enemies = new List<GruntSprite>();
            deadEnemies = new List<GruntSprite>();

            for (int i = 0; i < 10; i++)
            {
                int outerBounds = GAME_WIDTH + 60;
                Vector2 pos = new Vector2(r.Next(outerBounds, outerBounds + 100), r.Next(0, GAME_HEIGHT));
                ene = new GruntSprite(pos, player, texture);
                if (i < difficulty)
                    enemies.Add(ene);
                else
                {
                    ene.Alive = false;
                    deadEnemies.Add(ene);
                }
            }
        }

        private void NewChargersHelper()
        {
            Texture2D texture = null;
            List<SoundEffect> sounds = null;
            if (HasBeenInitialized)
            {
                texture = chargerStandby[0].Texture;
                sounds = chargerStandby[0].SoundEffects;
            }
            charger = new List<ChargerSprite>();
            chargerStandby = new List<ChargerSprite>();

            for (int i = 0; i < 10; i++)
            {
                ChargerSprite charge = new ChargerSprite(texture, sounds);
                chargerStandby.Add(charge);
            }
        }

        private void NewBulletsHelper()
        {
            Texture2D texture = null;
            if (HasBeenInitialized) //could trip if there are no bullets
                texture = shotBullets[0].Texture;
            bullets = new List<Bullet>();
            shotBullets = new List<Bullet>();

            for (int i = 0; i < 50; i++)
            {
                Bullet b = new Bullet(texture);
                shotBullets.Add(b);
            }
        }

        private void NewPowerUpsHelper()
        {
            Texture2D texture = null;
            if (HasBeenInitialized)
                texture = standbyCrates[0].Texture;
            powerUps = new List<PowerUpSprite>();
            standbyPowerUps = new List<PowerUpSprite>();

            for (int i = 0; i < 10; i++)
            {
                PowerUpSprite p = new PowerUpSprite(new Vector2(64 * 12, 225), new Vector2(-1, 0), 50, texture);
                standbyPowerUps.Add(p);
            }
        }

        private void NewCratesHelper()
        {
            if (HasBeenInitialized)
            {
                foreach (Crate crate in crates)
                {
                    crate.ResetCrate();   
                    standbyCrates.Add(crate);
                }
                crates.Clear();
            }
            else
            {
                crates = new List<Crate>();
                standbyCrates = new List<Crate>();

                for (int i = 0; i < 10; i++)
                {
                    Crate c = new Crate(this, CrateType.DarkCross, 2, GAME_WIDTH);
                    if (i < 1) crates.Add(c);
                    else standbyCrates.Add(c);
                }
            }
        }
        #endregion

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
                    case GameState.GameOver:
                        NewGame();
                        break;
                    default:
                        lives--;
                        _soundEffects[5].Play();
                        state = GameState.Pause;
                        break;
                }
            }

            if (lives < 1 && state != GameState.GameOver)//maybe move this to gameplay?
            {
                state = GameState.GameOver;
                UpdateLeaderboard(Environment.UserName, score);//could this move to the lose section?
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
                //currentMouse = Mouse.GetState();
                
                difficultyTimer += gameTime.ElapsedGameTime.TotalSeconds / 15;//combine with scoring at bottom into helper method - handleScoring
                if (difficultyTimer > difficulty && difficulty < 50)
                {
                    difficultyTimer = 0;
                    difficulty++;
                    int k = ran.Next(1, 4);
                    standbyCrates[0].ResetCrate(k);
                    standbyCrates[0].IsActive = true;
                    crates.Add(standbyCrates[0]);
                    standbyCrates.RemoveAt(0);
                }

                if (chargerTimer > chargerWaitTime) //break me into a helper method - private void handleCharger(chargerTime, chargerWaitTime)
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

                if (gunTimer > shootTime) //break me into a hempler method - private void handleGunShots(gunTimer, shootTime)
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

                if (enemies.Count < difficulty) //break me into a helper method - private void handleGoons(enemyCount, difficulty)
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

                player.Update(gameTime);//maybe move to take care of sequencing?
                
                for (int i = 0; i < powerUps.Count; i++)//helper method?
                {
                    if (powerUps[i].IsActive)
                    {
                        powerUps[i].Update(gameTime);
                        if (powerUps[i].Bounds.CollidesWith(player.Bounds))
                        {
                            lives++;
                            _soundEffects[6].Play();
                            powerUps[i].Position = new Vector2(-10, -10);
                            powerUps[i].IsActive = false;

                            standbyPowerUps.Add(powerUps[i]);
                            powerUps.RemoveAt(i);
                            i--;
                        }
                        else if (powerUps[i].Bounds.CollidesWith(gameBoundFront))
                        {
                            powerUps[i].Position = new Vector2(-10, -10);
                            powerUps[i].IsActive = false;

                            standbyPowerUps.Add(powerUps[i]);
                            powerUps.RemoveAt(i);
                            i--;
                        }

                    }
                }

                for (int k = 0; k < crates.Count; k++)//helper method?
                {
                    if (crates[k].IsActive)
                    {
                        crates[k].Update(gameTime);
                        if (crates[k].Bounds.CollidesWith(player.Bounds))
                        {
                            lives--;
                            _soundEffects[4].Play();
                            crates[k].IsActive = false;
                            standbyCrates.Add(crates[k]);
                            crates.RemoveAt(k);
                            state = GameState.Pause;
                            k--;
                        }
                        else if (crates[k].Bounds.X + 50 < 0)
                        {
                            crates[k].IsActive = false;
                            standbyCrates.Add(crates[k]);
                            crates.RemoveAt(k);
                            k--;
                        }
                        bool dead = false;
                        for (int j = 0; j < bullets.Count; j++)
                        {
                            bullets[j].Update(gameTime);
                            if (bullets[j].Bounds.CollidesWith(crates[k].Bounds) && !dead)
                            {
                                _soundEffects[4].Play();
                                bullets[j].hitCount -= 1;
                                if (bullets[j].hitCount <= 0)
                                {
                                    bullets[j].Shot = true;
                                    bullets[j].Position = new Vector2(-10, -10);
                                    shotBullets.Add(bullets[j]);
                                    bullets.RemoveAt(j);
                                    if (j > 0) j--;
                                }
                                dead = true;
                            }
                        }
                        if (dead)
                        {
                            crates[k].IsActive = false;
                            standbyPowerUps[0].Position = new Vector2(crates[k].Bounds.X, crates[k].Bounds.Y);
                            powerUps.Add(standbyPowerUps[0]);
                            standbyPowerUps.RemoveAt(0);
                            standbyCrates.Add(crates[k]);
                            crates.RemoveAt(k);
                            
                            k--;
                        }
                    }
                }

                for (int i = 0; i < enemies.Count; i++)//helper method?
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
                            _soundEffects[5].Play();
                            state = GameState.Pause;
                            i--;
                        }
                        else if (enemies[i].Bounds.CollidesWith(gameBoundFront))
                        {
                            enemies[i].Position = new Vector2(-10, -10);
                            enemies[i].Alive = false;

                            deadEnemies.Add(enemies[i]);
                            enemies.Remove(enemies[i]);
                            i--;
                        }
                    }

                    if (i < 0) //could probably do a break - this is messy
                        i = 0;

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
                            _soundEffects[5].Play();
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

        #region Helper Methods
        private void ShootGun(float rot, Vector2 dir)
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
                    shootTime = 4 - (2 / upgrades[2]);
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
        #endregion

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

            _spriteBatch.Begin(transformMatrix: translation); //why do we use two spritebatch calls? -could we just not feed the transform matrix?
            _tilemap.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin();
            
            player.Draw(gameTime, _spriteBatch);
            foreach (GruntSprite e in enemies)
            {
                if (e.Alive)
                    e.Draw(gameTime, _spriteBatch);
            }
            foreach (ChargerSprite c in charger)
            {
                c.Draw(gameTime, _spriteBatch);
            }
            foreach (Bullet b in bullets)
            {
                b.Draw(gameTime, _spriteBatch);
            }
            foreach (Crate c in crates)
            {
                if(c.IsActive)
                    c.Draw(gameTime, _spriteBatch);
            }
            foreach (PowerUpSprite p in powerUps)
            {
                 p.Draw(gameTime, _spriteBatch);
            }
            switch (state)
            {
                case GameState.Pause:
                    Pause.Draw(_spriteBatch, score, upgrades);
                    break;
                case GameState.GameOver:
                    Lost.Draw(_spriteBatch, Leaderboard);
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
    }
}