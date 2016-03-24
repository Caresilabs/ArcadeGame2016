using CloudColony.Framework;
using CloudColony.Scenes;
using Microsoft.Xna.Framework;

namespace CloudColony
{


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #if (!ARCADE)
            GraphicsDeviceManager graphics;
            SpriteBatch spriteBatch;
        #else
            public override string GameDisplayName { get { return "CloudColony"; } }
        #endif

        public Screen CurrentScreen { get; private set; }

        public Game1()
        {
#if (!ARCADE)
                graphics = new GraphicsDeviceManager(this);
#endif

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
#if (!ARCADE)
                    // Create a new SpriteBatch, which can be used to draw textures.
                    spriteBatch = new SpriteBatch(GraphicsDevice);
#endif

            SetScreen(new GameScreen());
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
            #if (!ARCADE)
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            #endif
            // get second between last frame and current frame, used for fair physics manipulation and not based on frames
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // then update the screen
            CurrentScreen.Update(delta);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw screen
            CurrentScreen.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        public void SetScreen(Screen newScreen)
        {
            if (newScreen == null) return;

            // Dispose old screen
            if (CurrentScreen != null)
                CurrentScreen.Dispose();

            // init new screen
            CurrentScreen = newScreen;
            newScreen.Game = this;
            newScreen.Graphics = GraphicsDevice;
            CurrentScreen.Init();
        }
    }
}
