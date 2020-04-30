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
    class Heart : Collectible
    {
        // Heart class inherits parent collectible constructor
        public Heart(Texture2D p_objectTexture, Rectangle p_objectPosition)
            : base(p_objectTexture, p_objectPosition)
        {
            active = true;          
        }

        // Checks if a player intersects with the collectible
        public override bool CheckCollision(Player playerObject)
        {
            if (this.ObjectPosition.Intersects(playerObject.ObjectPosition) && playerObject.Health < 5)
            {
                active = false;
                playerObject.Health = playerObject.Health + 1;
                Console.WriteLine("Heart Collected");
                return true;
            }
            return false;
        }
    }
}
