using CloudColony.Framework;
using CloudColony.Logic;
using Microsoft.Xna.Framework;

namespace CloudColony.GameObjects.Entities
{
    public class Entity : Sprite
    {
        public World World { get; private set; }

        public Vector2 Velocity { get { return velocity; } }
        protected Vector2 velocity;

        public bool IsDead { get; protected set; }

        public Entity(World world, TextureRegion region, float x, float y, float width, float height) : base(region, x, y, width, height)
        {
            this.World = world;
            this.velocity = new Vector2();
            this.IsDead = false;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            position += velocity * delta;
        }
    }
}
