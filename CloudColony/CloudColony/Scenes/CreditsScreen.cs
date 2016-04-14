﻿using CloudColony.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CloudColony.Scenes
{
    public class CreditsScreen : Screen
    {

        private string[] credits = {
            "Created by:", "",

            "Programmer:", "SIMON BOTHEN", "",

            "Art & Sound:", "SEBASTIAN LIND", "",

            "(c) " + DateTime.Now.Year

        };

        public Sprite Background { get; private set; }

        private float time;
        private Sprite logo;

        private Sprite simon;
        private Sprite sebastian;

        public override void Init()
        {
            this.logo = new Sprite(CC.Logo, CC.VIEWPORT_WIDTH / 2f, 240, 358, 64);
            logo.SetScale(1.6f);

            this.Background = new Sprite(CC.MenuBackground, CC.VIEWPORT_WIDTH / 2f, CC.VIEWPORT_HEIGHT / 2f, CC.VIEWPORT_WIDTH, CC.VIEWPORT_HEIGHT);
            Background.ZIndex = 1f;

            simon = new Sprite(CC.Simon, CC.VIEWPORT_WIDTH * 0.3f, CC.VIEWPORT_HEIGHT * 0.45f, 153, 168);
            sebastian = new Sprite(CC.Sebastian, CC.VIEWPORT_WIDTH * 0.7f, CC.VIEWPORT_HEIGHT * 0.65f, 169, 127);
        }

        public override void Update(float delta)
        {
            if (CC.AnyKeyPressed(PlayerIndex.One) || CC.AnyKeyPressed(PlayerIndex.Two))
            {
                SetScreen(new MainMenuScreen());
            }

            time += delta;
        }

        public override void Draw(SpriteBatch batch)
        {
            // Clear Screen
            Graphics.Clear(Color.Black);

            batch.Begin(SpriteSortMode.BackToFront,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    null);

            Background.Draw(batch);

            logo.Rotation = (float)Math.Sin(time * 10) / 50f;
            logo.Draw(batch);

            simon.Draw(batch);
            sebastian.Draw(batch);

            float y = 400;
            foreach (var txt in credits)
            {
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH / 2f, y),
                    Color.WhiteSmoke, (float)Math.Sin(time * 10 + y) / 30f, CC.Font.MeasureString(txt) / 2f, 1.6f, SpriteEffects.None, 0);

                y += 50;
            }

            Game.DrawFrame();

            batch.End();
        }

        public override void Dispose()
        {
        }
    }
}
