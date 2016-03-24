using CloudColony.Framework;
using CloudColony.Framework.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CloudColony.Scenes
{
    public class GameScreen : Screen
    {
        public Camera2D Camera { get; private set; }

        public override void Init()
        {
            this.Camera = new Camera2D(17.5f, 10f);
        }

        public override void Update(float delta)
        {
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
