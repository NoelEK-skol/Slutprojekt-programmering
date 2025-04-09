using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slutprojekt_programmering
{
    public class Bullet
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle hitbox;

        public Rectangle Hitbox{
            get{return hitbox;}
        }

        public Bullet(Texture2D texture, Vector2 spawnPosition){
            this.texture = texture;
            position = spawnPosition;
            hitbox = new Rectangle((int)position.X,(int)position.Y,30,40);
        }

        public void Update(){
            float speed = 150;
            position.X += speed * 1f/60f;

            hitbox.Location = position.ToPoint();
        }

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, hitbox, Color.Orange);
        }
    }
}