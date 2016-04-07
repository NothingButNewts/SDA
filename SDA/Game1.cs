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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool playerTurn;
        Player playerCharacter;
        Ghoul testGhoul;
        Map gameMap;

        public bool PlayerTurn { get { return playerTurn; }
            set { playerTurn = value;} }

        enum GameState { StartMenu, LevelSelect, Game, GameOver}
        GameState currentGameState;     

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            playerTurn = true;
            Window.IsBorderless = true;
            graphics.PreferredBackBufferHeight = (64 * 7);
            graphics.PreferredBackBufferWidth = (64 * 11);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            playerCharacter = new Player();
            testGhoul = new Ghoul();
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

            //I call the player and Ghoul LoadContents that are within the Sprite class
            playerCharacter.LoadContent(this.Content, "Character/Player", new Vector2(64,64));
            testGhoul.LoadContent(this.Content, "Character/Ghoul", new Vector2(512,256));
            gameMap.LoadLevels();
            // TODO: use this.Content to load your game content here
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
                }
            }
            if (currentGameState == GameState.Game)
            {
                if (playerTurn == true)
                {
                    //There is no collision detection implemented at the current moment. 
                    playerCharacter.Move(gameMap.WallSpaces);
                    playerTurn = playerCharacter.playerTurn;

                }
                else if (playerTurn == false)
                {
                    //There is no sort of AI enemy functionality implemented yet, so I am using a sleep to simulate the turn based functionality.
                    //After the AI is implemented, I may still keep a sleep so that the player doesn't move followed by an immediate enemy movement

                    System.Threading.Thread.Sleep(20);
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
            
            if (currentGameState == GameState.Game)
            {
                gameMap.Draw(spriteBatch);
                testGhoul.Draw(spriteBatch); //The enemy currently gets drawn and does literally nothing.
                playerCharacter.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
            
           
        
    }
}
