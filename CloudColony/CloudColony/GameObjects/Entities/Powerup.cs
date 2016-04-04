using CloudColony.Framework;
using CloudColony.Logic;

namespace CloudColony.GameObjects.Entities
{
    public class Powerup : Entity
    {
        public Powerup(World world, Player owner, TextureRegion region, float x, float y, float width, float height) 
            : base(world, owner, region, x, y, width, height)
        {
        }
    }
}
