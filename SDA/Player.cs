using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        
        //Handling player texture in Game1.cs for now
        //Texture2D texture;
        

        public Player()
        {
            
            health = 100;
            dexterity = 1;
            strength = 1;
            exp = 0;
            expToLevel = 100;
            level = 1;
           
        }

        public void Move()
        {

        }
    }
}
