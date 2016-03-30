
using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework.Graphics;
using CloudColony.Framework.Tools;
using CloudColony.Logic;

namespace CloudColony.Rendering
{
    public class GameRenderer : IRenderable
    {
        public Camera2D Camera { get; private set; }

        public World World { get; private set; }

        public GameRenderer(World world)
        {
            this.Camera = new Camera2D(17.5f, 10f);
            this.World = world;
        }

        public void Draw(SpriteBatch batch)
        {
            // Draw World
            batch.Begin(SpriteSortMode.BackToFront,
                     BlendState.AlphaBlend,
                     SamplerState.LinearClamp,
                     null,
                     null,
                     null,
                     Camera.GetMatrix());

            DrawObjects(batch);
            DrawPlayers(batch);

            batch.End();
        }

        private void DrawPlayers(SpriteBatch batch)
        {
            foreach (var obj in World.Entities)
            {
                obj.Draw(batch);
            }
        }

        private void DrawObjects(SpriteBatch batch)
        {
            World.PlayerRed.Draw(batch);
            World.PlayerBlue.Draw(batch);
        }
    }
}
