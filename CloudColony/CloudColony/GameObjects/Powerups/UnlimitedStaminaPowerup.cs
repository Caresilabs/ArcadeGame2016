using CloudColony.Framework;
using CloudColony.Logic;

namespace CloudColony.GameObjects.Powerups
{
    public class UnlimitedStaminaPowerup : Powerup
    {
        public UnlimitedStaminaPowerup(World world, TextureRegion region, float x, float y) 
            : base(world, region, x, y)
        {
        }

        public override void RunPower()
        {
            if (ActiveTime >= 4)
                Done = true;

            Owner.Stamina = Player.STAMINA_MAX;
        }
    }
}
