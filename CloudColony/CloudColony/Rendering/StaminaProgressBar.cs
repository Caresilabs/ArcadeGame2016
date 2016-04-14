using CloudColony.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CloudColony.Rendering
{
    public class StaminaProgressBar : IRenderable
    {
        public Vector2 Position { get; set; }

        private readonly Color color;
        private float percent;

        public StaminaProgressBar(Color color)
        {
            this.color = color;
        }

        public void SetPercentage(float percent)
        {
            this.percent = percent;
        }

        public void Draw(SpriteBatch batch)
        {
            float angle = percent * (float)(Math.PI * 2f);

            for (float i = 0; i < angle; i += 0.28f)
            {
                batch.Draw(CC.Pixel, Position + new Vector2(0.3f * (float)Math.Cos(i), 0.3f * (float)Math.Sin(i)),
                    CC.Pixel, color, i, Vector2.Zero, 0.08f, SpriteEffects.None, 0);
            }
        }
    }
}
