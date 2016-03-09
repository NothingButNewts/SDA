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
        }

        public void LoadLevels()
        {
            
            foreach(FileInfo file in files)
            {

                byteMap.Clear();
                if (file.Name.Contains("Level"))
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
 
        public void Draw(SpriteBatch spriteBatch)
        {
            int row = 0;
            int column = 0;
            foreach (int tile in levelMap[roomNumber])
            {

                if (tile == 1)
                {
                    spriteBatch.Draw(wall, new Rectangle(column * 64, 
                        row * 64, tileWidth, tileHeight), Color.White);
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
                else if (tile == 2)
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
