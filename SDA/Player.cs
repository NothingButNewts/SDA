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
        int health; //Current health that the player has
        int maxHealth; //Maximum health the player has
        int strength; //Will effect damage the player does
        int defense; //Reduction to the damage that the player takes
        int exp; //Player's current exp, changes with each monster kill 
        int expToLevel; //Player's exp that is needed to advance to the next level
        int level; //Players current level
        int damage; //Damage that the player does, based on weapon and strength
        int healthPot; //Tracks how many health potions the player has.
        Enemy lasthit; //Remember the last enemy hit by player
        int score;

        KeyboardState oldKBState; //Becomes equal to the previous currentKBState
        KeyboardState currentKBState; //Checks the current keyboard state

        public bool playerTurn; //Bool to determine if it is the player's turn or not, changes on enemy turns and player turns
        bool canMove;

        DirectionFacing currentDirection;
        enum DirectionFacing { Up, Down, Left, Right };

        Map map;
        

        public KeyboardState OldKBState { get { return oldKBState; } }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Map Room
        {
            get { return map; }
            set { map = value; }
        }

        public Enemy Lasthit
        {
            get { return lasthit; }
            set { lasthit = value; }
        }

        public int Pots
        {
            get { return healthPot; }
            set { healthPot = value; }
        }

        public int Lvl
        {
            get { return level; }
            set { level = value; }
        }

        public int Exp
        {
            get { return exp; }
            set { exp = value; }
        }

        public int ToLvl
        {
            get { return expToLevel-exp; }
            set { expToLevel = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public Player(Vector2 startPos, string asset,Map map):base(startPos, asset)
        {
            currentDirection = DirectionFacing.Up;
            strength = 1;
            exp = 0;
            maxHealth = 100;
            health = maxHealth;
            expToLevel = 40;
            level = 1;
            playerTurn = true;
            canMove = true;
            this.map = map;
            damage = 10;
            healthPot = 3;
            score = 0;
        }
        /// <summary>
        /// Method to move the character, uses input to move character one tile
        /// Checks if the key was pressed during the currentKBState vs. the oldKBState
        /// Up, left, right and down
        /// </summary>
        public void Move(List<Rectangle> walls,List<Enemy> enemies, Map map, ContentManager content)
        {
            playerTurn = true;
            Rectangle tempSize= size;
            currentKBState = Keyboard.GetState();
            if (oldKBState.IsKeyUp(Keys.Space) && currentKBState.IsKeyDown(Keys.Space)){
                Attack(enemies);
            }
            else if (oldKBState.IsKeyUp(Keys.H)&& currentKBState.IsKeyDown(Keys.H))
            {
                Heal();
            }
            else {
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

                //map transitioning
                if (tempSize.X > 770)
                {
                    ChangeRoom(content);
                    tempSize.X = 64;
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



                foreach (Enemy enemy in enemies)
                {
                    if (canMove == true)
                    {
                        if (enemy.size.Intersects(tempSize))
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
            oldKBState = currentKBState;
        }
        public void Heal()
        {
            if (healthPot > 0 && health != maxHealth)
            {
                int healAmt = (int)maxHealth / 3;
                health += healAmt;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                healthPot--;
            }
        }
        /// <summary>
        /// Called when the player's Exp is greater than or equal to expToLevel
        /// Increments Level by 1, scales expToLevel up, and reduces exp by expToLevel to reset
        /// </summary>
        public void Level()
        {
            level++;
            exp = exp - expToLevel;
            expToLevel = (int)(expToLevel * 1.25);
            if (exp < 0)
            {
                exp = 0;
            }
            strength++;
            damage = (10 * (int)(Math.Pow(1.15, strength)));
        }

        public void GainPotion()
        {
            healthPot++;    
        }

        public void Attack(List<Enemy> enemies)
        {
            Rectangle tempSize = size;
            foreach (Enemy enemy in enemies)
            {
                switch (currentDirection)
                {
                    case DirectionFacing.Up:
                        tempSize = new Rectangle(size.X, size.Y - 64, size.Width, size.Height);
                        if (tempSize.Intersects(enemy.size))
                        {
                            enemy.Health = enemy.Health - damage;
                            lasthit = enemy;
                            playerTurn = false;
                        }
                        break;

                    case DirectionFacing.Left:
                        tempSize = new Rectangle(size.X - 64, size.Y, size.Width, size.Height);
                        if (tempSize.Intersects(enemy.size))
                        {
                            enemy.Health = enemy.Health - damage;
                            lasthit = enemy;
                            playerTurn = false;
                        }
                        break;

                    case DirectionFacing.Right:
                        tempSize = new Rectangle(size.X + 64, size.Y, size.Width, size.Height);
                        if (tempSize.Intersects(enemy.size))
                        {
                            enemy.Health = enemy.Health - damage;
                            lasthit = enemy;
                            playerTurn = false;
                        }
                        break;

                    case DirectionFacing.Down:

                        tempSize = new Rectangle(size.X, size.Y + 64, size.Width, size.Height);
                        if (tempSize.Intersects(enemy.size))
                        {
                            enemy.Health = enemy.Health - damage;
                            lasthit = enemy;

                        }
                        break;

                    default:
                        break;
                }
                if (enemy.Health <= 0)
                {
                    enemy.IsAlive = false;
                    exp += enemy.ExpValue;
                    score += (enemy.ExpValue * 2);
                    if (exp >= expToLevel)
                    {
                        Level();
                    }
                }
            }
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

        public void ChangeRoom(ContentManager content)
        {
            if (map.RoomNumber == 98)
            {
                map.Doors[2] = false;
                return;
            }
            map.RoomNumber++;
            map.LoadRoom(content);
        }

        public void Reset(ContentManager content)
        {
            size = new Rectangle(128, 128, size.Width, size.Height);
            map.RoomNumber = 0;
            map.LoadRoom(content);
            health = 100;
            healthPot = 3;
            level = 1;
            score = 0;
            lasthit = null;
        }
    }
}
