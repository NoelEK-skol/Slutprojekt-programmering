using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slutprojekt_programmering
{
    public class Button
    {
        private readonly Texture2D texture;
        private Rectangle hitbox;
        private Color shade = Color.White;
        private bool clicked;
        

        public Button(Texture2D t, Rectangle h)
        {
            texture = t;
            hitbox = h;
        }

        public void Update()
        {
            MouseState mousestate = Mouse.GetState();
            Rectangle cursor = new(mousestate.Position.X, mousestate.Position.Y, 1, 1);

            if (cursor.Intersects(hitbox))
            {
                shade = Color.DarkGray;
                if (mousestate.LeftButton == ButtonState.Pressed)
                {
                    ButtonClicked();
                    clicked = true;
                }
                else
                {
                    clicked = false;
                }
            }
            else
            {
                shade = Color.White;
                clicked = false;
            }
        }

        public bool ButtonClicked()
        {
            return clicked;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox, shade);
        }



    }
}