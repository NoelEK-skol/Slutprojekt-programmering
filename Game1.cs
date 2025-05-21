using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.DirectWrite;
using SharpDX.MediaFoundation;
namespace Slutprojekt_programmering;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player player;
    private Texture2D backgroundTexture;
    private Texture2D backgroundTextureMeny;
    private Texture2D backgroundTextureGameOver;
    private Texture2D explorer;
    private Texture2D fire;
    private Texture2D moneky;
    private Texture2D monkey2;
    private Texture2D duck1;
    private Texture2D duck2;
    private Texture2D heart;
    private Texture2D platformShort;
    private Texture2D platformLong;
    private Texture2D stege;
    private int HP = 3;
    private Song theme;
    private SoundEffect effect;
    private SoundEffect shooteffect;
    private SoundEffect BulletHit;
    private SoundEffect PlayerHit;
    private SoundEffectInstance PlayerHitInstance;
    private GameStates _gameState;
    private SpriteFont Meny;
    private int timer;
    private int SpawnTimer; //gör så fiender inte kan spawna i varandra och med lite mellanrum
    private bool ExtraHP;

    public enum GameStates
    {
        Menu,
        Playing,
        Paused,
        GameOver,
    }

    private List<Enemy> enemies = new List<Enemy>();
    private List<Friendly> friendlies = new List<Friendly>();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        backgroundTexture = Content.Load<Texture2D>("Backgrund");
        backgroundTextureMeny = Content.Load<Texture2D>("StartMenyBackgrund");
        backgroundTextureGameOver = Content.Load<Texture2D>("GameOverScreen");
        explorer = Content.Load<Texture2D>("character");
        fire = Content.Load<Texture2D>("FireBall1");
        theme = Content.Load<Song>("battleThemeA");
        moneky = Content.Load<Texture2D>("Moneky1-removebg-preview");
        monkey2 = Content.Load<Texture2D>("pixel-art-bird-apa");
        duck1 = Content.Load<Texture2D>("DuckFriendly");
        duck2 = Content.Load<Texture2D>("DuckFriendlyFly");
        heart = Content.Load<Texture2D>("PixelH");
        stege = Content.Load<Texture2D>("Stege");
        platformShort = Content.Load<Texture2D>("ShortTile");
        platformLong = Content.Load<Texture2D>("LongTile");
        MediaPlayer.Volume = 0.1f;
        MediaPlayer.Play(theme);
        MediaPlayer.IsRepeating = true;
        effect = Content.Load<SoundEffect>("Jump__009");
        SoundEffectInstance effectInstance = effect.CreateInstance();
        effectInstance.Volume = 0.5f;
        shooteffect = Content.Load<SoundEffect>("Snare__004");
        BulletHit = Content.Load<SoundEffect>("Punch1__004");
        PlayerHit = Content.Load<SoundEffect>("Explosion3__009");
        PlayerHitInstance = PlayerHit.CreateInstance();
        PlayerHitInstance.Volume = 0.7f;
        player = new(explorer, new Vector2(250, 300), 150, fire, effectInstance, shooteffect);
        Meny = Content.Load<SpriteFont>("File");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();



        // TODO: Add your update logic here

        if (_gameState == GameStates.Menu)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _gameState = GameStates.Playing;
                timer = 0;
                SpawnTimer = 0;
            }
        }
        if (_gameState == GameStates.GameOver)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                _gameState = GameStates.Menu;
                enemies.Clear();
                friendlies.Clear();
                HP = 3;                         //reset HP
                player.position.X = 250;        //reset spelar position
                player.position.Y = 300;
                player.Bullets.Clear();         //reset bullet
                ExtraHP = false;
            }
        }

        else if (_gameState == GameStates.Playing)
        {

            EnemyBulletCollision();
            player.Update();
            PlayerCollision();
            timer ++;
            SpawnTimer ++;

            foreach (Enemy enemy in enemies)
            {
                enemy.Update();
                if (enemy.ShouldExit()) _gameState = GameStates.GameOver;
            }
            SpawnEnemy();
            SpawnEnemy2();

            foreach (Friendly friendly in friendlies)
            {
                friendly.Update();
            }
            SpawnFriendly();
            SpawnFriendly2();

            base.Update(gameTime);
        }

    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here


        if (_gameState == GameStates.Menu)
        {
            //rita ut menyn

            Rectangle bgRect = new(0, 0, 800, 480);
            _spriteBatch.Draw(backgroundTextureMeny, bgRect, Color.White);
            _spriteBatch.DrawString(Meny, "Detta är en meny \nSkjut (\"E\") aporna innan de tar sig till andra sidan\nTryck \"Space\" för att spela \n(OBS akta ankorna)", new Vector2(225, 100), Color.Azure);
        }
        else if (_gameState == GameStates.Playing)
        {
            Rectangle bgRect = new(0, 0, 800, 480);
            _spriteBatch.Draw(backgroundTexture, bgRect, Color.White);
            _spriteBatch.Draw(platformShort, new Rectangle(300, 290, 200, 50), Color.White);
            _spriteBatch.Draw(platformShort, new Rectangle(550, 200, 200, 50), Color.White);
            _spriteBatch.Draw(platformLong, new Rectangle(-5, 200, 200, 50), Color.White);
            _spriteBatch.Draw(stege, new Rectangle(-70, 180, 250, 250), Color.White);
            _spriteBatch.DrawString(Meny, (timer / 60).ToString(), new Vector2(710, 10), Color.Black);

            Random rand = new Random();                                 // spawna in +HP
            int value = rand.Next(1, 501);
            int spawnChancePercent = (int)1f;
            if (value <= spawnChancePercent)
                ExtraHP = true;
                if(ExtraHP == true)
            {
            _spriteBatch.Draw(heart, new Rectangle(625, 325, 40, 40), Color.White);         //att göra, när spelare nuddar extrea HP. HP ska gå upp.
            }


            player.Draw(_spriteBatch);
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(_spriteBatch);
            }

            foreach (Friendly friendly in friendlies)
            {
                friendly.Draw(_spriteBatch);
            }


            for (int i = 0; i < HP; i++)
            {
                _spriteBatch.Draw(heart, new Rectangle(50 * i, 0, 50, 50), Color.White);
            }

        }
        else
        {
            Rectangle bgRect = new(0, 0, 800, 480);
            _spriteBatch.Draw(backgroundTextureGameOver, bgRect, Color.White);
            _spriteBatch.DrawString(Meny, $"Du förlora!\nTryck \"r\" för att gå tillbaks till meny\nDu överlevde i", new Vector2(255, 100), Color.Azure);
            _spriteBatch.DrawString(Meny, $"{timer / 60} s", new Vector2(390, 138), Color.Blue);
        }
        


        _spriteBatch.End();
        base.Draw(gameTime);
    }


    private void SpawnEnemy()
    {
        Random rand = new Random();
        int value = rand.Next(1, 101);
        int spawnChancePercent = (int)1f;
        if (value <= spawnChancePercent && SpawnTimer >= 15)
        {
            enemies.Add(new Enemy(moneky, new(1000, 300)));
            SpawnTimer = 0;
        }
    }

    private void SpawnEnemy2()
    {
        Random rand = new Random();
        int value = rand.Next(1, 101);
        int spawnChancePercent = (int)1f;
        if (value <= spawnChancePercent && SpawnTimer >= 15)
        {
            enemies.Add(new Enemy(monkey2, new(1000, 60)));
            SpawnTimer = 0;
        }
    }

    private void SpawnFriendly()
    {
        Random rand = new Random();
        int value = rand.Next(1, 501);
        int spawnChancePercent = (int)1f; 
        if (value <= spawnChancePercent)
            friendlies.Add(new Friendly(duck1, new(1000, 360)));
    }

    private void SpawnFriendly2()
    {
        Random rand = new Random();
        int value = rand.Next(1, 501);
        int spawnChancePercent = (int)1f;
        if (value <= spawnChancePercent)
            friendlies.Add(new Friendly(duck2, new(1000, 60)));
    }

    private void EnemyBulletCollision()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = 0; j < player.Bullets.Count; j++)
            {
                if (enemies[i].Hitbox.Intersects(player.Bullets[j].Hitbox))
                {
                    enemies.RemoveAt(i);
                    player.Bullets.RemoveAt(j);
                    i--;
                    BulletHit.Play();
                    break;
                }
            }
        }

        for (int i = 0; i< friendlies.Count; i++)
        {
            for (int j = 0; j < player.Bullets.Count; j++)
            {
                if(friendlies[i].Hitbox.Intersects(player.Bullets[j].Hitbox))
                {
                    friendlies.RemoveAt(i);
                    player.Bullets.RemoveAt(j);
                    i--;
                    HP--;
                    if (HP <= 0)
                    {
                        _gameState = GameStates.GameOver;
                    }
                    break;
                }
            }
        }
    }


    private void PlayerCollision()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].Hitbox.Intersects(player.Hitbox))
            {
                HP--;
                enemies.RemoveAt(i);
                i--;
                PlayerHitInstance.Play();
                if (HP <= 0)
                {
                    _gameState = GameStates.GameOver;
                }
            }
        }
    }
}


// saker att göra:
//avstånd mellan ankor och apor
// sätt att få tillbaks HP?
// (leaderboard ifall tid över)