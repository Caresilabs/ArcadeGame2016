using CloudColony.Framework;
using CloudColony.Framework.Tools;
using CloudColony.Logic;
using System;

namespace CloudColony.GameObjects.Powerups
{
    public class ReviveShipPowerup : Powerup
    {
        public ReviveShipPowerup(World world, TextureRegion region, float x, float y) 
            : base(world, region, x, y)
        {
        }

        public override void RunPower()
        {
            var revives = Math.Min(World.MAX_NUM_SHIPS - Owner.Ships.Count, MathUtils.Random(2, 4));

            bool isRed = Owner.Index == Microsoft.Xna.Framework.PlayerIndex.One;
            World.SpawnShips(isRed ? revives : 0, isRed ? 0 : revives, Owner.Position);

            Done = true;
        }
    }
}
