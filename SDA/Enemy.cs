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
        public Enemy(Vector2 startPos, string asset):base(startPos, asset)
        {

        }

        abstract public void Move(List<Rectangle> walls,List<Sprite> entities, int attempt,Player player);
        abstract protected void Attack();

        protected virtual void Spawn()
        {
            
        }

        abstract protected void DetectPlayer();


    }
}

