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
            READY, RUNNING, GAMEOVER, PAUSED
        }

        public Camera2D UICamera { get; private set; }

        public GameState State { get; private set; }

        public GameRenderer Renderer { get; private set; }

        public World World { get; private set; }

        public override void Init()
        {
            this.UICamera = new Camera2D(CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT);
            this.World = new World();
            this.Renderer = new GameRenderer(World);
        }

        public override void Update(float delta)
        {
            switch (State)
            {
                case GameState.READY:
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
                        State = GameState.GAMEOVER;

                    if (World.State == World.WorldState.BLUEWON)
                        State = GameState.GAMEOVER;
                    break;
                case GameState.GAMEOVER:
                    SetScreen(new GameScreen());
                    break;
                case GameState.PAUSED:
                    if (CC.AnyKeyPressed(PlayerIndex.One) || CC.AnyKeyPressed(PlayerIndex.Two))
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
                        break;
                    case GameState.PAUSED:
                        string txt = "PAUSED";
                        batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT * 0.5f),
                            Color.Green, 0, CC.Font.MeasureString(txt) / 2f, 4.2f, SpriteEffects.None, 0);
                        break;
                    default:
                        break;
                }
            }
            batch.End();

        }

        public override void Dispose()
        {
        }
    }
}
