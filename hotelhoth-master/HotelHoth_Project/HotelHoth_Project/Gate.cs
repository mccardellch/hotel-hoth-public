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
    class Gate
    {
        // Fields defining the texture and position
        private Texture2D gateTexture;
        private Rectangle gatePosition;

        //The gate is opened or not
        private bool active;

        //Property for texture and position
        public Texture2D GateTexture { get => gateTexture; set => gateTexture = value; }
        public Rectangle GatePosition { get => gatePosition; set => gatePosition = value; }
        public int X { get => gatePosition.X; set => gatePosition.Y = value; }
        public int Y { get => gatePosition.Y; set => gatePosition.Y = value; }
        public bool Active { get { return active; } set { active = value; } }
        //Constructor
        public Gate(Texture2D p_objectTexture, Rectangle p_objectPosition)
        {
            gateTexture = p_objectTexture;
            gatePosition = p_objectPosition;
            active = false;
        }

        //Determind opened or not
        public void CheckActive(Player player)
        {
            if (player.Coins >= 3)
            {
                active = true;
            }
        }

        public bool PlayerCollides(Player player)
        {
            CheckActive(player);
            if (active && player.ObjectPosition.Intersects(this.gatePosition))
            {
                return true;
            }
            return false;
        }



        //Method draws spriteBatch
        public virtual void Draw(SpriteBatch sb)
        {
            if (this.active == true)
            {
                //in this part I set the wall as yellow for the gate
                //So the active gate is the yellow wall
                sb.Draw(gateTexture, gatePosition, Color.White);
            }
        }

        ////check the player's position, if the position of player equal to the player, game end
        //public bool EndGame(Player player)
        //{
        //    if (player.ObjectPosition == gatePosition)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
