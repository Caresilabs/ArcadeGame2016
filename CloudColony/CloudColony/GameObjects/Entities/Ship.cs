using System;
using CloudColony.Framework;
using CloudColony.GameObjects.Targets;
using CloudColony.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CloudColony.GameObjects.Powerups;

namespace CloudColony.GameObjects.Entities
{
    public class Ship : Entity
    {
        public const float MAX_SPEED = 3f;
        private const float MAX_HEALTH = 100;

        private const float MAX_SHIELD_HEALTH = 50;

        private const float FIRE_RATE = 0.5f;

        public const float SEPARATION_WEIGHT = 13f;
        public const float COHESION_WEIGHT = 3f;
        public const float ALIGNMENT_WEIGHT = 2f;

        public Player Player { get; private set; }

        public Target Target { get; set; }

        public float Health { get; private set; }

        public float Speed { get; set; }

        // Shoot
        private float shootDelayTimer;

        // Shield
        public float ShieldHealth { get; private set; }
        public Sprite ShieldSprite { get; private set; }

        public Ship(World world, Player owner, TextureRegion shieldTexture, Player player, float x, float y)
            : base(world, owner, null, x, y, 0.55f, 0.55f)
        {
            this.Player = player;
            this.Target = player;
            this.Health = MAX_HEALTH;
            this.shootDelayTimer = 0;
            velocity.X = (int)player.Index == 0 ? MAX_SPEED : -MAX_SPEED;

            this.ShieldSprite = new Sprite(shieldTexture, 0, 0, 1.25f, 1.25f);
            ShieldSprite.ZIndex = 0.35f;
            ShieldSprite.Color = new Color(255, 255, 255, 255) * 0.3f;

            ZIndex = 0.4f;

            AddAnimation("Move", new FrameAnimation(CC.Atlas, 0 + ((int)player.Index == 0 ? 4 : 36), 0, 32, 32, 2, 0.3f))
                .SetAnimation("Move");
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);

            if (ShieldHealth > 0)
                ShieldSprite.Draw(batch);
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            // Don't udpate player twice
            if (Target != Player)
            {
                Target.Update(delta);
            }

            var alignment = Alignment();
            var cohesion = Cohesion();
            var separation = Separation();

            var flock = (alignment * ALIGNMENT_WEIGHT) + (cohesion * COHESION_WEIGHT) + (separation * SEPARATION_WEIGHT);
            if (flock != Vector2.Zero)
            {
                velocity += flock * delta;

                // MaxSpeed();
            }

            SeekTarget(delta);

            MaxSpeed(delta);

            if (Target.Done)
                Target = Player;

            if (Health <= 0)
                IsDead = true;

            shootDelayTimer += delta;

            CheckCollision(delta);

            KeepInside();

            if (float.IsNaN(position.X))
            {
                velocity = Vector2.Zero;
            }

            ShieldSprite.SetPosition(position);
            ShieldSprite.SetScale((ShieldHealth / MAX_SHIELD_HEALTH) - 0.25f);
            ShieldHealth -= delta * 2;

            Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
        }

        private void CheckCollision(float delta)
        {
            foreach (var enemy in World.Entities)
            {
                if (enemy.Owner == Player)
                    continue;

                if (Player.Ships.Count == 1)
                {
                    if (enemy is Bullet)
                    {
                        if ((enemy.Position - position).Length() <= 2.1f && Health <= 60)
                            World.Slowmotion();
                    }
                }

                if (!enemy.Bounds.Intersects(Bounds))
                    continue;

                if (enemy is Bullet)
                {
                    Health -= Bullet.DAMAGE;
                    enemy.IsDead = true;
                }

                if (enemy is Powerup)
                {
                    enemy.IsDead = true;
                    Player.ActivePowerup = (Powerup)enemy;
                    Player.ActivePowerup.Init(Player);
                }

            }
        }

        public bool CanShoot()
        {
            return shootDelayTimer >= FIRE_RATE;
        }

        public void ActivateShield()
        {
            ShieldHealth = MAX_SHIELD_HEALTH;
        }

        public void Shoot()
        {
            ShieldHealth = 0;
            shootDelayTimer = 0;
            var dir = Velocity;
            dir.Normalize();

            Bullet bullet = new Bullet(World, Player, (int)Player.Index == 0 ? CC.BulletRed : CC.BulletBlue, position.X, position.Y, dir);
            World.SpawnBullet(bullet);
        }

        private void SeekTarget(float delta)
        {
            var desiredVelocity = (Target.Position - position);
            desiredVelocity.Normalize();
            desiredVelocity *= MAX_SPEED;
            velocity += (desiredVelocity - velocity) * 0.14f;
            // MaxSpeed();
        }

        private void MaxSpeed(float delta)
        {
            velocity.Normalize();
            Speed = MathHelper.Lerp(Speed, MAX_SPEED, delta * 2f);
            velocity *= Speed;
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
            if (Player.Ships.Count <= 1)
                return Vector2.Zero;

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
            if (Player.Ships.Count <= 1)
                return Vector2.Zero;

            Vector2 pcj = Vector2.Zero;
            int neighborCount = 0;
            foreach (var b in Player.Ships)
            {
                if (this != b && Distance(b.position, position) <= 3.5f)
                {
                    pcj += b.position;
                    neighborCount++;
                }
            }
            pcj /= neighborCount + 1;
            return (pcj - position) * 0.005f; // 0.01f
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
