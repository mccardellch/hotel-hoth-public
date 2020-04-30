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
    abstract class Collectible
    {
        // Fields defining the texture and position
        protected Texture2D objectTexture;
        protected Rectangle objectPosition;

        // Field for whether or not collectible is active
        protected bool active;

        // Property for texture
        public Texture2D ObjectTexture
        {
            get { return objectTexture; }
            set { objectTexture = value; }
        }

        // Property for position
        public Rectangle ObjectPosition
        {
            get { return objectPosition; }
            set { objectPosition = value; }
        }

        // Property for the X value of position
        public int X
        {
            get { return objectPosition.X; }
            set { objectPosition.X = value; }
        }

        // Property for the Y value of position
        public int Y
        {
            get { return objectPosition.Y; }
            set { objectPosition.Y = value; }
        }

        // Property for active boolean
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        // Parameterized constructor passing through a texture and position
        public Collectible(Texture2D p_objectTexture, Rectangle p_objectPosition)
        {
            objectTexture = p_objectTexture;
            objectPosition = p_objectPosition;
            active = true;
            objectPosition.Width = 20;
            objectPosition.Height = 20;
        }

        // Method draws spriteBatch
        public virtual void Draw(SpriteBatch sb)
        {
            if (this.active == true)
            {
                sb.Draw(objectTexture, objectPosition, Color.White);
            }
        }

        // Checks if a player interacts with the collectible
        public abstract bool CheckCollision(Player playerObject);
    }
}
