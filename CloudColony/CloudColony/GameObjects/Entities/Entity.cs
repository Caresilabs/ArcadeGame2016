using CloudColony.Framework;
using CloudColony.Logic;
using Microsoft.Xna.Framework;
using System;

namespace CloudColony.GameObjects.Entities
{
    public class Entity : Sprite
    {
        public Player Owner { get; private set; }

        public World World { get; private set; }

        public Vector2 Velocity { get { return velocity; } }
        protected Vector2 velocity;

        public Circle Bounds { get; set; }

        public bool IsDead { get; set; }

        public Entity(World world, Player owner, TextureRegion region, float x, float y, float width, float height) : base(region, x, y, width, height)
        {
            this.World = world;
            this.Owner = owner;
            this.velocity = new Vector2();
            this.IsDead = false;
            this.Bounds = new Circle(position, Math.Max(width, height) / 2f);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            position += velocity * delta;
            Bounds.Center = position;
        }
    }
}
