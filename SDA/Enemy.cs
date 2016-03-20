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
        //Base class for all the enemies, builds off of Sprite and then 
        int health;
        int defense;
        int expValue;

        abstract protected void Move();
        abstract protected void Attack();


    }
}

