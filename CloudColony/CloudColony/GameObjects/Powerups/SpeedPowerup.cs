using System;
using CloudColony.Framework;
using CloudColony.Logic;
using CloudColony.GameObjects.Entities;

namespace CloudColony.GameObjects.Powerups
{
    public class SpeedPowerup : Powerup
    {
        public SpeedPowerup(World world, TextureRegion region, float x, float y) 
            : base(world, region, x, y)
        {
        }

        public override void RunPower()
        {
            if (ActiveTime >= 4.5f)
                Done = true;

            foreach (var ship in Owner.Ships)
            {
                ship.Speed = Ship.MAX_SPEED * 1.45f;
            }
        }
    }
}
