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
        List<Rectangle> objectSpaces;
        List<Enemy> enemies;
        Random roomSelect;
        private int[][] floorMap;
        List<Sprite> sprites;


        public List<Rectangle> ObjectSpaces { get { return objectSpaces; } }
        public List<Enemy> Enemies { get { return enemies; } }
        public List<Sprite> Sprites { get { return sprites; } }
        /// <summary>
        /// Constructor for Map class, accepts the ContentManager from Game1, so that the class is able to draw the walls
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
            objectSpaces = new List<Rectangle>();
            enemies = new List<Enemy>();
            sprites = new List<Sprite>();
            roomSelect = new Random();
            floorMap = new int[9][];
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
        /// Loads 9 rooms into the floormap array, from the full possibility of levelMaps.
        /// </summary>
        public void LoadFloor()
        {
            for (int i = 0; i < 9; i++)
            {
                floorMap[i] = levelMap[roomSelect.Next(0, levelMap.Count)];        
            }
        }
        public void LoadRoom(ContentManager content)
        {
            int row = 0;
            int column = 0;
            int i = 0;
            foreach (int tile in levelMap[roomNumber])
            {
                if (tile == 6)
                {
                    enemies.Add(new Ghoul(new Vector2(tileHeight * column, tileWidth * row),"Character/Ghoul"));
                   
                    sprites.Add(enemies[i]);
                    i++;
                }
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
            foreach(Enemy enemy in enemies)
            {
                enemy.LoadContent(content);
            }
        }

        /// <summary>
        /// Draws the map to the screen, uses the levelMap list and the spriteBatch object from Game1
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            objectSpaces.Clear();
            int row = 0;
            int column = 0;
            int i = 0;
            foreach (int tile in levelMap[roomNumber])//Current Roomnumber is just set to 0 so it only loads one room. No need to load
                                                      //any other room at the current moment since I do not have any sort of 
                                                      //implementation for transitioning between rooms.
            {
                
                if (tile == 1) //1 is a wall
                {
                   objectSpaces.Add(new Rectangle(column * 64, row * 64, tileWidth, tileHeight));


                    spriteBatch.Draw(wall, objectSpaces[i], Color.White);
                    //Map is 77 tiles, so after it draws row 1 column 0 through 10, it automatically moves on to row 2 column 0
                    i++;
                }
                else if (tile == 4)
                {
                  //  enemies.Add(new Ghoul(new Vector2(64 * column, 64 * row),"Character/Ghoul"));
                    //objectSpaces.Add(new Rectangle(column * 64, row * 64, tileWidth, tileHeight));
                    //i++;
                }
                else if (tile == 5)
                {
                    //i++;
                }
                else if (tile == 6)
                {
                    
                    //i++;
                }
                else if (tile == 7)
                {
                    //i++;
                }

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

        }
    }
}
