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
        int maxHealth;
        int dexterity;
        int strength;
        int defense;
        int exp;
        int expToLevel;
        int level;
        int vitality;
        KeyboardState oldKBState;
        public bool playerTurn;
        

        public Player()
        {

            
            dexterity = 1;
            strength = 1;
            exp = 0;
            vitality = 1;
            maxHealth = 90 + (10 * vitality);
            expToLevel = 100;
            level = 1;
            oldKBState = Keyboard.GetState();
            playerTurn = true;
                
        }
        /// <summary>
        /// Method to move the character, uses input to move character one tile
        /// Up, left, right and down
        /// </summary>
        public void Move()
        {
            KeyboardState currentKBState = Keyboard.GetState();

            if (oldKBState.IsKeyUp(Keys.W) && currentKBState.IsKeyDown(Keys.W))
            {
                position.Y -= 60;
                playerTurn = false;
            }
            else if (oldKBState.IsKeyUp(Keys.A) && currentKBState.IsKeyDown(Keys.A))
            {
                position.X -= 100;
                playerTurn = false;
            }
            else if (oldKBState.IsKeyUp(Keys.D) && currentKBState.IsKeyDown(Keys.D))
            {
                position.X += 100;
                playerTurn = false;
            }
            else if (oldKBState.IsKeyUp(Keys.S) && currentKBState.IsKeyDown(Keys.S))
            {
                position.Y += 60;
                playerTurn = false;
            }
            oldKBState = currentKBState;
            
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
    }
}
