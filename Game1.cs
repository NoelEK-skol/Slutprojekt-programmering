﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Slutprojekt_programmering;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player player;
    private Texture2D backgroundTexture;
    private Texture2D explorer;
    private Texture2D fire;
    private Texture2D moneky;
    Song theme;
    SoundEffect effect;
    SoundEffect shooteffect;


    private List<Enemy> enemies = new List<Enemy>();

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
        explorer = Content.Load<Texture2D>("character");
        fire = Content.Load<Texture2D>("FireBall1");
        theme = Content.Load<Song>("battleThemeA");
        moneky = Content.Load<Texture2D>("Moneky1-removebg-preview");
        MediaPlayer.Volume = 0.1f;
        MediaPlayer.Play(theme);
        MediaPlayer.IsRepeating = true;
        effect = Content.Load<SoundEffect>("Jump__009");
        SoundEffectInstance effectInstance = effect.CreateInstance();
        effectInstance.Volume = 0.5f;
        shooteffect = Content.Load<SoundEffect>("Snare__004");
        player = new(explorer, new Vector2(250, 300), 150, fire, effectInstance, shooteffect);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        

        // TODO: Add your update logic here

        EnemyBulletCollision();
        player.Update();

        foreach(Enemy enemy in enemies){
            enemy.Update();
        }
        SpawnEnemy();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        Rectangle bgRect = new(0,0, 800, 480);
        _spriteBatch.Draw(backgroundTexture, bgRect, Color.White);
        player.Draw(_spriteBatch);
        foreach(Enemy enemy in enemies){
            enemy.Draw(_spriteBatch);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }


    private void SpawnEnemy(){
        Random rand = new Random();
        int value = rand.Next(1, 101);
        int spawnChancePercent = (int)1f;
        if(value <= spawnChancePercent)
            enemies.Add(new Enemy(moneky, new(1000, 300)));
    }


    private void EnemyBulletCollision(){
        for(int i = 0; i < enemies.Count; i++){
            for(int j = 0; j < player.Bullets.Count; j++){
                if(enemies[i].Hitbox.Intersects(player.Bullets[j].Hitbox)){
                    enemies.RemoveAt(i);
                    player.Bullets.RemoveAt(j);
                    i--;
                    break;
                }
            }
        }
    }
}
