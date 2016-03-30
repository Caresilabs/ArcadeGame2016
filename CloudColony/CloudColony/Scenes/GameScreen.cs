using CloudColony.Framework;
using CloudColony.Framework.Tools;
using CloudColony.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CloudColony.Scenes
{
    public class GameScreen : Screen
    {
        public enum GameState
        {
            RUNNING, GAMEOVER, PAUSED
        }

        public Camera2D Camera { get; private set; }

        public GameState State { get; private set; }

        public World World { get; private set; }

        public override void Init()
        {
            this.Camera = new Camera2D(17.5f, 10f);
            this.World = new World();
        }

        public override void Update(float delta)
        {
            switch (State)
            {
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

            // Draw World
            batch.Begin(SpriteSortMode.BackToFront,
                     BlendState.AlphaBlend,
                     SamplerState.LinearClamp,
                     null,
                     null,
                     null,
                     Camera.GetMatrix());

            // Draw stuff

            batch.End();
        }

        public override void Dispose()
        {
        }
    }
}
