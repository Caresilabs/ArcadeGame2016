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

        public const float SEPARATION_WEIGHT = 12f;
        public const float COHESION_WEIGHT = 2f;
        public const float ALIGNMENT_WEIGHT = 2f;

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

            Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
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

            if (position.Y < Size.Y / 2f)
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
            Vector2 pvj = Vector2.Zero;
            foreach (var b in Player.Ships)
            {
                if (this != b)
                {
                    pvj += b.velocity;
                }
            }
            pvj /= (Player.Ships.Count - 1);
            return (pvj - velocity) * 0.01f;
        }

        private Vector2 Cohesion()
        {
            Vector2 pcj = Vector2.Zero;
            int neighborCount = 0;
            foreach (var b in Player.Ships)
            {
                if (this != b && Distance(b.position, position) <= 3)
                {
                    pcj += b.position;
                    neighborCount++;
                }
            }
            pcj /= neighborCount + 1;
            return (pcj - position) * 0.01f;
        }

        private Vector2 Separation()
        {
            Vector2 vec = Vector2.Zero;
            int neighborCount = 0;
            foreach (var b in Player.Ships)
            {
                if (this != b)
                {
                    var distance = Distance(position, b.position);
                    if (distance > 0 && distance < 5)
                    {
                        var deltaVector = position - b.position;
                        deltaVector.Normalize();
                        deltaVector /= distance;
                        vec += deltaVector;
                        neighborCount++;
                    }
                }
            }
            Vector2 averageSteeringVector = (neighborCount > 0) ? (vec / neighborCount) : Vector2.Zero;
            return averageSteeringVector;
        }

        private float Distance(Vector2 v1, Vector2 v2)
        {
            return (v1 - v2).Length();
        }
    }
}
