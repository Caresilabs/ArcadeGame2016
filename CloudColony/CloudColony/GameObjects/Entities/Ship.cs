using CloudColony.Framework;
using CloudColony.GameObjects.Targets;
using CloudColony.Logic;

namespace CloudColony.GameObjects.Entities
{
    public class Ship : Entity
    {
        public Player Player { get; private set; }

        public Target Target { get; private set; }

        public Ship(World world, TextureRegion region, Player player, float x, float y) : base(world, region, x, y, 16, 16)
        {
            this.Player = player;
            this.Target = player;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

        }
    }
}
