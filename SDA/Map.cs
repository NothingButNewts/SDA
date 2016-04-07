using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace SDA
{
    class Map
    {
        int roomNumber;
        int tileWidth;
        int tileHeight;
        int[] tileMap;
        List<int[]> levelMap;
        DirectoryInfo dirInfo;
        List<byte> byteMap;
        Texture2D wall;
        FileInfo[] files;
        List<Rectangle> wallSpaces;

        public List<Rectangle> WallSpaces { get { return wallSpaces; } }
        /// <summary>
        /// Constructor for Map class, accepts the ContentManager from Game1, so that the class is able to draw the wallsx
        /// </summary>
        /// <param name="content"></param>
        public Map(ContentManager content)
        {
            tileMap = new int[77];
            tileWidth = 64;
            tileHeight = 64;
            levelMap = new List<int[]>();
            dirInfo = new DirectoryInfo("Maps");
            byteMap = new List<byte>();
            roomNumber = 0;
            wall = content.Load<Texture2D>("WorldObjects/Wall");
            files = dirInfo.GetFiles();
            wallSpaces = new List<Rectangle>();
        }

        /// <summary>
        /// Checks each file in the "Maps" directory, if it includes the string "Level" within it, it loads the bytes into the 
        /// byteMap list, and then takes every 4th byte and puts that into the tileMap int array, and adds that into the levelMap list
        /// </summary>
        public void LoadLevels()
        {
            
            foreach(FileInfo file in files)
            {

                byteMap.Clear();
                if (file.Name.Contains("Level")) //Will load all levels into the levelMap List
                {
                    byteMap.InsertRange(0, File.ReadAllBytes("Maps/" + file.Name));
                    for (int i = 0; i < byteMap.Count; i += 4)
                    {
                        tileMap[i / 4] = (byteMap[i]);

                    }
                    levelMap.Add(tileMap);
                }
                }
        }
 
        /// <summary>
        /// Draws the map to the screen, uses the levelMap list and the spriteBatch object from Game1
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            int row = 0;
            int column = 0;
            int i = 0;
            foreach (int tile in levelMap[roomNumber])//Current Roomnumber is just set to 0 so it only loads one room. No need to load
                                                      //any other room at the current moment since I do not have any sort of 
                                                      //implementation for transitioning between rooms.
            {

                if (tile == 1) //1 is a wall
                {
                    wallSpaces.Add(new Rectangle(column*64, row*64,tileWidth,tileHeight));

                    //I'll most likely end up reworking how I handle loading of maps so that I save the rectangles
                    //that are used in the spriteBatch.Draw so that I can use it later for collision detection to check if
                    //a players move would put the model into the wall or inside an enemy, and then refuse that movement.

                    spriteBatch.Draw(wall, wallSpaces[i], Color.White);
                    //Map is 77 tiles, so after it draws row 1 column 0 through 10, it automatically moves on to row 2 column 0
                    if (column == 10)
                    {
                        column = 0;
                        row++;
                        i++;
                    }
                    else
                    {
                        i++;
                        column++;
                    }
                }

                else if (tile == 2) //2 is an empty space
                {
                    
                    if (column == 10)
                    {
                        column = 0;
                        row++;
                    }
                    else
                    {
                        column++;
                    }
                }
                else
                {

                } 
            }
        }      

    }
}
