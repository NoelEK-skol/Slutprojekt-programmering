using Microsoft.Xna.Framework;


namespace Slutprojekt_programmering
{
    public class Platform

    {
        private Rectangle rectangle;



        public bool Collided(Rectangle otherRectangle)
        {
            return otherRectangle.Intersects(this.rectangle);
        }


    }
}