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
    class Weapon : Collectible
    {
        // Weapon class inherits parent collectible constructor
        public Weapon(Texture2D p_objectTexture, Rectangle p_objectPosition)
            : base(p_objectTexture, p_objectPosition)
        {
            active = true;
        }

        // Checks if a player intersects with the collectible
        public override bool CheckCollision(Player playerObject)
        {
            if (this.ObjectPosition.Intersects(playerObject.ObjectPosition) && playerObject.Damage <= 3)
            {
                active = false;
                playerObject.Damage++;
                Console.WriteLine("Weapon Collected");
                return true;
            }
            return false;
        }
    }
}
