using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace SDA
{
    class Player : Sprite   
    {
        int health; //Maximum health the player has
        int dexterity; //Dexterity measurement, might effect movement or crit chance, not positive
        int strength; //Will effect damage the player does
        int defense; //Reduction to the damage that the player takes
        int exp; //Player's current exp, changes with each monster kill 
        int expToLevel; //Player's exp that is needed to advance to the next level
        int level; //Players current level
        int vitality; //Stat to effect how much max health the player has
        int damage; //Damage that the player does, based on weapon and strength
        KeyboardState oldKBState; //Becomes equal to the previous currentKBState
        KeyboardState currentKBState; //Checks the current keyboard state
        public bool playerTurn; //Bool to determine if it is the player's turn or not, changes on enemy turns and player turns
        bool canMove;
        DirectionFacing currentDirection;
        enum DirectionFacing { Up, Down, Left, Right };
        Map map = new Map();

        public KeyboardState OldKBState { get { return oldKBState; } }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Player(Vector2 startPos, string asset):base(startPos, asset)
        {
            currentDirection = DirectionFacing.Up;
            dexterity = 1;
            strength = 1;
            exp = 0;
            vitality = 1;
            health = 100;
            expToLevel = 100;
            level = 1;
            playerTurn = true;
            canMove = true;         
        }
        /// <summary>
        /// Method to move the character, uses input to move character one tile
        /// Checks if the key was pressed during the currentKBState vs. the oldKBState
        /// Up, left, right and down
        /// </summary>
        public void Move(List<Rectangle> walls,List<Enemy> enemies)
        {
            playerTurn = true;
            Rectangle tempSize= size;
            KeyboardState currentKBState = Keyboard.GetState();
            
            if (oldKBState.IsKeyUp(Keys.W) && currentKBState.IsKeyDown(Keys.W))
            {
                tempSize = new Rectangle(size.X, size.Y - 64, size.Width, size.Height);
                playerTurn = false;
                currentDirection = DirectionFacing.Up;
            }
            else if (oldKBState.IsKeyUp(Keys.A) && currentKBState.IsKeyDown(Keys.A))
            {
                tempSize = new Rectangle(size.X - 64, size.Y, size.Width, size.Height);
                playerTurn = false;
                currentDirection = DirectionFacing.Left;
            }
            else if (oldKBState.IsKeyUp(Keys.D) && currentKBState.IsKeyDown(Keys.D))
            {
                tempSize = new Rectangle(size.X + 64, size.Y, size.Width, size.Height);
                playerTurn = false;
                currentDirection = DirectionFacing.Right;
            }
            else if (oldKBState.IsKeyUp(Keys.S) && currentKBState.IsKeyDown(Keys.S))
            {
                tempSize = new Rectangle(size.X, size.Y + 64, size.Width, size.Height);
                playerTurn = false;
                currentDirection = DirectionFacing.Down;
            }
            oldKBState = currentKBState;
            foreach (Rectangle item in walls)
            {
                if (canMove == true)
                {
                    if (item.Intersects(tempSize))
                    {
                        canMove = false;

                    }
                }
            }

            

            foreach(Enemy enemy in enemies)
            {
                if (canMove == true)
                {
                    if (enemy.size.Intersects(size))
                    {
                        canMove = false;
                    }                                                                                                                                                         
                }
            }
            if (canMove == true && CheckOuterWalls(tempSize.X, tempSize.Y))

            {
                size = tempSize;
            }
            else
            {
                canMove = true;
            }
        }
        /// <summary>
        /// Called when the player's Exp is greater than or equal to expToLevel
        /// Increments Level by 1, scales expToLevel up, and reduces exp by expToLevel to reset
        /// </summary>
        public void Level()
        {
            level++;
            expToLevel = (int)(expToLevel * 1.5);
            exp = exp - expToLevel;
        }

        public int Attack(List<Enemy> enemies)
        {
            switch (currentDirection)
            {
                case DirectionFacing.Up:
                   
                    break;
                case DirectionFacing.Left:
                    break;
                case DirectionFacing.Right:
                    break;
                case DirectionFacing.Down:
                    break;
                default:
                    break;
            }
            return damage;
        }

        //handles player collision with outer walls that surround the map. Takes x and y coordinates as parameters. If the player is able to move, returns true.
        public bool CheckOuterWalls(int x, int y)
        {
            if(x < 64)
            {
                if (map.Doors[0] && y == 256) return true;
                return false;
            }
            if(y < 64)
            {
                if (map.Doors[1] && x == 384) return true;
                return false;
            }
            if(x > 704)
            {
                if (map.Doors[2] && y == 256) return true;
                return false;
            }
            if(y > 448)
            {
                if (map.Doors[3] && x == 384) return true;
                return false;
            }

            return true;
        }

        public void ChangeRoom(string dir)
        {
           if(dir == "left")
            {
                if (map.RoomNumber == 1) map.RoomNumber = 0;
                if (map.RoomNumber == 2) map.RoomNumber = 1;
                if (map.RoomNumber == 4) map.RoomNumber = 3;
                if (map.RoomNumber == 5) map.RoomNumber = 4;
                if (map.RoomNumber == 7) map.RoomNumber = 6;
                if (map.RoomNumber == 8) map.RoomNumber = 7;
            }

            if (dir == "right")
            {
                if (map.RoomNumber == 0) map.RoomNumber = 1;
                if (map.RoomNumber == 1) map.RoomNumber = 2;
                if (map.RoomNumber == 3) map.RoomNumber = 4;
                if (map.RoomNumber == 4) map.RoomNumber = 5;
                if (map.RoomNumber == 6) map.RoomNumber = 7;
                if (map.RoomNumber == 7) map.RoomNumber = 8;
            }

            if (dir == "up")
            {
                if (map.RoomNumber == 3) map.RoomNumber = 0;
                if (map.RoomNumber == 4) map.RoomNumber = 1;
                if (map.RoomNumber == 5) map.RoomNumber = 2;
                if (map.RoomNumber == 6) map.RoomNumber = 3;
                if (map.RoomNumber == 7) map.RoomNumber = 4;
                if (map.RoomNumber == 8) map.RoomNumber = 5;
            }

            if (dir == "down")
            {
                if (map.RoomNumber == 0) map.RoomNumber = 3;
                if (map.RoomNumber == 1) map.RoomNumber = 4;
                if (map.RoomNumber == 2) map.RoomNumber = 5;
                if (map.RoomNumber == 3) map.RoomNumber = 6;
                if (map.RoomNumber == 4) map.RoomNumber = 7;
                if (map.RoomNumber == 5) map.RoomNumber = 8;
            }

            if (map.RoomNumber == 0)
            {
                map.Doors[0] = false;
                map.Doors[1] = false;
                map.Doors[2] = true;
                map.Doors[3] = true;
            }

            else if (map.RoomNumber == 1)
            {
                map.Doors[0] = true;
                map.Doors[1] = false;
                map.Doors[2] = true;
                map.Doors[3] = true;
            }

            else if (map.RoomNumber == 2)
            {
                map.Doors[0] = true;
                map.Doors[1] = false;
                map.Doors[2] = false;
                map.Doors[3] = true;
            }

            else if (map.RoomNumber == 3)
            {
                map.Doors[0] = false;
                map.Doors[1] = true;
                map.Doors[2] = true;
                map.Doors[3] = true;
            }

            else if (map.RoomNumber == 4)
            {
                map.Doors[0] = true;
                map.Doors[1] = true;
                map.Doors[2] = true;
                map.Doors[3] = true;
            }

            else if (map.RoomNumber == 5)
            {
                map.Doors[0] = true;
                map.Doors[1] = true;
                map.Doors[2] = false;
                map.Doors[3] = true;
            }

            else if (map.RoomNumber == 6)
            {
                map.Doors[0] = false;
                map.Doors[1] = true;
                map.Doors[2] = true;
                map.Doors[3] = false;
            }

            else if (map.RoomNumber == 7)
            {
                map.Doors[0] = true;
                map.Doors[1] = true;
                map.Doors[2] = true;
                map.Doors[3] = false;
            }

            else
            {
                map.Doors[0] = true;
                map.Doors[1] = true;
                map.Doors[2] = false;
                map.Doors[3] = false;
            }
        }
    }
}
