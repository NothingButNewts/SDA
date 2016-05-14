using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SDA
{
    class Ghoul : Enemy
    {
        bool canMove;
        Random move;
        int moveDirection; // 0 moves up, 1 moves left, 2 moves down, 3 moves right
        int damage;

        public Ghoul(Vector2 startPos, string asset, int floor):base(startPos, asset )
        {
            canMove = true;
            move = new Random();
            base.Health = (int)(50 * (Math.Pow(1.25,floor)));
            base.ExpValue = (int)(base.Health / 7.5);
            base.Name = "Ghoul";
        }


        public override void Attack(Player player)
        {
            bool canAttack = DetectPlayer(player);

            if(canAttack == true)
            {
                player.Health = player.Health - 5;
            }
            else if (canAttack == false)
            {
                return;
            }
        }



        public override void Move(List<Rectangle> walls,List<Sprite>entities, int attempt,Player player)
        {
            Rectangle tempSize = size;

            //if ((Math.Abs(this.size.X - player.size.X) > 75) && (Math.Abs(this.size.Y - player.size.Y) > 75))//if the player is not close then move randomly
           // {
                moveDirection = move.Next(0, 4);
         //   }
         /*   else
            {
                if (this.size.X != player.size.X)//Ghoul moves toward the player
                {
                    if (this.size.X > player.size.X)
                    {
                        moveDirection = 1;
                    }
                    else
                    {
                        moveDirection = 3;
                    }
                }
                else
                {
                    if (this.size.Y > player.size.Y)
                    {
                        moveDirection = 0;
                    }
                    else
                    {
                        moveDirection = 2;
                    }
                }
            }*/

            switch (moveDirection)
            {
                case 0: tempSize = new Rectangle(size.X, size.Y - 64, size.Width, size.Height);
                    break;
                case 1: tempSize = new Rectangle(size.X - 64, size.Y, size.Width, size.Height);
                    break;
                case 2: tempSize = new Rectangle(size.X, size.Y + 64, size.Width, size.Height);
                    break;
                case 3: tempSize = new Rectangle(size.X + 64, size.Y, size.Width, size.Height);
                    break;
                default: break;
            }
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
            foreach(Sprite entity in entities)
            {
                if (canMove== true)
                {
                    if (entity.size.Intersects(tempSize))
                    {
                        canMove = false;
                    }
                }
            }
            if (player.size.Intersects(tempSize))
            {
                if (canMove == true)
                {
                    canMove = false;
                }
            }
            if (CheckOuterWalls(tempSize.X, tempSize.Y)== false)
            {
                canMove = false;
            }
            if (canMove == true)
            {
                size = tempSize;
            }
            else
            {
                if (attempt >= 1)
                {
                    return;
                }
                else
                {
                    attempt++;
                    canMove = true;
                    Move(walls, entities, attempt,player);
                }
            }
        }
        public override bool DetectPlayer(Player player)
        {
        
            Rectangle tempSize=size;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        tempSize = new Rectangle(size.X + 64, size.Y, size.Width, size.Height);
                        break;
                    case 1:
                        tempSize = new Rectangle(size.X - 64, size.Y, size.Width, size.Height);
                        break;
                    case 2:
                        tempSize = new Rectangle(size.X, size.Y + 64, size.Width, size.Height);
                        break;
                    case 3:
                        tempSize = new Rectangle(size.X, size.Y - 64, size.Width, size.Height);
                        break;
                    default:
                        break;
                }
                if (tempSize.Intersects(player.size))
                {
                    return true;
                }
                
            }
            return false;
        }
        
    }
}
