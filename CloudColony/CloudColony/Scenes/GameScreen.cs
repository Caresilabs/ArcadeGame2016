using CloudColony.Framework;
using CloudColony.Framework.Tools;
using CloudColony.Logic;
using CloudColony.Rendering;
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
        }

        public override void Dispose()
        {
        }
    }
}
