using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CloudColony.Scenes
{
    public class MainMenuScreen : Screen
    {
        public bool PlayerRedReady { get; private set; }
        public bool PlayerBlueReady { get; private set; }

        public float TotalTime { get; private set; }

        public float PlayDelayTime { get; private set; }

        public Sprite Background { get; private set; }

        private Sprite logo;

        public override void Init()
        {
            this.PlayerRedReady = false;
            this.PlayerBlueReady = false;

            this.logo = new Sprite(CC.Logo, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.24f, 358, 64);
            logo.SetScale(2f);

            this.Background = new Sprite(CC.MenuBackground, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT / 2f, CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT);
            Background.ZIndex = 1f;
        }

        public override void Update(float delta)
        {
            TotalTime += delta;


            if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Side) == InputState.Released ||
                       InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Side) == InputState.Released)
            {
                SetScreen(new CreditsScreen());
                return;
            }

            // Hack so input wont happen after every game
            if (TotalTime > 0.2f)
            {
                if (CC.AnyKeyPressed(PlayerIndex.One))
                    PlayerRedReady = true;

                if (CC.AnyKeyPressed(PlayerIndex.Two))
                    PlayerBlueReady = true;
            }

            if (PlayerRedReady && PlayerBlueReady)
            {
                PlayDelayTime += delta;

                if (PlayDelayTime >= 1f)
                    SetScreen(new GameScreen());
            }

        }

        public override void Draw(SpriteBatch batch)
        {
            Graphics.Clear(Color.Black);

            batch.Begin(SpriteSortMode.BackToFront,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    null);

            Background.Draw(batch);

            // Draw player red side
            var redBackground = new Sprite(CC.Button2, CC.VIEWPORT_WIDTH * 0.25f, CC.VIEWPORT_HEIGHT * 0.55f, 700, 170);
            redBackground.ZIndex = 0.9f;
            redBackground.Draw(batch);
            if (PlayerRedReady)
            {
                string txt = "Player 1 READY";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.25f, CC.VIEWPORT_HEIGHT * 0.55f),
                    Color.Red, (float)Math.Sin(TotalTime * 10) / 10f, CC.Font.MeasureString(txt) / 2f, 1.6f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 1 - Press any key";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.25f, CC.VIEWPORT_HEIGHT * 0.55f),
                    Color.Red, 0, CC.Font.MeasureString(txt) / 2f, 1.3f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
            }

            // Draw player blue side
            var blueBackground = new Sprite(CC.Button2, CC.VIEWPORT_WIDTH * 0.75f, CC.VIEWPORT_HEIGHT * 0.55f, 700, 170);
            blueBackground.ZIndex = 0.9f;
            blueBackground.Draw(batch);
            if (PlayerBlueReady)
            {
                string txt = "Player 2 READY";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.75f, CC.VIEWPORT_HEIGHT * 0.55f),
                    Color.Blue, (float)Math.Sin(TotalTime * 10) / 10f, CC.Font.MeasureString(txt) / 2f, 1.3f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 2 - Press any key";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.75f, CC.VIEWPORT_HEIGHT * 0.55f),
                    Color.Blue, 0, CC.Font.MeasureString(txt) / 2f, 1.3f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
            }

            logo.SetPosition(logo.Position.X, logo.Position.Y + (float)Math.Sin(TotalTime*2) / 2.5f);
            logo.Draw(batch);

            // Draw insert coin
            {
                var coinBackground = new Sprite(CC.Button1, CC.VIEWPORT_WIDTH * 0.5f, CC.VIEWPORT_HEIGHT * 0.85f, 500, 140);
                coinBackground.ZIndex = 0.9f;
                coinBackground.Draw(batch);

                string insert = "Insert coin";
                batch.DrawString(CC.Font, insert, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.86f),
                    Color.WhiteSmoke * (float)((Math.Sin(TotalTime * 5) + 1) / 2f), 0, CC.Font.MeasureString(insert) / 2f, 1.9f, SpriteEffects.None, 0);
            }

            // Copy right.
            string copy = "(c) " +  DateTime.Now.Year + " | Side button for credits";
            batch.DrawString(CC.Font, copy, new Vector2(42, 30f),
                Color.WhiteSmoke * (float)((Math.Sin(TotalTime * 2) + 1) / 2f), 0, Vector2.Zero, 1.15f, SpriteEffects.None, 0);


            Game.DrawFrame();

            batch.End();
        }

        public override void Dispose()
        {
        }
    }
}
