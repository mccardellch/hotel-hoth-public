using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HotelHoth_Project
{
    class Enemy : Character
    {
        // Field for attack direction
        private BulletDirection direction;

        // Field for list of enemy bullets
        private List<Bullet> bullets;

        // Property for bullet direction
        public BulletDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        // Property for bullet list
        public List<Bullet> Bullets
        {
            get { return bullets; }
            set { bullets = value; }
        }

        //Constructor
        public Enemy(Texture2D p_objectTexture, Rectangle p_objectPosition, BulletDirection p_direction)
            : base(p_objectTexture, p_objectPosition)
        {
            active = true;
            health = 3;
            speed = 1;
            damage = 1;
            direction = p_direction;
            bullets = new List<Bullet>();
        }

        /// <summary>
        /// The attack method adds a bullet to the enemy's list of bullets
        /// Depending on the enemy's direction, a new bullet instance will be created
        /// No matter what direction the enemy is facing, the bullet will be positioned outside of the player and centered
        /// The bullet direction will effect the BulletMove() method
        /// </summary>
        /// <param name="bulletTexture"> A texture2D is a required paramter to define its image </param>
        public bool Attack(Texture2D bulletTexture)
        {
            // Adds bullet only if count is less than 1
            if (bullets.Count < 2)
            {
                switch (direction)
                {
                    case BulletDirection.Up:
                        bullets.Add(new Bullet(bulletTexture, new Rectangle(objectPosition.X + (objectPosition.Width / 2) - 5, objectPosition.Y - 10, 10, 10), 8, damage, direction));
                        break;
                    case BulletDirection.Down:
                        bullets.Add(new Bullet(bulletTexture, new Rectangle(objectPosition.X + (objectPosition.Width / 2) - 5, objectPosition.Y + objectPosition.Height, 10, 10), 8, damage, direction));
                        break;
                    case BulletDirection.Left:
                        bullets.Add(new Bullet(bulletTexture, new Rectangle(objectPosition.X - 10, objectPosition.Y + (objectPosition.Height / 2) - 5, 10, 10), 8, damage, direction));
                        break;
                    case BulletDirection.Right:
                        bullets.Add(new Bullet(bulletTexture, new Rectangle(objectPosition.X + objectPosition.Width, objectPosition.Y + (objectPosition.Height / 2) - 5, 10, 10), 8, damage, direction));
                        break;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Method simply draws the bullet, called in Game1's draw method
        /// </summary>
        /// <param name="sb"></param>
        public void DrawEnemyBullets(SpriteBatch sb)
        {
            foreach (Bullet b in bullets)
            {
                sb.Draw(b.ObjectTexture, b.ObjectPosition, Color.White);
            }
        }

        /// <summary>
        /// For each bullet in the enemy's bullet list, the position is updated
        /// This method is called in Game1's update method
        /// </summary>
        public void UpdateEnemyBulletPositions(ExternalTool p_levelEditor)
        {
            foreach (Bullet b in bullets)
            {
                b.BulletMove(p_levelEditor);
            }
        }

        /// <summary>
        /// Removes the enemy's bullets from the enemy's bullet list if travelling outside the screen
        /// Or when it becomes inactive due to a collision with either the environment or an enemy
        /// </summary>
        public void UpdateEnemyBullets(List<Enemy> listOfEnemies, GraphicsDevice graphics)
        {
            foreach (Enemy e in listOfEnemies)
            {
                for (int i = e.Bullets.Count - 1; i >= 0; i--)
                {
                    if (e.Bullets[i].ObjectPosition.X + e.Bullets[i].ObjectPosition.Width < 0
                        || e.Bullets[i].ObjectPosition.X > graphics.Viewport.Width
                        || e.Bullets[i].ObjectPosition.Y + e.Bullets[i].ObjectPosition.Height < 0
                        || e.Bullets[i].ObjectPosition.Y > graphics.Viewport.Height
                        || e.Bullets[i].Active == false)
                    {
                        e.Bullets.RemoveAt(i);
                    }
                }
            }
        }

        public override bool IsDead()
        {
            if (health <= 0)
            {
                active = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Walk() { }
    }
}
