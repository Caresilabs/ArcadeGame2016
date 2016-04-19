using System;
using Microsoft.Xna.Framework;
using CloudColony.Framework.Tools;

namespace CloudColony.GameObjects.Targets
{
    public class ExplosionTarget : Target
    {
        public const float COST = 75;

        private const float DISTANCE = 4f;

        private readonly Vector2 direction;

        private Player player;

        private float time;

        public ExplosionTarget(Player player)
        {
            this.player = player;
            this.direction = new Vector2((float)Math.Cos(MathUtils.Random(6.28f)), (float)Math.Sin(MathUtils.Random(6.28f)));
        }

        public bool Done
        {
            get
            {
                return time >= 3.0f;
            }
        }

        public Vector2 Position
        {
            get
            {
                return player.Position + (direction * DISTANCE);
            }
        }

        public void Update(float delta)
        {
            time += delta;
        }
    }
}
