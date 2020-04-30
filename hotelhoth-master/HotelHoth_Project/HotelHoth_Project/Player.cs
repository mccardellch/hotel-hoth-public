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
    class Player: Character
    {
        // Field for coins
        private int coins;

        // Field for attack direction
        private BulletDirection direction;

        // Field for list of player bullets
        private List<Bullet> bullets;

        private PlayerState playerCurrentState;
        private PlayerState playerPreviousState;

        private Rectangle perimeter; //the perimeter edges of the player

        // Property for the amount of coins the player has collected
        public int Coins { get { return coins; } set { coins = value; } }

        // Property for bullet list
        public List<Bullet> Bullets
        {
            get { return bullets; }
            set { bullets = value; }
        }

        // Property for direction
        public BulletDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Rectangle Perimeter { get { return perimeter; } set { perimeter = value; } }
   
        public PlayerState PlayerCurrentState { get { return playerCurrentState; } set { playerCurrentState = value; } }
        public PlayerState PlayerPreviousState { get { return playerPreviousState; } set { playerPreviousState = value; } }

        // Constructor
        public Player(Texture2D p_objectTexture, Rectangle p_objectPosition)
            : base(p_objectTexture, p_objectPosition)
        {
            active = true;
            health = 3;
            speed = 1;
            damage = 1;
            coins = 0;
            direction = BulletDirection.Right;
            bullets = new List<Bullet>();
            perimeter = new Rectangle(objectPosition.X, objectPosition.Y, 35, 45);
        }

        /// <summary>
        /// The attack method adds a bullet to the player's list of bullets
        /// Depending on the player's direction, a new bullet instance will be created
        /// No matter what direction the player is moving, the bullet will be positioned outside of the player and centered
        /// The bullet direction will effect the BulletMove() method
        /// </summary>
        /// <param name="bulletTexture"> A texture2D is a required paramter to define its image </param>
        public bool Attack(Texture2D bulletTexture)
        {
            // Adds bullet only if count is less than 3
            if (bullets.Count < 3)
            {
                switch (direction)
                {
                    case BulletDirection.Up:
                        bullets.Add(new Bullet(bulletTexture, new Rectangle(objectPosition.X + (objectPosition.Width / 2) - 5, objectPosition.Y - 10, 10, 10), 10, damage, direction));
                        break;
                    case BulletDirection.Down:
                        bullets.Add(new Bullet(bulletTexture, new Rectangle(objectPosition.X + (objectPosition.Width / 2) - 5, objectPosition.Y + objectPosition.Height, 10, 10), 10, damage, direction));
                        break;
                    case BulletDirection.Left:
                        bullets.Add(new Bullet(bulletTexture, new Rectangle(objectPosition.X - 10, objectPosition.Y + (objectPosition.Height / 2) - 5, 10, 10), 10, damage, direction));
                        break;
                    case BulletDirection.Right:
                        bullets.Add(new Bullet(bulletTexture, new Rectangle(objectPosition.X + objectPosition.Width, objectPosition.Y + (objectPosition.Height / 2) - 5, 10, 10), 10, damage, direction));
                        break;
                }

                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method simply draws the bullet, called in Game1's draw method
        /// </summary>
        /// <param name="sb"></param>
        public void DrawPlayerBullets(SpriteBatch sb)
        {
            foreach (Bullet b in bullets)
            {
                sb.Draw(b.ObjectTexture, b.ObjectPosition, Color.White);
            }
        }

        /// <summary>
        /// For each bullet in the player's bullet list, the position is updated
        /// This method is called in Game1's update method
        /// </summary>
        public void UpdatePlayerBulletPositions(ExternalTool p_levelEditor)
        {
            foreach (Bullet b in bullets)
            {
                b.BulletMove(p_levelEditor);
            }
        }

        
        // Checks if the Player is dead or not
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


        public override void Draw(SpriteBatch sb)
        {
            Vector2 spriteOrigin = new Vector2(22.5f, 17.5f);
            Vector2 spritePosition = new Vector2(objectPosition.X, objectPosition.Y);

            //sb.Draw(objectTexture, objectPosition, Color.White);

            switch (direction)
            {
                case BulletDirection.Up:
                    sb.Draw(objectTexture, spritePosition, null, Color.White, -90, spriteOrigin, 0.15f, SpriteEffects.None, 0);
                    break;
                case BulletDirection.Down:
                    sb.Draw(objectTexture, spritePosition, null, Color.White, 90, spriteOrigin, 0.15f, SpriteEffects.None, 0);
                    break;
                case BulletDirection.Left:
                    sb.Draw(objectTexture, spritePosition, null, Color.White, 0, spriteOrigin, 0.15f, SpriteEffects.FlipHorizontally, 0);
                    break;
                case BulletDirection.Right:
                    sb.Draw(objectTexture, spritePosition, null, Color.White, 0, spriteOrigin, 0.15f, SpriteEffects.None, 0);
                    break;
            }
        }

        public bool BoxCollision(List<Environment> listWalls)
        {
            //trying to find out how to code collisions from certain sides of player rectangle.
            //look in checkPlayerState()
            bool collide = false;
            for (int i = 0; i < listWalls.Count; i++)
            {
                if (listWalls[i].Collides(ObjectPosition))
                {
                    collide = true;
                    return true;
                }
            }
            return false;
        }
        
        // Resets Player Stats
        public void ResetPlayer()
        {
            active = true;
            health = 3;
            speed = 1;
            damage = 2;
            coins = 0;
            direction = BulletDirection.Right;
            bullets = new List<Bullet>();
            objectPosition = new Rectangle(50, 50, 35, 35);
        }

        /// <summary>
        /// This method prevents player from walking through an enemy
        /// Takes in a Player object and list of Enemy objects
        /// </summary>
        public void PlayerToEnemyCollisions(List<Enemy> listOfEnemies)
        {
            for (int i = 0; i < listOfEnemies.Count; i++) //loops through each enemy in the list
            {
                if (ObjectPosition.Intersects(listOfEnemies[i].ObjectPosition)) //checks if the player is colliding with the enemy at the index 
                {
                    switch (Direction) //checks the direction of the player
                    {
                        case BulletDirection.Up:
                            ObjectPosition = new Rectangle(ObjectPosition.X, ObjectPosition.Y + 50, ObjectPosition.Width, ObjectPosition.Height);
                            Health--;
                            break;
                        case BulletDirection.Down:
                            ObjectPosition = new Rectangle(ObjectPosition.X, ObjectPosition.Y - 50, ObjectPosition.Width, ObjectPosition.Height);
                            Health--;
                            break;
                        case BulletDirection.Left:
                            ObjectPosition = new Rectangle(ObjectPosition.X + 50, ObjectPosition.Y, ObjectPosition.Width, ObjectPosition.Height);
                            Health--;
                            break;
                        case BulletDirection.Right:
                            ObjectPosition = new Rectangle(ObjectPosition.X - 50, ObjectPosition.Y, ObjectPosition.Width, ObjectPosition.Height);
                            Health--;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes the player bullets from the player's bullet list if travelling outside the screen
        /// Or when it becomes inactive due to a collision with either the environment or an enemy
        /// </summary>
        public void UpdatePlayerBullets(GraphicsDevice graphics)
        {
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                if (Bullets[i].ObjectPosition.X + Bullets[i].ObjectPosition.Width < 0
                    || Bullets[i].ObjectPosition.X > graphics.Viewport.Width
                    || Bullets[i].ObjectPosition.Y + Bullets[i].ObjectPosition.Height < 0
                    || Bullets[i].ObjectPosition.Y > graphics.Viewport.Height
                    || Bullets[i].Active == false)
                {
                    Bullets.RemoveAt(i);
                }
            }
        }

        //Temporary method for moving single sprite character
        // Temporary method for moving single sprite character
        // Depending on the movement direction, the bullet direction is also changed
        public void PlayerMove(ExternalTool p_levelEditor)
        {
            //creating keyboard state for user input 
            KeyboardState kb = Keyboard.GetState();
            KeyboardState prevKb = Keyboard.GetState();

            //will move character when certain keys are pressed
            //add/subtract 22 to  
            if (kb.IsKeyDown(Keys.D))   //Right
            {
                objectPosition.X += 5;
                direction = BulletDirection.Right;
                playerCurrentState = PlayerState.WalkE;
            }
            if (kb.IsKeyDown(Keys.W))   // Up
            {
                objectPosition.Y -= 5;
                direction = BulletDirection.Up;
                playerCurrentState = PlayerState.WalkN;
            }
            if (kb.IsKeyDown(Keys.S))   // Down
            {
                objectPosition.Y += 5;
                direction = BulletDirection.Down;
                playerCurrentState = PlayerState.WalkS;
            }
            if (kb.IsKeyDown(Keys.A))   // Left
            {
                objectPosition.X -= 5;
                direction = BulletDirection.Left;
                playerCurrentState = PlayerState.WalkW;
            }

            //IF COLLIDING WITH WALL, THE SPRITE POSITION GOES IN THE OPPOSITE DIRECTION 
            if (kb.IsKeyDown(Keys.D) && BoxCollision(p_levelEditor.ListWalls) == true)   //Right
            {
                objectPosition.X -= 5;
                playerCurrentState = PlayerState.WalkE;
            }
            if (kb.IsKeyDown(Keys.W) && BoxCollision(p_levelEditor.ListWalls) == true)   // Up
            {
                objectPosition.Y += 5;
                playerCurrentState = PlayerState.WalkN;
            }
            if (kb.IsKeyDown(Keys.S) && BoxCollision(p_levelEditor.ListWalls) == true)   // Down
            {
                objectPosition.Y -= 5;
                playerCurrentState = PlayerState.WalkS;
            }
            if (kb.IsKeyDown(Keys.A) && BoxCollision(p_levelEditor.ListWalls) == true)   // Left
            {
                objectPosition.X += 5;
                playerCurrentState = PlayerState.WalkW;
            }

            switch (playerCurrentState)
            {
                case PlayerState.FaceW:
                    if (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D) && kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S))
                    {
                        playerCurrentState = PlayerState.FaceW;
                    }
                    prevKb = kb;
                    break;

                case PlayerState.WalkW:
                    if (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D) && kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S))
                    {
                        playerCurrentState = PlayerState.FaceW;
                    }
                    prevKb = kb;
                    break;

                case PlayerState.FaceE:
                    if (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D) && kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S))
                    {
                        playerCurrentState = PlayerState.FaceE;
                    }
                    prevKb = kb;
                    break;

                case PlayerState.WalkE:
                    if (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D) && kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S))
                    {
                        playerCurrentState = PlayerState.FaceE;
                    }
                    prevKb = kb;
                    break;

                case PlayerState.FaceN:
                    if (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D) && kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S))
                    {
                        playerCurrentState = PlayerState.FaceN;
                    }
                    prevKb = kb;
                    break;

                case PlayerState.WalkN:
                    if (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D) && kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S))
                    {
                        playerCurrentState = PlayerState.FaceN;
                    }
                    prevKb = kb;
                    break;

                case PlayerState.FaceS:
                    if (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D) && kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S))
                    {
                        playerCurrentState = PlayerState.FaceS;
                    }
                    prevKb = kb;
                    break;

                case PlayerState.WalkS:
                    if (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D) && kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S))
                    {
                        playerCurrentState = PlayerState.FaceS;
                    }
                    prevKb = kb;
                    break;

            }
        }
    }
}
