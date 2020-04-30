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
    class Interface
    {

        //Fields
        private Rectangle position;
        private Texture2D textureHover;
        private Texture2D texture;
        private bool hover;

        //Properties
        public Rectangle Position { get { return position; } set { position = value; } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public Texture2D TextureHover { get { return textureHover; } set { textureHover = value; } }
        public bool Hover { get { return hover; } set { hover = value; } }

        //Constructor
        public Interface(Rectangle p_position, Texture2D p_texture, Texture2D p_textureHover)
        {
            position = p_position;
            texture = p_texture;
            hover = false;
        }

        public bool ButtonHover(MouseState ms)
        {
            ms = Mouse.GetState();
            if (position.Contains(ms.Position))
            {
                hover = true;
                return true;
            }
            else
            {
                hover = false;
                return false;
            }
        }

        //Override the LoadContent method
        public virtual void LoadContent(Game g, string fileName, string fileNameHover)
        {
            texture = g.Content.Load<Texture2D>(fileName);
            textureHover = g.Content.Load<Texture2D>(fileNameHover);
        }

        //Override the Draw method
        public virtual void Draw(SpriteBatch sb)
        {
            if (hover == true)
            {
                sb.Draw(textureHover, position, Color.White);   //show the hover over texture
            }
            else
            {
                sb.Draw(texture, position, Color.White);       //show the regular texture
            }
        }
    }
}
