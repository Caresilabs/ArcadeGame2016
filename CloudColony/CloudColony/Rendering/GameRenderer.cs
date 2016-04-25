using CloudColony.Framework;
using Microsoft.Xna.Framework.Graphics;
using CloudColony.Framework.Tools;
using CloudColony.Logic;
using Microsoft.Xna.Framework;
using System;

namespace CloudColony.Rendering
{
    public class GameRenderer : IRenderable, IUpdate
    {
        public Camera2D Camera { get; private set; }

        public World World { get; private set; }

        public Sprite Background { get; private set; }

        public Sprite Parallax1 { get; private set; }
        public Sprite Parallax2 { get; private set; }

        private float scrollX;

        public GameRenderer(World world)
        {
            this.Camera = new Camera2D(17.5f, 10f);
            this.Background = new Sprite(CC.GameBackground, Camera.GetWidth() / 2f, Camera.GetHeight() / 2f, Camera.GetWidth(), Camera.GetHeight());
            this.Background.Color = Color.White * 0.8f;//0.65f;
            Background.ZIndex = 1f;
            this.World = world;

            Parallax1 = new Sprite(new TextureRegion(CC.ParallaxTexture, 0, 0, 160, 90), Camera.GetWidth() / 2f, Camera.GetHeight() / 2f, Camera.GetWidth(), Camera.GetHeight());
            Parallax1.ZIndex = 0.98f;
            Parallax1.Color = Color.White * 0.75f;
            Parallax1.DrawOffset = new Vector2(0, Parallax1.Size.Y / 2f);

            Parallax2 = new Sprite(new TextureRegion(CC.ParallaxTexture, 0, 90, 160, 90), Camera.GetWidth() / 2f, Camera.GetHeight() / 2f, Camera.GetWidth(), Camera.GetHeight());
            Parallax2.ZIndex = 0.99f;
            Parallax2.Color = Color.White * 0.55f;
            Parallax2.DrawOffset = new Vector2(0, Parallax2.Size.Y / 2f);

            scrollX = Camera.GetWidth() * 3;
        }


        public void Update(float delta)
        {
            scrollX += .18f * delta;
            // scrollX %= Camera.GetWidth() * 2;
        }

        public void Draw(SpriteBatch batch)
        {
            // Draw World
            batch.Begin(SpriteSortMode.BackToFront,
                     BlendState.AlphaBlend,
                     SamplerState.PointWrap,
                     null,
                     null,
                     null,
                     Camera.GetMatrix());

            Background.Draw(batch);

            {
                float px2Scale = 0.43f;
                // Draw the texture, if it is still onscreen.

                Parallax2.SetPosition((((px2Scale * scrollX - (Camera.GetWidth() * 1.0f))) % (Camera.GetWidth() * 2)) - Camera.GetWidth(), Parallax2.Position.Y);
                Parallax2.Draw(batch);

                Parallax2.SetPosition(((px2Scale * (scrollX + Camera.GetWidth() * 0.0f)) % (Camera.GetWidth() * 2)) - Camera.GetWidth(), Parallax2.Position.Y);
                Parallax2.Draw(batch);

                Parallax1.SetPosition(((scrollX - Camera.GetWidth()) % (Camera.GetWidth() * 2)) - Camera.GetWidth(), Parallax1.Position.Y);
                Parallax1.Draw(batch);

                Parallax1.SetPosition((scrollX % (Camera.GetWidth() * 2)) - Camera.GetWidth(), Parallax1.Position.Y);
                Parallax1.Draw(batch);
            }

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
