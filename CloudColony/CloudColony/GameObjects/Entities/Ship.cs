using CloudColony.GameObjects.Targets;
using CloudColony.Logic;

namespace CloudColony.GameObjects.Entities
{
    public class Ship : Entity
    {
        public Player Player { get; private set; }

        public Target Target { get; private set; }

        public Ship(World world, Player player) : base(world)
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
