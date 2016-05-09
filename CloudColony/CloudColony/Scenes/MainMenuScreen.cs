using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace CloudColony.Scenes
{
    public class MainMenuScreen : Screen
    {
        public bool PlayerRedReady { get; private set; }
        public bool PlayerBlueReady { get; private set; }

        public float TotalTime { get; private set; }

        public float PlayDelayTime { get; private set; }

        public bool BothReady { get; set; }

        public Sprite Background { get; private set; }

        public Sprite RedBackground { get; private set; }
        public Sprite BlueBackground { get; private set; }
        public Sprite CoinBackground { get; private set; }

        private Sprite logo;

        public override void Init()
        {
            this.PlayerRedReady = false;
            this.PlayerBlueReady = false;
            this.BothReady = false;

            InitUI();

            MediaPlayer.Volume = 0.33f;
        }

        private void InitUI()
        {
            logo = new Sprite(CC.Logo, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.235f, 358, 64);
            logo.SetScale(2.3f);

            Background = new Sprite(CC.MenuBackground, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT / 2f, CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT);
            Background.ZIndex = 1f;

            RedBackground = new Sprite(CC.Button2, CC.VIEWPORT_WIDTH * 0.25f, CC.VIEWPORT_HEIGHT * 0.55f, 600, 180);
            RedBackground.ZIndex = 0.9f;

            BlueBackground = new Sprite(CC.Button2, CC.VIEWPORT_WIDTH * 0.75f, CC.VIEWPORT_HEIGHT * 0.55f, 600, 180);
            BlueBackground.ZIndex = 0.9f;

            CoinBackground = new Sprite(CC.Button1, CC.VIEWPORT_WIDTH * 0.5f, CC.VIEWPORT_HEIGHT * 0.85f, 500, 140);
            CoinBackground.ZIndex = 0.9f;
        }

        public override void Update(float delta)
        {
            TotalTime += delta;

            // Hack so input wont happen after every game
            if (TotalTime > 0.4f)
            {
                if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Side) == InputState.Released ||
                       InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Side) == InputState.Released)
                {
                    SetScreen(new CreditsScreen());
                    return;
                }

                if (CC.AnyKeyJustClicked(PlayerIndex.One))
                    PlayerRedReady = true;

                if (CC.AnyKeyJustClicked(PlayerIndex.Two))
                    PlayerBlueReady = true;
            }

            if (PlayerRedReady && PlayerBlueReady)
            {
                PlayDelayTime += delta;

                if (PlayDelayTime >= 1f)
                {
                    if (!BothReady)
                    {
                        SetScreen(new GameScreen());
                        CC.PlayGameSound.Play();
                        CC.PlayGameSound.Play();
                        MediaPlayer.Volume = 0.12f;
                        BothReady = true;
                    }
                }
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
            RedBackground.Draw(batch);
            if (PlayerRedReady)
            {
                string txt = "Player 1 READY";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.25f, CC.VIEWPORT_HEIGHT * 0.55f),
                    Color.Red, (float)Math.Sin(TotalTime * 10) / 10f, CC.Font.MeasureString(txt) / 2f, 1.45f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 1 - Press any key";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.25f, CC.VIEWPORT_HEIGHT * 0.55f),
                    Color.Red, 0, CC.Font.MeasureString(txt) / 2f, 1.15f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
            }

            // Draw player blue side
            BlueBackground.Draw(batch);
            if (PlayerBlueReady)
            {
                string txt = "Player 2 READY";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.75f, CC.VIEWPORT_HEIGHT * 0.55f),
                    Color.Blue, (float)Math.Sin(TotalTime * 10) / 10f, CC.Font.MeasureString(txt) / 2f, 1.45f, SpriteEffects.None, 0);
            }
            else
            {
                string txt = "Player 2 - Press any key";
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH * 0.75f, CC.VIEWPORT_HEIGHT * 0.55f),
                    Color.Blue, 0, CC.Font.MeasureString(txt) / 2f, 1.15f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
            }

            // Draw logo
            logo.SetPosition(logo.Position.X, logo.Position.Y + (float)Math.Sin(TotalTime * 2) / 2.5f);
            logo.Draw(batch);

            // Draw insert coin
            {
                CoinBackground.Draw(batch);

                string insert = "Insert coin";
                batch.DrawString(CC.Font, insert, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.86f),
                     new Color(56, 45, 0) * (float)((Math.Sin(TotalTime * 5.0f) + 1) / 2f), 0, CC.Font.MeasureString(insert) / 2f, 1.9f, SpriteEffects.None, 0);
            }

            // Copy right.
            string copy = "...C.C.B.N. ALERT... Side button for credits... github.com/Caresilabs/ArcadeGame2016... " + "(c) " + DateTime.Now.Year;
            //batch.DrawString(CC.Font, copy, new Vector2(42, 29f),
            //    Color.WhiteSmoke * (float)((Math.Sin(TotalTime * 2.5f) + 1) / 2f), 0, Vector2.Zero, 1.15f, SpriteEffects.None, 0);
            batch.DrawString(CC.Font, copy, new Vector2(CC.VIEWPORT_WIDTH - (TotalTime * 150) % (CC.VIEWPORT_WIDTH + CC.Font.MeasureString(copy).X * 1.15f), 29f),
               Color.WhiteSmoke, 0, Vector2.Zero, 1.15f, SpriteEffects.None, 0);



            Game.DrawFrame();

            batch.End();
        }

        public override void Dispose()
        {
        }
    }
}
