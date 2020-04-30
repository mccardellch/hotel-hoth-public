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
    class Boss: Character
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
        public Boss(Texture2D p_objectTexture, Rectangle p_objectPosition, BulletDirection p_direction)
            : base(p_objectTexture, p_objectPosition)
        {
            active = true;
            health = 100;
            speed = 1;
            damage = 1;
            direction = p_direction;
            bullets = new List<Bullet>();
        }

        public bool Attack(Texture2D bulletTexture)
        {
            // Adds bullet only if count is less than 1
            if (bullets.Count < 2)
            {
                

                while (IsDead())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bullets.Add(new Bullet(bulletTexture, new Rectangle(objectPosition.X + (objectPosition.Width / 2) - 5, objectPosition.Y + objectPosition.Height, 10, 10), 8, damage, direction));
                    }
                    System.Threading.Thread.Sleep(5000);
                }

                return true;
            }

            return false;
        }

        public void DrawEnemyBullets(SpriteBatch sb)
        {
            foreach (Bullet b in bullets)
            {
                sb.Draw(b.ObjectTexture, b.ObjectPosition, Color.White);
            }
        }

        public void UpdateEnemyBulletPositions(ExternalTool p_levelEditor)
        {
            foreach (Bullet b in bullets)
            {
                b.BulletMove(p_levelEditor);
            }
        }


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
    }
}
