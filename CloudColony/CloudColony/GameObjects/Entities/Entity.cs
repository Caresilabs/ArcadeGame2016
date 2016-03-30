using CloudColony.Framework;
using CloudColony.Logic;
using Microsoft.Xna.Framework;

namespace CloudColony.GameObjects.Entities
{
    public class Entity : Sprite
    {
        public World World { get; private set; }

        public Vector2 Velocity { get { return velocity; } }
        private Vector2 velocity;

        public Entity(World world) : base(null, 0,0,0,0)
        {
            this.World = world;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            position += velocity * delta;
        }
    }
}
