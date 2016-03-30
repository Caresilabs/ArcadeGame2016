using CloudColony.Framework;
using CloudColony.Framework.Tools;
using CloudColony.Logic;
using CloudColony.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public float ReadyTime { get; private set; }

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
                    ReadyTime += delta;
                    if (ReadyTime >= 3)
                        State = GameState.RUNNING;

                    World.Update(delta);
                    break;
                case GameState.RUNNING:
                    World.Update(delta);
                    break;
                case GameState.GAMEOVER:
                    break;
                case GameState.PAUSED:
                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            // Clear Screen
            Graphics.Clear(Color.Green);

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
                        batch.DrawString(CC.Font, "" + (int)(3 - ReadyTime), new Vector2(CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT / 2f),
                            Color.Red, 0, new Vector2(8, 8), 10 + (ReadyTime - (float)Math.Floor(ReadyTime)) * 2, SpriteEffects.None, 0);
                        break;
                    case GameState.RUNNING:
                        break;
                    case GameState.GAMEOVER:
                        break;
                    case GameState.PAUSED:
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
