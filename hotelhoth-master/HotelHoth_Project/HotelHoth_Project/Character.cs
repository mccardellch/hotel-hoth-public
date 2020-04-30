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

    /// <summary>
    /// This is the parent class of the Player, Enemy, Boss
    /// </summary>
    class Character
    {
        //Field
        protected int health;
        protected int damage;
        protected int speed;

        // Fields defining the texture and position
        protected Texture2D objectTexture;
        protected Rectangle objectPosition;

        // Field for whether or not collectible is active
        protected bool active;

        // Property for health
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        // Property for damage
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        // Property for speed
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        // Property for object texture
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

        // Property for active bool
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        //Parameterized Constructor
        public Character(Texture2D p_objectTexture, Rectangle p_objectPosition)
        {
            objectTexture = p_objectTexture;
            objectPosition = p_objectPosition;
            active = true;
        }

        //Method
        public virtual void Attack() { }
        public virtual bool IsDead() { return false; }

        // Method draws spriteBatch
        public virtual void Draw(SpriteBatch sb)
        {
            if (this.active == true)
            {
                sb.Draw(objectTexture, objectPosition, Color.White);
            }
        }
    }
}
