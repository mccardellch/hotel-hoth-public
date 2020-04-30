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
    // Enum for bullet direction, dictates bullet motion
    public enum BulletDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    class Bullet
    {
        // Fields defining the texture and position
        private Texture2D objectTexture;
        private Rectangle objectPosition;

        // Field for whether or not collectible is active
        private bool active;

        // Fields for speed of bullet and damage it deals
        private int speed;
        private int damage;

        // Field for bullet direction
        private BulletDirection direction;

        // Property for bullet direction
        public BulletDirection Direction
        {
            get { return direction; }
        }

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

        // Property for speed int
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        // Property for speed int
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        // Parameterized constructor passing through a texture and position
        public Bullet(Texture2D p_objectTexture, Rectangle p_objectPosition, int p_speed, int p_damage, BulletDirection p_direction)
        {
            objectTexture = p_objectTexture;
            objectPosition = p_objectPosition;
            speed = p_speed;
            damage = p_damage;
            active = true;
            direction = p_direction;
        }

        // Method draws spriteBatch
        public virtual void Draw(SpriteBatch sb)
        {
            if (this.active == true)
            {
                sb.Draw(objectTexture, objectPosition, Color.White);
            }
        }

        // Checks if a character interacts with the bullet
        public bool CheckCollision(Character check)
        {
            if (this.ObjectPosition.Intersects(check.ObjectPosition))
            {
                active = false;
                check.Health -= damage;
                return true;
            }

            else
            {
                return false;
            }
        }

        // Checks bullet collision with walls
        public bool boxCollision(List<Environment> listWalls)
        {
            //trying to find out how to code collisions from certain sides of player rectangle.
            //look in checkPlayerState()
            bool collide = false;
            for (int i = 0; i < listWalls.Count; i++)
            {
                if (listWalls[i].Collides(ObjectPosition))
                {
                    collide = true;
                }
            }
            if (collide == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This method is used in Game1's update method
        /// It tells which direction the bullet will move based on it predefined enumeration state
        /// </summary>
        public void BulletMove(ExternalTool p_levelEditor)
        {
            // Switch statement uses the current bullet direction to define its location operation
            // Changes in position are defined by the bullet's speed
            switch (direction)
            {
                case BulletDirection.Up:
                    this.objectPosition.Y -= speed;
                    break;
                case BulletDirection.Down:
                    this.objectPosition.Y += speed;
                    break;
                case BulletDirection.Left:
                    this.objectPosition.X -= speed;
                    break;
                case BulletDirection.Right:
                    this.objectPosition.X += speed;
                    break;
            }

            //IF COLLIDING WITH WALL, THE BULLET BECOMES INACTIVE 
            if (boxCollision(p_levelEditor.ListWalls) == true)
            {
                active = false;
            }
        }
    }
}
