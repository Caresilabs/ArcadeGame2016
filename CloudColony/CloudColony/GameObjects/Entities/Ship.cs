using System;
using CloudColony.Framework;
using CloudColony.GameObjects.Targets;
using CloudColony.Logic;
using Microsoft.Xna.Framework;

namespace CloudColony.GameObjects.Entities
{
    public class Ship : Entity
    {
        private const float MAX_SPEED = 2;
        private const float MAX_HEALTH = 100;

        public const float SEPARATION_WEIGHT = 10f;
        public const float COHESION_WEIGHT = 2f;
        public const float ALIGNMENT_WEIGHT = 22f;

        public Player Player { get; private set; }

        public Target Target { get; set; }

        public float Health { get; private set; }

        public Ship(World world, TextureRegion region, Player player, float x, float y) : base(world, region, x, y, 0.5f, 0.5f)
        {
            this.Player = player;
            this.Target = player;
            this.Health = MAX_HEALTH;
            velocity.X = (int)player.Index == 0 ? MAX_SPEED : -MAX_SPEED;
            Color = (int)player.Index == 0 ? Color.Red : Color.Blue;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            var alignment = Alignment();
            var cohesion = Cohesion();
            var separation = Separation();

            var flock = (alignment * ALIGNMENT_WEIGHT) + (cohesion * COHESION_WEIGHT) + (separation * SEPARATION_WEIGHT);
            if (flock != Vector2.Zero)
            {
                velocity += flock * delta;
                MaxSpeed();
            }

            SeekTarget(delta);

            if (Target.Done)
                Target = Player;

            if (Health <= 0)
                IsDead = true;

            KeepInside();
        }

        private void SeekTarget(float delta)
        {
            var desiredVelocity = (Target.Position - position);
            desiredVelocity.Normalize();
            desiredVelocity *= MAX_SPEED;
            velocity += (desiredVelocity - velocity) * 0.14f;
            MaxSpeed();
        }

        private void MaxSpeed()
        {
            velocity.Normalize();
            velocity *= MAX_SPEED;
        }

        private void KeepInside()
        {
            if (position.X < Size.X / 2f)
            {
                position.X = Size.X / 2f;
                velocity.X = Math.Abs(velocity.X);
            }

            if (position.X > World.WORLD_WIDTH - Size.X / 2f)
            {
                position.X = World.WORLD_WIDTH - Size.X / 2f;
                velocity.X = -Math.Abs(velocity.X);
            }

            if (position.Y < Size.Y /2f)
            {
                position.Y = Size.Y / 2f;
                velocity.Y = Math.Abs(velocity.Y);
            }

            if (position.Y > World.WORLD_HEIGHT - Size.Y / 2f)
            {
                position.Y = World.WORLD_HEIGHT - Size.Y / 2f;
                velocity.Y = -Math.Abs(velocity.Y);
            }
        }

        private Vector2 Alignment()
        {
            var NeighborCount = 0;

            Vector2 v = new Vector2();

            foreach (var boid in Player.Ships)
            {
                if (boid == this)
                    continue;

                if (Distance(position, boid.position) < 4)
                {
                    v += boid.velocity;
                    ++NeighborCount;
                }
            }

            if (NeighborCount == 0)
                return v;

            v /= NeighborCount;
            v.Normalize();

            return v;
        }

        private Vector2 Cohesion()
        {
            var NeighborCount = 0;

            Vector2 v = new Vector2();

            foreach (var boid in Player.Ships)
            {
                if (boid == this)
                    continue;

                if (Distance(position, boid.position) < 4)
                {
                    v += boid.position;
                    ++NeighborCount;
                }
            }

            if (NeighborCount == 0)
                return v;

            v /= NeighborCount;
            v = new Vector2(v.X - position.X, v.Y - position.Y);
            v.Normalize();

            return v;
        }

        private Vector2 Separation()
        {
            var NeighborCount = 0;

            Vector2 v = new Vector2();

            foreach (var boid in Player.Ships)
            {
                if (boid == this)
                    continue;

                if (Distance(position, boid.position) < 3)
                {
                    v += (boid.position - position) * 0.5f;
                    ++NeighborCount;
                }
            }

            if (NeighborCount == 0)
                return v;

            v /= NeighborCount;
            // v = new Vector2(v.X - position.X, v.Y - position.Y);
            v *= -1;
            v.Normalize();

            return v;
        }

        private float Distance(Vector2 v1, Vector2 v2)
        {
            return (v1 - v2).Length();
        }
    }
}
