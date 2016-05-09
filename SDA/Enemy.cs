using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SDA
{
    abstract class  Enemy : Sprite
    {
        //Base class for all the enemies, builds off of Sprite
        int health;
        int defense;
        int expValue;
        bool isAlive;
        Map map = new Map();
        public Enemy(Vector2 startPos, string asset):base(startPos, asset)
        {
            isAlive = true;
        }

        abstract public void Move(List<Rectangle> walls,List<Sprite> entities, int attempt,Player player);
        abstract public void Attack(Player player);
        abstract public bool DetectPlayer(Player player);

        public int Health { get { return health; } set { health = value; } }
        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
        public int ExpValue { get { return expValue; } set { expValue = value; } }

        //deals with enemy collision with outer walls to make sure they can't leave the room
        public bool CheckOuterWalls(int x, int y)
        {
            if (x < 64)
            {
                return false;
            }
            if (y < 64)
            {
                return false;
            }
            if (x > 704)
            {
                return false;
            }
            if (y > 448)
            {
                return false;
            }

            return true;
        }
    }
}

