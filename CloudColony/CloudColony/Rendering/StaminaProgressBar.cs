using CloudColony.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CloudColony.Rendering
{
    public class StaminaProgressBar : IRenderable
    {
        public Vector2 Position { get; set; }

        private float percent;

        public StaminaProgressBar()
        {

        }

        public void SetPercentage(float percent)
        {
            this.percent = percent;
        }

        public void Draw(SpriteBatch batch)
        {
            float angle = percent * (float)(Math.PI * 2f);

            for (float i = 0; i < angle; i += 0.4f)
            {
                batch.Draw(CC.Atlas, Position + new Vector2(0.2f * (float)Math.Cos(i), 0.2f * (float)Math.Sin(i)),
                    CC.BulletBlue, Color.Violet, i, Vector2.Zero, 0.04f, SpriteEffects.None, 1);
            }
        }
    }
}
