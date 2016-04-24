using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

/*This level editor is used to create custom maps for Super Dungeon Adventure (name still pending). Once the map is customized to the user's liking 
 *and they click the save button, they are prompted with the option to place a ladder object. Once that is completed, a binary file (.DAT) of the map is created,
 *and each sprite is represented by an integer.
 *Author: Jake Toporoff
 */
namespace LevelEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //attributes. Collapsed to create less clutter
        #region
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D player;
        Texture2D blob;
        Texture2D vamp;
        Texture2D shade;
        Texture2D ghoul;
        Texture2D wall;
        Texture2D save;
        Texture2D ladder;
        SpriteFont font;
        MouseState mState;
        MouseState oldState;
        bool canSave = false; //when true, the save game method is usable
        Texture2D highlight; //golden square that "highlights" selected sprite
        Spaces[,] level = new Spaces[7, 11]; //2D array of level spaces that make up the level
        int selected = 1; //represents the selected sprite, defaults to wall.

        //integer array to represent the selected sprite. Refer to the commented out array below named placeables (save is in there merely as a place holder)
        //8 represents the ladder sprite. I kind of forgot about it until later so it only appears once the user tries to save
        int[] select = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        bool[] clicked = { false, true, false, false, false, false, false, false }; //boolean array to see which sprite is selected. Wall is selected by default.

        //positions of all spaces in the level in order to draw sprites in the right position
        Rectangle[] positions =
        {
        //row 1
        new Rectangle(106, 51, 40, 39), new Rectangle(157, 51, 45, 39), new Rectangle(213, 51, 44, 39), new Rectangle(268, 51, 44, 39), new Rectangle(323, 51, 44, 39), new Rectangle(378, 51, 44, 39),
        new Rectangle(433, 51, 44, 39), new Rectangle(488, 51, 44, 39), new Rectangle(543, 51, 44, 39), new Rectangle(598, 51, 44, 39), new Rectangle(653, 51, 42, 39),
        //row 2
        new Rectangle(106, 101, 40, 42), new Rectangle(157, 101, 45, 42), new Rectangle(213, 101, 44, 42), new Rectangle(268, 101, 44, 42), new Rectangle(323, 101, 44, 42), new Rectangle(378, 101, 44, 42),
        new Rectangle(433, 101, 44, 42), new Rectangle(488, 101, 44, 42), new Rectangle(543, 101, 44, 42), new Rectangle(598, 101, 44, 42), new Rectangle(653, 101, 42, 42),
        //row 3
        new Rectangle(106, 154, 40, 41), new Rectangle(157, 154, 45, 41), new Rectangle(213, 154, 44, 41), new Rectangle(268, 154, 44, 41), new Rectangle(323, 154, 44, 41), new Rectangle(378, 154, 44, 41),
        new Rectangle(433, 154, 44, 41), new Rectangle(488, 154, 44, 41), new Rectangle(543, 154, 44, 41), new Rectangle(598, 154, 44, 41), new Rectangle(653, 154, 42, 41),
        //row 4
        new Rectangle(106, 206, 40, 42), new Rectangle(157, 206, 45, 42), new Rectangle(213, 206, 44, 42), new Rectangle(268, 206, 44, 42), new Rectangle(323, 206, 44, 42), new Rectangle(378, 206, 44, 42),
        new Rectangle(433, 206, 44, 42), new Rectangle(488, 206, 44, 42), new Rectangle(543, 206, 44, 42), new Rectangle(598, 206, 44, 42), new Rectangle(653, 206, 42, 42),
        //row 5
        new Rectangle(106, 259, 40, 41), new Rectangle(157, 259, 45, 41), new Rectangle(213, 259, 44, 41), new Rectangle(268, 259, 44, 41), new Rectangle(323, 259, 44, 41), new Rectangle(378, 259, 44, 41),
        new Rectangle(433, 259, 44, 41), new Rectangle(488, 259, 44, 41), new Rectangle(543, 259, 44, 41), new Rectangle(598, 259, 44, 41), new Rectangle(653, 259, 42, 41),
        //row 6
        new Rectangle(106, 311, 40, 42), new Rectangle(157, 311, 45, 42), new Rectangle(213, 311, 44, 42), new Rectangle(268, 311, 44, 42), new Rectangle(323, 311, 44, 42), new Rectangle(378, 311, 44, 42),
        new Rectangle(433, 311, 44, 42), new Rectangle(488, 311, 44, 42), new Rectangle(543, 311, 44, 42), new Rectangle(598, 311, 44, 42), new Rectangle(653, 311, 42, 42),
        //row 7
        new Rectangle(106, 364, 40, 41), new Rectangle(157, 364, 45, 41), new Rectangle(213, 364, 44, 41), new Rectangle(268, 364, 44, 41), new Rectangle(323, 364, 44, 41), new Rectangle(378, 364, 44, 41),
        new Rectangle(433, 364, 44, 41), new Rectangle(488, 364, 44, 41), new Rectangle(543, 364, 44, 41), new Rectangle(598, 364, 44, 41), new Rectangle(653, 364, 42, 41)
        };

        //array to order the sprites (since I can't actually use references to the main texture2d's, this is here for reference.
        //Texture2D[] placeables = { player, wall, clear, save, vamp, blob, ghoul, shade};
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //Finds out the newest selected sprite(player, wall, blob, etc.) based on where the mouse was clicked
        private void WhichSpriteClicked()
        {
            int previousSelection = 0;

            //checks to see if mouse was clicked
            if (mState.LeftButton == ButtonState.Pressed)
            {
                //clears previous selection
                for (int i = 0; i < clicked.Length; i++)
                {
                    //if the position clicked is not within a sprite's "box", the previously selected
                    //sprite will remain selected.
                    if (clicked[i])
                        previousSelection = i;
                    clicked[i] = false;
                }

                //checks which sprite was clicked, highlights that sprite
                //player spawn
                if (mState.X > 11 && mState.X < 86 && mState.Y > 50 && mState.Y < 125)
                {
                    canSave = false;
                    clicked[0] = true;
                    selected = select[0];
                }
                //wall
                else if (mState.X > 11 && mState.X < 86 && mState.Y > 140 && mState.Y < 215)
                {
                    canSave = false;
                    clicked[1] = true;
                    selected = select[1];
                }
                //clear space
                else if (mState.X > 11 && mState.X < 86 && mState.Y > 230 && mState.Y < 305)
                {
                    canSave = false;
                    clicked[2] = true;
                    selected = select[2];
                }
                //save level
                else if (mState.X > 11 && mState.X < 86 && mState.Y > 320 && mState.Y < 395)
                {
                    canSave = true;
                    clicked[3] = true;
                    selected = select[1];
                }
                //vamp
                else if (mState.X > 708 && mState.X < 783 && mState.Y > 50 && mState.Y < 125)
                {
                    canSave = false;
                    clicked[4] = true;
                    selected = select[4];
                }
                //blob
                else if (mState.X > 708 && mState.X < 783 && mState.Y > 140 && mState.Y < 215)
                {
                    canSave = false;
                    clicked[5] = true;
                    selected = select[5];
                }
                //ghoul
                else if (mState.X > 708 && mState.X < 783 && mState.Y > 230 && mState.Y < 305)
                {
                    canSave = false;
                    clicked[6] = true;
                    selected = select[6];
                }
                //shade
                else if (mState.X > 708 && mState.X < 783 && mState.Y > 320 && mState.Y < 395)
                {
                    canSave = false;
                    clicked[7] = true;
                    selected = select[7];
                }
                else
                {
                    clicked[previousSelection] = true;
                    selected = select[previousSelection];
                }
            }
        }

        //if mouse is hovering over a specific sprite, some information about that sprite 
        //will be displayed at the top of the screen.
        private void Description()
        {
            //Player Description if mouse is hovering over the image
            if (Ez(19, 84, 57, 124))
            {
                spriteBatch.DrawString(font, "Player Spawn", new Vector2(320, 10), Color.Goldenrod);
            }
            //Wall Description
            if (Ez(19, 84, 148, 214))
            {
                spriteBatch.DrawString(font, "Wall", new Vector2(372, 10), Color.Goldenrod);
            }
            //Clear Space Description
            if (Ez(19, 84, 239, 304))
            {
                spriteBatch.DrawString(font, "Clear Space", new Vector2(330, 10), Color.Goldenrod);
            }
            //Save Description
            if (Ez(19, 84, 328, 394))
            {
                spriteBatch.DrawString(font, "Save Level", new Vector2(336, 10), Color.Goldenrod);
            }
            //Vamp Description
            if (Ez(717, 783, 57, 123))
            {
                spriteBatch.DrawString(font, "Spawn Vamp", new Vector2(321, 10), Color.Goldenrod);
            }
            //Blob Description
            if (Ez(717, 783, 148, 213))
            {
                spriteBatch.DrawString(font, "Spawn Blob", new Vector2(328, 10), Color.Goldenrod);
            }
            //Ghoul Description
            if (Ez(717, 783, 239, 303))
            {
                spriteBatch.DrawString(font, "Spawn Ghoul", new Vector2(319, 10), Color.Goldenrod);
            }
            //Shade Description
            if (Ez(717, 783, 328, 393))
            {
                spriteBatch.DrawString(font, "Spawn Shade", new Vector2(316, 10), Color.Goldenrod);
            }
            //Ladder Description
            if (canSave)
            {
                if (Ez(25, 90, 405, 455))
                {
                    spriteBatch.DrawString(font, "Ladder", new Vector2(355, 10), Color.Goldenrod);
                }
                if (Ez(713, 783, 405, 455))
                {
                    spriteBatch.DrawString(font, "Are you sure?", new Vector2(328, 10), Color.Goldenrod);
                }
            }
        }

        //changes selected pixel by redrawing the highlight square in a different position
        private void Highlight()
        {
            if (clicked[0]) //player
                spriteBatch.Draw(highlight, new Rectangle(11, 50, 75, 75), Color.White);

            if (clicked[1]) //wall
                spriteBatch.Draw(highlight, new Rectangle(11, 140, 75, 75), Color.White);

            if (clicked[2]) //clear space
                spriteBatch.Draw(highlight, new Rectangle(11, 230, 75, 75), Color.White);

            if (clicked[3]) //save level
                spriteBatch.Draw(highlight, new Rectangle(11, 320, 75, 75), Color.White);

            if (clicked[4]) //spawn vamp
                spriteBatch.Draw(highlight, new Rectangle(708, 50, 75, 75), Color.White);

            if (clicked[5]) //spawn blob
                spriteBatch.Draw(highlight, new Rectangle(708, 140, 75, 75), Color.White);

            if (clicked[6]) //spawn ghoul
                spriteBatch.Draw(highlight, new Rectangle(708, 230, 75, 75), Color.White);

            if (clicked[7]) //spawn shade
                spriteBatch.Draw(highlight, new Rectangle(708, 320, 75, 75), Color.White);
        }

        //used to check mouse position over the actual spaces within the level in / checks if click occurs to place a sprite
        private void Populate()
        {
            //spriteBatch.DrawString(font, "X: " + mState.X + " Y: " + mState.Y, new Vector2(350, 450), Color.Red); <-- Testing Code
            //column 1
            if (Ez(102, 149, 47, 92)) Place(0, 0);
            if (Ez(102, 149, 97, 146)) Place(0, 1);
            if (Ez(102, 149, 150, 198)) Place(0, 2);
            if (Ez(102, 149, 202, 251)) Place(0, 3);
            if (Ez(102, 149, 255, 303)) Place(0, 4);
            if (Ez(102, 149, 307, 356)) Place(0, 5);
            if (Ez(102, 149, 360, 408)) Place(0, 6);

            //column 2
            if (Ez(153, 205, 47, 92)) Place(1, 0);
            if (Ez(153, 205, 97, 146)) Place(1, 1);
            if (Ez(153, 205, 150, 198)) Place(1, 2);
            if (Ez(153, 205, 202, 251)) Place(1, 3);
            if (Ez(153, 205, 255, 303)) Place(1, 4);
            if (Ez(153, 205, 307, 356)) Place(1, 5);
            if (Ez(153, 205, 360, 408)) Place(1, 6);

            //column 3
            if (Ez(209, 260, 47, 92)) Place(2, 0);
            if (Ez(209, 260, 97, 146)) Place(2, 1);
            if (Ez(209, 260, 150, 198)) Place(2, 2);
            if (Ez(209, 260, 202, 251)) Place(2, 3);
            if (Ez(209, 260, 255, 303)) Place(2, 4);
            if (Ez(209, 260, 307, 356)) Place(2, 5);
            if (Ez(209, 260, 360, 408)) Place(2, 6);

            //column 4
            if (Ez(264, 315, 47, 92)) Place(3, 0);
            if (Ez(264, 315, 97, 146)) Place(3, 1);
            if (Ez(264, 315, 150, 198)) Place(3, 2);
            if (Ez(264, 315, 202, 251)) Place(3, 3);
            if (Ez(264, 315, 255, 303)) Place(3, 4);
            if (Ez(264, 315, 307, 356)) Place(3, 5);
            if (Ez(264, 315, 360, 408)) Place(3, 6);

            //column 5
            if (Ez(319, 370, 47, 92)) Place(4, 0);
            if (Ez(319, 370, 97, 146)) Place(4, 1);
            if (Ez(319, 370, 150, 198)) Place(4, 2);
            if (Ez(319, 370, 202, 251)) Place(4, 3);
            if (Ez(319, 370, 255, 303)) Place(4, 4);
            if (Ez(319, 370, 307, 356)) Place(4, 5);
            if (Ez(319, 370, 360, 408)) Place(4, 6);

            //column 6
            if (Ez(374, 425, 47, 92)) Place(5, 0);
            if (Ez(374, 425, 97, 146)) Place(5, 1);
            if (Ez(374, 425, 150, 198)) Place(5, 2);
            if (Ez(374, 425, 202, 251)) Place(5, 3);
            if (Ez(374, 425, 255, 303)) Place(5, 4);
            if (Ez(374, 425, 307, 356)) Place(5, 5);
            if (Ez(374, 425, 360, 408)) Place(5, 6);

            //column 7
            if (Ez(429, 480, 47, 92)) Place(6, 0);
            if (Ez(429, 480, 97, 146)) Place(6, 1);
            if (Ez(429, 480, 150, 198)) Place(6, 2);
            if (Ez(429, 480, 202, 251)) Place(6, 3);
            if (Ez(429, 480, 255, 303)) Place(6, 4);
            if (Ez(429, 480, 307, 356)) Place(6, 5);
            if (Ez(429, 480, 360, 408)) Place(6, 6);

            //column 8
            if (Ez(484, 535, 47, 92)) Place(7, 0);
            if (Ez(484, 535, 97, 146)) Place(7, 1);
            if (Ez(484, 535, 150, 198)) Place(7, 2);
            if (Ez(484, 535, 202, 251)) Place(7, 3);
            if (Ez(484, 535, 255, 303)) Place(7, 4);
            if (Ez(484, 535, 307, 356)) Place(7, 5);
            if (Ez(484, 535, 360, 408)) Place(7, 6);

            //column 9
            if (Ez(539, 590, 47, 92)) Place(8, 0);
            if (Ez(539, 590, 97, 146)) Place(8, 1);
            if (Ez(539, 590, 150, 198)) Place(8, 2);
            if (Ez(539, 590, 202, 251)) Place(8, 3);
            if (Ez(539, 590, 255, 303)) Place(8, 4);
            if (Ez(539, 590, 307, 356)) Place(8, 5);
            if (Ez(539, 590, 360, 408)) Place(8, 6);

            //column 10
            if (Ez(594, 645, 47, 92)) Place(9, 0);
            if (Ez(594, 645, 97, 146)) Place(9, 1);
            if (Ez(594, 645, 150, 198)) Place(9, 2);
            if (Ez(594, 645, 202, 251)) Place(9, 3);
            if (Ez(594, 645, 255, 303)) Place(9, 4);
            if (Ez(594, 645, 307, 356)) Place(9, 5);
            if (Ez(594, 645, 360, 408)) Place(9, 6);

            //column 11
            if (Ez(649, 698, 47, 92)) Place(10, 0);
            if (Ez(649, 698, 97, 146)) Place(10, 1);
            if (Ez(649, 698, 150, 198)) Place(10, 2);
            if (Ez(649, 698, 202, 251)) Place(10, 3);
            if (Ez(649, 698, 255, 303)) Place(10, 4);
            if (Ez(649, 698, 307, 356)) Place(10, 5);
            if (Ez(649, 698, 360, 408)) Place(10, 6);
        }

        //Actaully draws the sprite in the grid.
        //parameters: row and column of the space and the number referencing the selected sprite
        private void Place(int col, int row)
        {
            if (!canSave)
            {
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    switch (selected)
                    {
                        case 0: //player spawn
                            {
                                if (checkNumSpawns(0))
                                {
                                    level[row, col].BinNum = 0;
                                    level[row, col].Draw = true;
                                }
                                break;
                            }
                        case 1: //wall
                            {
                                level[row, col].BinNum = 1;
                                level[row, col].Draw = true;
                                break;
                            }
                        case 2: //clear space
                            {
                                level[row, col].BinNum = 2;
                                level[row, col].Draw = false;
                                break;
                            }
                        case 3: //save game
                            {
                                break;
                            }
                        case 4: //spawn vamp
                            {
                                level[row, col].BinNum = 4;
                                level[row, col].Draw = true;
                                break;
                            }
                        case 5: //spawn blob
                            {
                                level[row, col].BinNum = 5;
                                level[row, col].Draw = true;
                                break;
                            }
                        case 6: //spawn ghoul
                            {
                                level[row, col].BinNum = 6;
                                level[row, col].Draw = true;
                                break;
                            }
                        case 7: //spawn shade
                            {
                                level[row, col].BinNum = 7;
                                level[row, col].Draw = true;
                                break;
                            }
                    }
                }
            }
            else
            {
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    if (checkNumSpawns(8))
                    {
                        level[row, col].BinNum = 8;
                        level[row, col].Draw = true;
                    }
                }
            }
        }

        //updates each space on the board with the appropriate character sprite
        private void UpdateLevel()
        {
            for (int i = 0; i < level.GetLength(0); i++)
            {
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    if (level[i, j].Draw)
                    {
                        switch (level[i, j].BinNum)
                        {
                            case 0: //player spawn
                                {
                                    spriteBatch.Draw(player, level[i, j].GetPos(), Color.White);
                                    break;
                                }
                            case 1: //wall
                                {
                                    spriteBatch.Draw(wall, level[i, j].GetPos(), Color.White);
                                    break;
                                }
                            case 2: //clear space
                                {
                                    break;
                                }
                            case 3: //save game
                                {
                                    break;
                                }
                            case 4: //spawn vamp
                                {
                                    spriteBatch.Draw(vamp, level[i, j].GetPos(), Color.White);
                                    break;
                                }
                            case 5: //spawn blob
                                {
                                    spriteBatch.Draw(blob, level[i, j].GetPos(), Color.White);
                                    break;
                                }
                            case 6: //spawn ghoul
                                {
                                    spriteBatch.Draw(ghoul, level[i, j].GetPos(), Color.White);
                                    break;
                                }
                            case 7: //spawn shade
                                {
                                    spriteBatch.Draw(shade, level[i, j].GetPos(), Color.White);
                                    break;
                                }
                            case 8: //ladder
                                {
                                    spriteBatch.Draw(ladder, level[i, j].GetPos(), Color.White);
                                    break;
                                }
                        }
                    }
                }
            }
        }

        //Adds two new buttons, one to add a ladder to the level and one to confirm the save game
        private void SaveGame()
        {
            spriteBatch.Draw(ladder, new Rectangle(30, 418, 60, 50), Color.White);
            if (Ez(25, 90, 405, 455))
            {
                spriteBatch.Draw(highlight, new Rectangle(17, 403, 70, 70), Color.White);
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    spriteBatch.Draw(highlight, new Rectangle(17, 403, 70, 70), Color.Blue);
                    selected = select[8];
                }
            }
            spriteBatch.Draw(save, new Rectangle(718, 418, 60, 50), Color.White);
            if (Ez(713, 783, 405, 455))
            {
                spriteBatch.Draw(highlight, new Rectangle(705, 403, 78, 70), Color.White);
                //checks for a click to avoid saving 10+ times while holding down the mouse
                if (oldState.LeftButton == ButtonState.Released && mState.LeftButton == ButtonState.Pressed)
                {
                    spriteBatch.Draw(highlight, new Rectangle(705, 403, 78, 70), Color.Blue);
                    ConvertLevel();
                }
                oldState = mState;
            }
        }

        //converts the level into a binary file
        private void ConvertLevel()
        {
            int i = 0;
            int lvlNum = 0;
            int gap = 0;
            string lvlNm = "";

            //this is used to create a new file every time the save function is used by making the file name one number higher than the last
            //ex: Level 0.dat, Level 1.dat, Level 2.dat, etc.
            string[] files = Directory.GetFiles(".");
            foreach (string file in files)
            {
                lvlNm = "";
                if (file.Contains("Level ")) //if the file is a saved level
                {
                    i++;
                    
                    for (int num = 8; num < file.Length - 4; num++) //gets the number of the level in the file name
                    {
                        lvlNm += file[num];
                    }

                    if (gap != int.Parse(lvlNm)) //checks to see if the number in the file name matches the number it should be (prevents skips in numbers which would cause the same file to
                    {                            //keep getting overwriten over and over again
                        lvlNum = gap;
                        break;
                    }
                    else //if the file has the correct number, continue checking the other files and set levelnum equal to the next number in line
                    {
                        gap++;
                        lvlNum = i;
                    }
                }
            }
            

            try {
                Stream lvl = File.OpenWrite("Level " + lvlNum + ".dat");
                BinaryWriter output = new BinaryWriter(lvl);
                for (int j = 0; j < level.GetLength(0); j++)
                {
                    for (int k = 0; k < level.GetLength(1); k++)
                    {
                        output.Write(level[j, k].BinNum);
                    }
                }
                output.Close();
            }
            catch(Exception ex)
            {
                return;
            }
        }

        //since only one player spawn / one ladder can exist within a given level, this method checks to see if one already exists to prevent duplicates
        private bool checkNumSpawns(int sprite)
        {
            for (int i = 0; i < level.GetLength(0); i++)
            {
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    if (level[i, j].BinNum == sprite)
                        return false;
                }
            }
            return true;
        }

        /*It's name Ez since I won't have to type as much this way. Also the method really just is to make 
         *my job easier so I don't have to type out this method body 100+ times.
         *parameters: the bounds of each space in the level grid
         *returns: whether or not the mouse is over the space or not
         */
        public Boolean Ez(int x1, int x2, int y1, int y2)
        {
            if (mState.X > x1 && mState.X < x2 && mState.Y > y1 && mState.Y < y2) return true;
            return false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true; //shows mouse cursor in editor

            //populating array of spaces
            for (int i = 0; i < level.GetLength(0); i++)
            {
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    level[i, j] = new Spaces(false, 2, positions[i * 11 + j]);
                }
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //background
            background = Content.Load<Texture2D>("Level Editor");

            //player (and clickable object) sprites
            player = Content.Load<Texture2D>("Character Sprites/Player");
            blob = Content.Load<Texture2D>("Character Sprites/Blob");
            ghoul = Content.Load<Texture2D>("Character Sprites/Ghoul");
            shade = Content.Load<Texture2D>("Character Sprites/Shade");
            vamp = Content.Load<Texture2D>("Character Sprites/Vamp");
            save = Content.Load<Texture2D>("Character Sprites/Save");
            wall = Content.Load<Texture2D>("Character Sprites/Wall");
            ladder = Content.Load<Texture2D>("Character Sprites/Ladder");
            highlight = Content.Load<Texture2D>("Highlight");

            //font
            font = Content.Load<SpriteFont>("mainFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            mState = Mouse.GetState();

            WhichSpriteClicked();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //background
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);

            //left side images
            spriteBatch.Draw(player, new Rectangle(23, 63, 60, 60), Color.White);
            spriteBatch.Draw(wall, new Rectangle(33, 161, 40, 40), Color.White);
            spriteBatch.Draw(save, new Rectangle(23, 335, 60, 60), Color.White);

            //right side images
            spriteBatch.Draw(vamp, new Rectangle(720, 63, 60, 60), Color.White);
            spriteBatch.Draw(blob, new Rectangle(720, 148, 60, 60), Color.White);
            spriteBatch.Draw(ghoul, new Rectangle(720, 240, 60, 60), Color.White);
            spriteBatch.Draw(shade, new Rectangle(720, 330, 60, 60), Color.White);

            Description();

            Highlight();

            Populate();

            UpdateLevel();

            if (canSave) SaveGame();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    //represents spaces within the level. 
    class Spaces
    {
        private bool draw; //boolean to see whether the space is drawable or not (not drawable if space is clear)
        private int binNum; // number used when the level is converted into a binary file
        Rectangle pos; //position of where the sprite will be drawn in each space

        public bool Draw { get { return draw; } set { draw = value; } }

        public Rectangle GetPos() { return pos; }

        public int BinNum { get { return binNum; } set { binNum = value; } }

        public Spaces(bool draw, int bin, Rectangle pos)
        {
            this.draw = draw;
            binNum = bin;
            this.pos = pos;
        }

    }
}
