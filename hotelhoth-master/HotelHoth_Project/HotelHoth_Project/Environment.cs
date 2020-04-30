using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HotelHoth_Project
{
    class Environment
    {

        // Fields
        private Texture2D environmentTexture;
        private Rectangle environmentPosition;
        public bool collidesWith;

        // Properties
        public Texture2D EnvironmentTexture { get { return environmentTexture; } set { environmentTexture = value; } }
        public Rectangle EnvironmentPosition { get { return environmentPosition; } set { environmentPosition = value; } }
        public bool CollidesWith { get { return collidesWith; } set { collidesWith = value; } }

        public Environment(Texture2D p_environmentTexture, Rectangle p_environmentPosition)
        {
            environmentPosition = p_environmentPosition;
            environmentTexture = p_environmentTexture;
            collidesWith = false;
        }

        //Collide Method
        public bool Collides(Rectangle rect)
        {
            if (rect.Intersects(this.environmentPosition))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Override Draw Method
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(environmentTexture, environmentPosition, Color.White);
        }

    }
}
