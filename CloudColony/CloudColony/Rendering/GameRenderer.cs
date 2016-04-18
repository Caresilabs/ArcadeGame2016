
using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework.Graphics;
using CloudColony.Framework.Tools;
using CloudColony.Logic;
using Microsoft.Xna.Framework;

namespace CloudColony.Rendering
{
    public class GameRenderer : IRenderable
    {
        public Camera2D Camera { get; private set; }

        public World World { get; private set; }

        public Sprite Background { get; private set; }

        public GameRenderer(World world)
        {
            this.Camera = new Camera2D(17.5f, 10f);
            this.Background = new Sprite(CC.GameBackground, Camera.GetWidth() / 2f, Camera.GetHeight() / 2f, Camera.GetWidth(), Camera.GetHeight());
            this.Background.Color = Color.White * 0.65f;
            Background.ZIndex = 1f;
            this.World = world;
        }

        public void Draw(SpriteBatch batch)
        {
            // Draw World
            batch.Begin(SpriteSortMode.BackToFront,
                     BlendState.AlphaBlend,
                     SamplerState.PointClamp,
                     null,
                     null,
                     null,
                     Camera.GetMatrix());

            Background.Draw(batch);
            DrawObjects(batch);
            DrawPlayers(batch);

            batch.End();
        }

        private void DrawPlayers(SpriteBatch batch)
        {
            World.PlayerRed.Draw(batch);
            World.PlayerBlue.Draw(batch);
        }

        private void DrawObjects(SpriteBatch batch)
        {
            foreach (var obj in World.Entities)
                obj.Draw(batch);

            foreach (var obj in World.Effects)
                obj.Draw(batch);
        }
    }
}
