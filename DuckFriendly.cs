using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slutprojekt_programmering
{
    public class Friendly
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle hitbox;
        public Rectangle Hitbox
        {
            get {return hitbox;}
        }

        public Friendly(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            hitbox = new Rectangle((int)position.X, (int)position.Y, 80, 80);
        }

        public void Update()
        {
            float speed = 150;
            position.X -= speed * 1f/60f;
            hitbox.Location = position.ToPoint();
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox, Color.White);
        }


    }
}