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
    class Coin : Collectible
    {
        // Weapon class inherits parent collectible constructor
        public Coin(Texture2D p_objectTexture, Rectangle p_objectPosition)
            : base(p_objectTexture, p_objectPosition)
        {
            active = true;
        }

        // Checks if a player intersects with the collectible
        // Replace Collectible with player
        public override bool CheckCollision(Player playerObject)
        {
            if (this.ObjectPosition.Intersects(playerObject.ObjectPosition))
            {
                active = false;
                playerObject.Coins++;
                return true;
            }
            return false;
        }
    }
}
