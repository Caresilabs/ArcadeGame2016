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

        private Sprite logo;

        public override void Init()
        {
            this.PlayerRedReady = false;
            this.PlayerBlueReady = false;

            this.logo = new Sprite(CC.Logo, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.22f, 358, 64);
            logo.SetScale(2f);
        }

        public override void Update(float delta)
        {
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

            if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released ||
                       InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
            {
                SetScreen(new CreditsScreen());
            }

            TotalTime += delta;

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

            if (PlayerRedReady)
            {
                string txt = "Player 1 READY";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.3f, CC.VIEWPORT_HEIGHT * 0.5f),
                    Color.Red, (float)Math.Sin(TotalTime * 10) / 10f, CC.Font.MeasureString(txt) / 2f, 1.6f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 1 - Press any key";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.3f, CC.VIEWPORT_HEIGHT * 0.5f),
                    Color.Red, 0, CC.Font.MeasureString(txt) / 2f, 1.6f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
            }

            if (PlayerBlueReady)
            {
                string txt = "Player 2 READY";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.7f, CC.VIEWPORT_HEIGHT * 0.5f),
                    Color.Blue, (float)Math.Sin(TotalTime * 10) / 10f, CC.Font.MeasureString(txt) / 2f, 1.6f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 2 - Press any key";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.7f, CC.VIEWPORT_HEIGHT * 0.5f),
                    Color.Blue, 0, CC.Font.MeasureString(txt) / 2f, 1.6f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
            }

            logo.SetPosition(logo.Position.X, logo.Position.Y + (float)Math.Sin(TotalTime*2) / 2.5f);
            logo.Draw(batch);


            string insert = "Insert coin";
            batch.DrawString(CC.Font, insert, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.87f),
                Color.White * (float)((Math.Sin(TotalTime * 5) + 1) / 2f), 0, CC.Font.MeasureString(insert) / 2f, 2, SpriteEffects.None, 0);


            //if (PlayDelayTime > 0)
            //{
            //    batch.Draw(CC.Pixel, new Rectangle(0, 0, CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT), CC.Pixel, Color.Black * ((PlayDelayTime / 2f)));
            //}

            Game.DrawFrame();

            batch.End();
        }

        public override void Dispose()
        {
        }
    }
}
