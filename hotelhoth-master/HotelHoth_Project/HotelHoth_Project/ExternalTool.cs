using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HotelHoth_Project
{
    class ExternalTool
    {

        //private int gameWidth;
        //private int gameHeight;

        //3:5 ratio for screen - Referencing LevelFile1.txt for these dimensions
        private const int arrayHeight = 36; // ORIGINALLY 6
        private const int arrayWidth = 60;  // ORIGINALLY 10

        //Fields
        private string[,] my2dArray = new string[arrayWidth, arrayHeight];
        private List<Environment> listWalls;

        //Properties
        public List<Environment> ListWalls
        {
            get { return listWalls; }
            set { listWalls = value; }
        }
        public string[,] GetArray //return the 2d array
        {
            get { return my2dArray; }
        }
        public int GetArrayHeight //return the array height of the LevelFile1.txt
        {
            get { return arrayHeight; }
        }
        public int GetArrayWidth //return the array width of the LevelFile1.txt
        {
            get { return arrayWidth; }
        }

        //CONSTRUCTOR
        public ExternalTool()
        {
            listWalls = new List<Environment>();
        }

        //Methods
        public void Reader()
        {
            StreamReader reader = null;
            try
            {
                int count = 0;
                foreach (string line in File.ReadLines(@"../../../../LevelFile1.txt"))
                {
                    string s = line;
                    char[] mychars = s.ToCharArray();
                    for (int i = 0; i < s.Length; i++)
                    {
                        my2dArray[i, count] = mychars[i].ToString();
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with file : " + ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

        }

        public List<Environment> PopulateWallList(int viewWidth, int viewHeight, Texture2D p_environmentTexture)
        {
            for (int i = 0; i < arrayWidth; i++)
            {
                for (int j = 0; j < arrayHeight; j++)
                {
                    if (my2dArray[i, j] == "X" || my2dArray[i, j] == "x")
                    {
                        Rectangle position = new Rectangle(i * (viewWidth / arrayWidth), j * (viewHeight / arrayHeight), 20, 20);
                        listWalls.Add(new Environment(p_environmentTexture, position));
                    }
                }
            }

            return listWalls;
        }
        
        public int SpriteCount()
        {
            return -1;
            //returns number of sprites
        }

        public int Xincrease(int screenWidth)
        {
            //the amount x increases by each time a sprite is drawn
            int xinc = screenWidth / arrayWidth;
            return xinc;
        }

        public int Yincrease(int screenHeight) //put GraphicsDevice.ViewPort.Height                                    
        {
            //the amount y increases each time a sprite is drawn
            int yinc = screenHeight / arrayHeight; //value to increase y by each time
            return yinc;
        }



    }
}
