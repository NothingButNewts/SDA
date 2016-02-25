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
        Texture2D tempGrid;
        bool playerTurn;
        Player playerCharacter;
        Skeleton testSkeleton;
        
        public bool PlayerTurn { get { return playerTurn; }
            set { playerTurn = value;} }
        
        
        //Probably Temporary, just using for variables for the playerRect, bmight need tro change
        int X;
        int Y;
        
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            playerTurn = true;
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playerCharacter = new Player();
            testSkeleton = new Skeleton();
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
            tempGrid = Content.Load<Texture2D>("grid");
            playerCharacter.LoadContent(this.Content, "PlayerTexture", new Vector2(10,10));
            testSkeleton.LoadContent(this.Content, "EnemyTexture", new Vector2(210,150));

            
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
            if (playerTurn == true)
            {

                playerCharacter.Move();
                playerTurn = playerCharacter.playerTurn;
            }
            else if (playerTurn == false)
            {
                System.Threading.Thread.Sleep(20);
                playerTurn = true;
            }
            
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(tempGrid, new Rectangle(0, 0, Window.ClientBounds.Width, 
                Window.ClientBounds.Height), Color.White);
            testSkeleton.Draw(spriteBatch);
            playerCharacter.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
            
           
        
    }
}
