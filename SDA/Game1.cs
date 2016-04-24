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
        enum GameState { StartMenu, Game, GameOver } 

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool playerTurn;
        Player playerCharacter;  
        Map gameMap;
        GameState currentGameState;
        Ghoul test;

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
            graphics.PreferredBackBufferHeight = (64 * 7);
            graphics.PreferredBackBufferWidth = (64 * 11);
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

            playerCharacter = new Player(new Vector2(64, 64), "Character/Player");
            gameMap = new Map(this.Content);
            currentGameState = GameState.StartMenu;
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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
            if (currentGameState == GameState.StartMenu)
            {
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentGameState = GameState.Game;
                        gameMap.LoadFloor();
                    }
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
                     
                    playerCharacter.Move(gameMap.ObjectSpaces,gameMap.Enemies);
                    playerTurn = playerCharacter.playerTurn;

                }
                else if (playerTurn == false)
                {
                    //There is no sort of AI enemy functionality implemented yet, so I am using a sleep to simulate the turn based functionality.
                    //After the AI is implemented, I may still keep a sleep so that the player doesn't move followed by an immediate enemy movement
                    //       test.Move(gameMap.ObjectSpaces);
                    foreach (Enemy enemy in gameMap.Enemies)
                    {
                        enemy.Move(gameMap.ObjectSpaces,gameMap.Sprites,0,playerCharacter);
                    }
                    playerTurn = true;

                }
            }
            else if (currentGameState == GameState.GameOver)
            {

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
            
            if (currentGameState == GameState.Game)
            {
                gameMap.Draw(spriteBatch);
                playerCharacter.Draw(spriteBatch);
                foreach (Enemy enemy in gameMap.Enemies)
                {
                    enemy.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
            
           
        
    }
}