using CloudColony.Framework;
using CloudColony.GameObjects.Entities;
using CloudColony.Logic;

namespace CloudColony.GameObjects.Powerups
{
    public abstract class Powerup : Entity
    {
        public bool Done { get; protected set; }

        public float ActiveTime { get; private set; }

        public Powerup(World world, TextureRegion region, float x, float y) 
            : base(world, null, region, x, y, 0.55f, 0.55f)
        {
        }

        public void Init(Player player)
        {
            this.Owner = player;
            ActiveTime = 0;
        }

        public abstract void RunPower();

        public override void Update(float delta)
        {
            base.Update(delta);
            Rotation += delta * 0.7f;
            ActiveTime += delta;
        }
    }
}
