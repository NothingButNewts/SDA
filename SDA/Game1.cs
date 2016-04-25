using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SDA
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        enum GameState { Menu, LevelSelect, Game, GameOver }
        enum MenuState { Menu1, Menu2, Instruct1, Instruct2, Controls1, Controls2, GameOver1, GameOver2 }
        MenuState menu;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool playerTurn;
        Player playerCharacter;  
        Map gameMap;
        GameState currentGameState;
        Ghoul test;
        Texture2D background1; //differnet menu images
        Texture2D background2;
        Texture2D background3;
        Texture2D background4;
        Texture2D background5;
        Texture2D background6;
        Texture2D background7;
        Texture2D background8;

        //Temporary bool to test the enemy spawning, should change when map transitioning is in
        bool mapLoaded;

        public bool PlayerTurn { get { return playerTurn; }
            set { playerTurn = value;} }
     

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            playerTurn = true;
            Window.IsBorderless = true;
            graphics.PreferredBackBufferHeight = (64 * 9);
            graphics.PreferredBackBufferWidth = (64 * 13);
            mapLoaded = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            
            gameMap = new Map(this.Content);
            playerCharacter = new Player(new Vector2(128, 128), "Character/Player",gameMap);
            currentGameState = GameState.Menu;
            menu = MenuState.Menu1;
            currentGameState = GameState.Menu;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerCharacter.LoadContent(this.Content);
      
            gameMap.LoadLevels();

            // TODO: use this.Content to load your game content here
            background1 = Content.Load<Texture2D>("Menu1");
            background2 = Content.Load<Texture2D>("Menu2");
            background3 = Content.Load<Texture2D>("Instruct1");
            background4 = Content.Load<Texture2D>("Instruct2");
            background5 = Content.Load<Texture2D>("Controls1");
            background6 = Content.Load<Texture2D>("Controls2");
            background7 = Content.Load<Texture2D>("GameOver1");
            background8 = Content.Load<Texture2D>("GameOver2");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //allows the menus to change between each other, allowing for
        //proper transitions. Uses the MenuState for the changes
        //called in the update method
        public void ChangeMenu()
        {
            KeyboardState state = Keyboard.GetState();
            switch (menu)
            {
                case MenuState.Menu1:
                    if (state.IsKeyDown(Keys.Down))
                    {
                        menu = MenuState.Menu2;
                        break;
                    }
                    else if (state.IsKeyDown(Keys.Enter))
                    {
                        //start the game
                        System.Threading.Thread.Sleep(500);
                        currentGameState = GameState.Game;
                        menu = MenuState.Menu1;
                        gameMap.LoadFloor();
                        break;
                    }
                    else { break; }
                case MenuState.Menu2:
                    if (state.IsKeyDown(Keys.Up))
                    {
                        menu = MenuState.Menu1;
                        break;
                    }
                    else if (state.IsKeyDown(Keys.Enter))
                    {
                        menu = MenuState.Instruct1;
                        System.Threading.Thread.Sleep(500);
                        break;
                    }
                    else { break; }
                case MenuState.Instruct1:
                    if (state.IsKeyDown(Keys.Right))
                    {
                        menu = MenuState.Instruct2;
                        break;
                    }
                    else if (state.IsKeyDown(Keys.Enter))
                    {
                        //start the game
                        menu = MenuState.Menu1;
                        System.Threading.Thread.Sleep(500);
                        break;
                    }
                    else { break; }
                case MenuState.Instruct2:
                    if (state.IsKeyDown(Keys.Left))
                    {
                        menu = MenuState.Instruct1;
                        break;
                    }
                    else if (state.IsKeyDown(Keys.Enter))
                    {
                        menu = MenuState.Controls1;
                        System.Threading.Thread.Sleep(500);
                        break;
                    }
                    else { break; }
                case MenuState.Controls1:
                    if (state.IsKeyDown(Keys.Right))
                    {
                        menu = MenuState.Controls2;
                        break;
                    }
                    else if (state.IsKeyDown(Keys.Enter))
                    {
                        menu = MenuState.Menu1;
                        System.Threading.Thread.Sleep(500);
                        break;
                    }
                    else { break; }
                case MenuState.Controls2:
                    if (state.IsKeyDown(Keys.Left))
                    {
                        menu = MenuState.Controls1;
                        break;
                    }
                    else if (state.IsKeyDown(Keys.Enter))
                    {
                        System.Threading.Thread.Sleep(500);
                        currentGameState = GameState.Game;
                        menu = MenuState.Menu1;
                        gameMap.LoadFloor();
                        break;
                    }
                    else { break; }
                case MenuState.GameOver1:
                    if (state.IsKeyDown(Keys.Right))
                    {
                        menu = MenuState.GameOver2;
                        break;
                    }
                    else if (state.IsKeyDown(Keys.Enter))
                    {
                        System.Threading.Thread.Sleep(500);
                        menu = MenuState.Menu1;
                        break;
                    }
                    else { break; }
                case MenuState.GameOver2:
                    if (state.IsKeyDown(Keys.Left))
                    {
                        menu = MenuState.GameOver1;
                        break;
                    }
                    else if (state.IsKeyDown(Keys.Enter))
                    {
                        System.Threading.Thread.Sleep(500);
                        Exit();
                        break;
                    }
                    else { break; }
            }
        }

        //The method to draw whichever menu is currently active.
        public void DrawMenu()
        {
            if (menu == MenuState.Menu1)
            {
                spriteBatch.Draw(background1, new Vector2(0.0f, 0.0f), Color.White);
            }
            else if (menu == MenuState.Menu2)
            {
                spriteBatch.Draw(background2, new Vector2(0.0f, 0.0f), Color.White);
            }
            else if (menu == MenuState.Instruct1)
            {
                spriteBatch.Draw(background3, new Vector2(0.0f, 0.0f), Color.White);
            }
            else if (menu == MenuState.Instruct2)
            {
                spriteBatch.Draw(background4, new Vector2(0.0f, 0.0f), Color.White);
            }
            else if (menu == MenuState.Controls1)
            {
                spriteBatch.Draw(background5, new Vector2(0.0f, 0.0f), Color.White);
            }
            else if (menu == MenuState.Controls2)
            {
                spriteBatch.Draw(background6, new Vector2(0.0f, 0.0f), Color.White);
            }
            else if (menu == MenuState.GameOver1)
            {
                spriteBatch.Draw(background7, new Vector2(0.0f, 0.0f), Color.White);
            }
            else if (menu == MenuState.GameOver2)
            {
                spriteBatch.Draw(background8, new Vector2(0.0f, 0.0f), Color.White);
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) 
                Exit();

            // TODO: Add your update logic here
            if (currentGameState == GameState.Menu)
            {
                ChangeMenu();
            }
            else if (currentGameState == GameState.Game)
            {
                if (mapLoaded == false)
                {
                    gameMap.LoadRoom(this.Content);
                    mapLoaded = true;
                }
                if (playerTurn == true)
                {
                    playerCharacter.Move(gameMap.ObjectSpaces,gameMap.Enemies,gameMap);
                    playerTurn = playerCharacter.playerTurn;

                }
                else if (playerTurn == false)
                {
                    foreach (Enemy enemy in gameMap.Enemies)
                    {
                        if (enemy.DetectPlayer(playerCharacter))
                        {

                            enemy.Attack(playerCharacter);

                        }
                        else { 
                        enemy.Move(gameMap.ObjectSpaces,gameMap.Sprites,0,playerCharacter);
                        }
                    }
                    playerTurn = true;

                }
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            if (currentGameState == GameState.Menu)
            {
                DrawMenu();
            }

            if (currentGameState == GameState.Game)
            {
                gameMap.Draw(spriteBatch);
                playerCharacter.Draw(spriteBatch);
                foreach (Enemy enemy in gameMap.Enemies)
                {
                    if (enemy.IsAlive == true)
                    {


                        enemy.Draw(spriteBatch);
                    }
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
            
           
        
    }
}
