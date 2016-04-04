using CloudColony.Framework;
using CloudColony.Framework.Tools;
using CloudColony.Logic;
using CloudColony.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CloudColony.Scenes
{
    public class GameScreen : Screen
    {
        public enum GameState
        {
            READY, RUNNING, GAMEOVER, PAUSED, EXIT
        }

        public Camera2D UICamera { get; private set; }

        public GameState State { get; private set; }

        public GameRenderer Renderer { get; private set; }

        public World World { get; private set; }

        public Sprite WinSprite { get; private set; }

        public float TotalTime { get; private set; }

        public float ExitTime { get; private set; }

        public override void Init()
        {
            this.UICamera = new Camera2D(CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT);
            this.World = new World();
            this.Renderer = new GameRenderer(World);
        }

        public override void Update(float delta)
        {
            TotalTime += delta;

            switch (State)
            {
                case GameState.READY:
                    if (TotalTime < 1.4f)
                        World.SetReady();

                    World.Update(delta);

                    if (World.State == World.WorldState.RUNNING)
                        State = GameState.RUNNING;
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
                    }

                    if (World.State == World.WorldState.BLUEWON)
                    {
                        State = GameState.GAMEOVER;
                        WinSprite = new Sprite(CC.WinBlue, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.45f, 96, 64);
                    }

                    break;
                case GameState.GAMEOVER:
                    if (InputHandler.GetButtonState(PlayerIndex.One, PlayerInput.Start) == InputState.Released ||
                      InputHandler.GetButtonState(PlayerIndex.Two, PlayerInput.Start) == InputState.Released)
                    {
                        State = GameState.EXIT;
                        ExitTime = 1.5f;
                    }

                    WinSprite.SetScale(MathHelper.Lerp(WinSprite.Scale.X, 6f, delta * 4f));

                    World.Update(delta);

                    break;
                case GameState.PAUSED:
                    if (CC.AnyKeyPressed(PlayerIndex.One) || CC.AnyKeyPressed(PlayerIndex.Two))
                    {
                        State = GameState.READY;
                        World.SetReady();
                    }
                    break;
                case GameState.EXIT:
                    World.Update(delta);

                    if (ExitTime <= 0)
                        SetScreen(new MainMenuScreen());
                    else
                        ExitTime -= delta;
                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            // Clear Screen
            Graphics.Clear(Color.Black);

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
                            Color.Red, 0, CC.Font.MeasureString(countdown) / 2f, 5 + (World.ReadyTime - (float)Math.Floor(World.ReadyTime)) * 2, SpriteEffects.None, 0);
                        break;
                    case GameState.RUNNING:
                        break;
                    case GameState.GAMEOVER:

                        WinSprite.Draw(batch);

                        string continueText = "Press start to continue...";
                        batch.DrawString(CC.Font, continueText, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.67f),
                            Color.White, 0, CC.Font.MeasureString(continueText) / 2f, 2.2f, SpriteEffects.None, 0);

                        break;
                    case GameState.PAUSED:
                        string txt = "PAUSED";
                        batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.5f),
                            Color.Green, 0, CC.Font.MeasureString(txt) / 2f, 3.5f + (float)((Math.Sin(TotalTime * 5) + 1) / 15f), SpriteEffects.None, 0);
                        break;
                    default:
                        break;
                }
            }

            if (TotalTime < 1.5f)
            {
                batch.Draw(CC.Pixel, new Rectangle(0, 0, CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT), CC.Pixel, Color.Black * (1.5f - (TotalTime / 1.5f)));
            }

            if (State == GameState.EXIT)
            {
                batch.Draw(CC.Pixel, new Rectangle(0, 0, CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT), CC.Pixel, Color.Black * (1.5f - (ExitTime / 1.5f)));
            }

            batch.End();

        }

        public override void Dispose()
        {
        }
    }
}
