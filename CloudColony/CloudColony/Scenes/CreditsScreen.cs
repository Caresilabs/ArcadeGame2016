using CloudColony.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CloudColony.Scenes
{
    public class CreditsScreen : Screen
    {

        private string[] credits = {
            "Created by:", "",

            "Lead Programmer:", "SIMON BOTHEN", "",

            "Art:", "SEBASTIAN LIND", "",

            "(c) 2016"

        };

        private float time;

        public override void Init()
        {
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


            var logoText = "Cloud Colony";
            batch.DrawString(CC.Font, logoText, new Vector2(CC.VIEWPORT_WIDTH / 2f, 230),
               Color.Crimson, (float)Math.Sin(time * 10) / 50f, CC.Font.MeasureString(logoText) / 2f, 2f, SpriteEffects.None, 0);

            float y = 350;
            foreach (var txt in credits)
            {
                batch.DrawString(CC.Font, txt, new Vector2(CC.VIEWPORT_WIDTH / 2f, y),
                    Color.Crimson, (float)Math.Sin(time * 10 + y) / 30f, CC.Font.MeasureString(txt) / 2f, 1.6f, SpriteEffects.None, 0);

                y += 50;
            }

            batch.End();
        }

        public override void Dispose()
        {
        }
    }
}
