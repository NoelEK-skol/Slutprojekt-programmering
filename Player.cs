using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slutprojekt_programmering
{
    public class Player
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle hitbox;
        private KeyboardState newKState;
        private KeyboardState oldKState;
        private Texture2D bulletTexture;
        const float GRAVITY = 30f;
        Vector2 velocity;
        private bool canJump = true;
        SoundEffectInstance effect;
        SoundEffect shooteffect;

        private List<Bullet> bullets = new List<Bullet>();
        public List<Bullet> Bullets{
            get{return bullets;}
        }

        public Player(Texture2D texture, Vector2 position, int pixelSize, Texture2D bulletTexture, SoundEffectInstance effect, SoundEffect shooteffect){
            this.texture = texture;
            this.bulletTexture = bulletTexture;
            this.position = position;
            this.effect = effect;
            this.shooteffect = shooteffect;
            hitbox = new Rectangle((int)position.X,(int)position.Y,(int)(pixelSize*0.75), pixelSize);
        }

        private void jump(){
            if(canJump){
                velocity.Y = -10;
                canJump = false;
            }
        }

        public void Update(){
            newKState = Keyboard.GetState();
            Move();
            Shoot();
            oldKState = newKState;

            if(position.Y >= 300){
                velocity.Y = 0;
                position.Y = 300;
                canJump = true;
            }

            foreach(Bullet b in bullets){
                b.Update();
            }
        }

        public Rectangle Hitbox{
            get{return hitbox;}
        }

        private void Shoot(){
            if(newKState.IsKeyDown(Keys.E) && oldKState.IsKeyUp(Keys.E)){
                bullets.Add(new(bulletTexture, position + new Vector2(-5,25)));
                shooteffect.Play();
            }
        }


        private void Move(){
            if(newKState.IsKeyDown(Keys.A) && position.X > -60){
                position.X -= 4;
            }
            if(newKState.IsKeyDown(Keys.D) && position.X < 860 - hitbox.Width){
                position.X += 4;
            }
            if(newKState.IsKeyDown(Keys.Space) && oldKState.IsKeyUp(Keys.Space)){
                jump();
                effect.Play();
            }
            position.Y += velocity.Y;
            velocity.Y += GRAVITY * 1f/60f;

            hitbox.Location = position.ToPoint();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox, Color.White);
            foreach(Bullet b in bullets){
                b.Draw(spriteBatch);
            }
        }


    }
}