using CloudColony.Framework;
using CloudColony.Framework.Tools;
using CloudColony.Logic;
using CloudColony.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace CloudColony.Scenes
{
    public class GameScreen : Screen
    {
        public enum GameState
        {
            READY, RUNNING, GAMEOVER, PAUSED
        }

        public Camera2D UICamera { get; private set; }

        public GameState State { get; private set; }

        public GameRenderer Renderer { get; private set; }

        public World World { get; private set; }

        public Sprite WinSprite { get; private set; }

        public float TotalTime { get; private set; }


        public override void Init()
        {
            this.UICamera = new Camera2D(CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT);
            this.World = new World();
            this.Renderer = new GameRenderer(World);

            MediaPlayer.Volume = 0.42f;
        }

        public override void Update(float delta)
        {
            TotalTime += delta;

            Renderer.Update(delta);

            switch (State)
            {
                case GameState.READY:
                    if (TotalTime < 0.5f)
                        World.SetReady();

                    World.Update(delta);

                    if (World.State == World.WorldState.RUNNING)
                    {
                        State = GameState.RUNNING;
                    }
                    break;
                case GameState.RUNNING:
                    if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released ||
                        InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
                    {
                        State = GameState.PAUSED;
                    }

                    World.Update(delta);

                    if (World.State == World.WorldState.REDWON)
                    {
                        State = GameState.GAMEOVER;
                        WinSprite = new Sprite(CC.WinRed, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.45f, 96, 64);
                        MediaPlayer.Volume = 0.21f;
                        CC.WinSound.Play();
                    }

                    if (World.State == World.WorldState.BLUEWON)
                    {
                        State = GameState.GAMEOVER;
                        WinSprite = new Sprite(CC.WinBlue, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.45f, 96, 64);
                        MediaPlayer.Volume = 0.21f;
                        CC.WinSound.Play();
                    }

                    break;
                case GameState.GAMEOVER:
                    if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released ||
                      InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
                    {
                        SetScreen(new MainMenuScreen());
                    }

                    WinSprite.SetScale(MathHelper.Lerp(WinSprite.Scale.X, 6f, delta * 4f));

                    World.Update(delta);

                    break;
                case GameState.PAUSED:
                    if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Side) == InputState.Released ||
                    InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Side) == InputState.Released)
                    {
                        SetScreen(new MainMenuScreen());
                    }
                    else if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released ||
                    InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
                    {
                        State = GameState.READY;
                        World.SetReady();
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            // Clear Screen
            Graphics.Clear(Color.White);

            Renderer.Draw(batch);

            batch.Begin(SpriteSortMode.BackToFront,
                     BlendState.AlphaBlend,
                     SamplerState.PointClamp,
                     null,
                     null,
                     null,
                     UICamera.GetMatrix());
            {
                switch (State)
                {
                    case GameState.READY:
                        string countdown = "" + (int)(3 - World.ReadyTime + 1);
                        countdown = countdown == "0" ? "GO!" : countdown;
                        batch.DrawString(CC.Font, countdown, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT / 2f),
                            Color.Black, 0, CC.Font.MeasureString(countdown) / 2f, 5 + (World.ReadyTime - (float)Math.Floor(World.ReadyTime)) * 2, SpriteEffects.None, 0);
                        break;
                    case GameState.RUNNING:
                        break;
                    case GameState.GAMEOVER:

                        WinSprite.Draw(batch);

                        string continueText = "Press start to continue...";
                        batch.DrawString(CC.Font, continueText, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.67f),
                            Color.Black, 0, CC.Font.MeasureString(continueText) / 2f, 2.2f, SpriteEffects.None, 0);

                        break;
                    case GameState.PAUSED:
                        string txt = "PAUSED";
                        batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.5f),
                            Color.Black, 0, CC.Font.MeasureString(txt) / 2f, 3.5f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);

                        txt = "-Side button to exit-";
                        batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.575f),
                            Color.Black, 0, CC.Font.MeasureString(txt) / 2f, 1.85f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
                        break;
                    default:
                        break;
                }
            }

            Game.DrawFrame();

            batch.End();

        }

        public override void Dispose()
        {
        }
    }
}
