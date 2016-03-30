using CloudColony.GameObjects.Entities;
using Microsoft.Xna.Framework;

namespace CloudColony.GameObjects.Targets
{
    public class FlankTarget : Target
    {
        public const float COST = 50;

        private const float FLANK_DISTANCE = 5;

        private readonly int direction;
        private readonly Vector2 mid;

        public Vector2 Position
        {
            get
            {
                var right = mid - Player.Position;
                right.Normalize();

                float x = right.X;
                right.X = right.Y;
                right.Y = -x;
                return mid + (right * FLANK_DISTANCE * direction);
            }
        }

        public bool Done
        {
            get
            {
                // TODO dont use position.. cpu hog
                return (Ship.Position - Position).LengthSquared() < 1;
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
        }
    }
}
