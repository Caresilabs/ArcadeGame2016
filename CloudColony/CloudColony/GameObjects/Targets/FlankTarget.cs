using CloudColony.GameObjects.Entities;
using CloudColony.Logic;
using Microsoft.Xna.Framework;

namespace CloudColony.GameObjects.Targets
{
    public class FlankTarget : Target
    {
        public const float COST = 45;

        private const float FLANK_DISTANCE = 3f;

        private readonly int direction;
        private readonly Vector2 mid;
        private readonly float range;

        private float time;

        public Vector2 Position
        {
            get
            {
                var tmp = mid - Player.Position;
                tmp.Normalize();
                tmp *= range;

                var right = mid - Player.Position;
                right.Normalize();

                float x = right.X;
                right.X = right.Y;
                right.Y = -x;

                var p = tmp + Player.Position + (right * FLANK_DISTANCE * direction);
                KeepInside(ref p);
                return p;
            }
        }

        public bool Done
        {
            get
            {
                // TODO dont use position.. cpu hog
                return time >= 2.1f || (Ship.Position - Position).LengthSquared() < 1;
            }
        }

        public Ship Ship { get; private set; }
        public Player Player { get; private set; }

        public FlankTarget(Ship ship, Player player, int direction)
        {
            this.Ship = ship;
            this.Player = player;
            this.mid = ship.Position;
            this.direction = direction;
            this.range = (player.Position - ship.Position).Length(); //1.8f;  
        }


        private void KeepInside(ref Vector2 position)
        {
            if (position.X < 0)
            {
                position.X = 0;
            }
            else
            if (position.X > World.WORLD_WIDTH)
            {
                position.X = World.WORLD_WIDTH;
            }

            if (position.Y < 0)
            {
                position.Y = 0;
            }
            else
            if (position.Y > World.WORLD_HEIGHT)
            {
                position.Y = World.WORLD_HEIGHT;
            }
        }

        public void Update(float delta)
        {
            time += delta;
        }
    }
}
