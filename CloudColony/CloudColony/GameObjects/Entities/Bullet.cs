using CloudColony.Framework;
using CloudColony.Logic;

namespace CloudColony.GameObjects.Entities
{
    public class Bullet : Entity
    {
        public Bullet(World world, TextureRegion region, float x, float y, float width, float height) : base(world, region, x, y, width, height)
        {
        }
    }
}
