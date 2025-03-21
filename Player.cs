using System;
using System.Numerics;
using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slutprojekt_programmering
{
    public class Player
    {
        private Texture2D texture;
        private Microsoft.Xna.Framework.Vector2 position;
        private Rectangle hitbox;
        private KeyboardState newKState;
        private KeyboardState oldKState;
        const float GRAVITY = 30f;
        Microsoft.Xna.Framework.Vector2 velocity;
        private bool canJump = true;

        public Player(Texture2D texture, Microsoft.Xna.Framework.Vector2 position, int pixelSize){
            this.texture = texture;
            this.position = position;
            hitbox = new Rectangle((int)position.X,(int)position.Y,(int)(pixelSize*1.5), pixelSize);
        }

        private void jump(){
            if(canJump){
                velocity.Y = -10;
                canJump = false;
            }
        }

        public void Update(){
            newKState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            Move();
            oldKState = newKState;

            if(position.Y >= 300){
                velocity.Y = 0;
                position.Y = 300;
                canJump = true;
            }
        }


        private void Move(){
            if(newKState.IsKeyDown(Keys.A) && position.X > -60){
                position.X -= 4;
            }
            if(newKState.IsKeyDown(Keys.D) && position.X < 860 - hitbox.Width){
                position.X += 4;
            }
            if(newKState.IsKeyDown(Keys.Space)){
                jump();
            }
            position.Y += velocity.Y;
            velocity.Y += GRAVITY * 1f/60f;

            hitbox.Location = position.ToPoint();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox, Color.White);
        }


        public Rectangle Hitbox{
            get{return hitbox;}
        }
    }
}