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
        int maxHealth; //Maximum health the player has
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

        public KeyboardState OldKBState { get { return oldKBState; } }
        public Player()
        {
            currentDirection = DirectionFacing.Up;
            dexterity = 1;
            strength = 1;
            exp = 0;
            vitality = 1;
            maxHealth = 90 + (10 * vitality);
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
        public void Move(List<Rectangle> walls)
        {
            Rectangle tempSize= size;
            //No collision detection is implemented currently, so the player is completely able to clip through walls and go off the screen.
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
            if (canMove == true)
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
    }
}
