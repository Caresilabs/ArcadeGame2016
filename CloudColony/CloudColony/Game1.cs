using CloudColony.Framework;
using CloudColony.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private Screen nextScreen;

        private Sprite frame;

        private Sprite transitionSprite;
        private FrameAnimation transitionAnimation;

        public Game1()
        {
#if (!ARCADE)
                graphics = new GraphicsDeviceManager(this);
#endif

        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            base.Initialize();
        }

        protected override void LoadContent()
        {
#if (!ARCADE)
                    // Create a new SpriteBatch, which can be used to draw textures.
                    spriteBatch = new SpriteBatch(GraphicsDevice);
#endif
            // Load Content
            CC.Load(Content);

            frame = new Sprite(CC.Frame, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT / 2f, CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT);

            // Transition
            transitionSprite = new Sprite(null, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT / 2f, CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT);

            nextScreen = new MainMenuScreen();
            SetNextScreen();
            //SetScreen(new MainMenuScreen());
        }

        protected override void UnloadContent()
        {
        }

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

            // Transition
            if (transitionAnimation != null)
            {
                if (transitionAnimation.GetPercent() >= 1)
                {
                    if (nextScreen != null)
                    {
                        transitionAnimation = new FrameAnimation(CC.TransitionTexture, 0, 0, 160, 90, 18, 0.05f, new Point(1, 0), false, true);
                        transitionSprite.AddAnimation("anim", transitionAnimation).SetAnimation("anim");

                        SetNextScreen();
                    }
                    else
                    {
                        transitionAnimation = null;
                    }
                }
                transitionSprite.Update(delta);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Draw screen
            CurrentScreen.Draw(spriteBatch);

            // Draw transition
            if (transitionAnimation != null)
            {
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                transitionSprite.Draw(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public void DrawFrame()
        {
            frame.Draw(spriteBatch);
        }

        private void SetNextScreen()
        {
            // Dispose old screen
            if (CurrentScreen != null)
                CurrentScreen.Dispose();

            // init new screen
            CurrentScreen = nextScreen;
            nextScreen.Game = this;
            nextScreen.Graphics = GraphicsDevice;
            CurrentScreen.Init();

            nextScreen = null;
        }

        public void SetScreen(Screen newScreen)
        {
            if (newScreen == null || transitionAnimation != null) return;

            this.nextScreen = newScreen;

            transitionAnimation = new FrameAnimation(CC.TransitionTexture, 0, 0, 160, 90, 18, 0.05f, new Point(1, 0), false, false);
            transitionSprite.AddAnimation("anim", transitionAnimation).SetAnimation("anim");
        }
    }
}
