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
    class Player
    {
        int health;
        int dexterity;
        int strength;
        int defense;
        int exp;
        int expToLevel;
        int level;
        int vitality;

        Texture2D texture;

        public Texture2D Texture { get { return texture; } }
        

        public Player(ContentManager content)
        {
            
            health = 100;
            dexterity = 1;
            strength = 1;
            exp = 0;
            expToLevel = 100;
            level = 1;
            content.RootDirectory = "Content";
            texture = content.Load<Texture2D>("PlayerTexture");
           
        }
        /// <summary>
        /// Method to move the character, uses input to move character one tile
        /// Up, left, right and down
        /// </summary>
        public void Move()
        {
            
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
