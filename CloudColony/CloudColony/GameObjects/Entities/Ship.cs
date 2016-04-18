using System;
using CloudColony.Framework;
using CloudColony.GameObjects.Targets;
using CloudColony.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CloudColony.GameObjects.Powerups;
using CloudColony.Framework.Tools;

namespace CloudColony.GameObjects.Entities
{
    public class Ship : Entity
    {
        private const float MAX_HEALTH = 100;

        private const float MAX_SHIELD_HEALTH = 50;
        private const float SHIELD_COST = 1.1f;
        private const float SHIELD_DAMAGE = 3;

        private const float FIRE_RATE = 0.8f;

        public const float SEPARATION_WEIGHT = 13f;
        public const float COHESION_WEIGHT = 3f;
        public const float ALIGNMENT_WEIGHT = 2f;

        public Player Player { get; private set; }

        public Target Target { get; set; }

        public float Health { get; private set; }

        public float Speed { get; set; }
        public float MaxSpeed { get; set; }

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

            MaxSpeed = 3f;
            velocity.X = (int)player.Index == 0 ? MaxSpeed : -MaxSpeed;

            this.ShieldSprite = new Sprite(shieldTexture, 0, 0, 1.25f, 1.25f);
            ShieldSprite.ZIndex = 0.35f;
            ShieldSprite.Color = new Color(255, 255, 255, 255) * 0.3f;

            ZIndex = 0.4f + MathUtils.Random(0.001f, 0.005f);

            AddAnimation("Move", new FrameAnimation(CC.Atlas, 0 + ((int)player.Index == 0 ? 4 : 36), 0, 32, 32, 2, 0.3f, new Point(0, 1)))
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

            //MaxSpeed = 2.8f + (((float)World.MAX_NUM_SHIPS / Player.Ships.Count) * 0.3f);
            MaxSpeed = MathHelper.Lerp(5.5f, 3f, Player.Ships.Count / (float)World.MAX_NUM_SHIPS);


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
            }

            SeekTarget(delta);

            CapSpeed(delta);

            if (Target.Done)
                Target = Player;

            shootDelayTimer += delta;

            CheckCollision(delta);

            if (Health <= 0)
            {
                IsDead = true;
                World.SpawnEffect(Rendering.SpriteFX.EffectType.DESTROY, position);
                return;
            }

            KeepInside();

            if (ShieldHealth > 0)
            {
                ShieldSprite.SetPosition(position);
                ShieldSprite.SetScale((ShieldHealth / MAX_SHIELD_HEALTH) - 0.25f);
                ShieldHealth -= delta * 2;

                if (!Owner.TryDrainStamina(MathHelper.Lerp(10, 1, Owner.Ships.Count / (float)World.MAX_NUM_SHIPS) * SHIELD_COST * delta))
                    ShieldHealth = 0;
            }

            Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
        }

        private void CheckCollision(float delta)
        {
            var colliders = World.HashGrid.GetPossibleColliders(this);
            foreach (var enemy in colliders)
            {
                if (enemy.Owner == Player)
                    continue;

                if (Player.Ships.Count == 1)
                {
                    if (enemy is Bullet)
                    {
                        if ((enemy.Position - position).Length() <= 1.7f && Health <= 60)
                            World.Slowmotion();
                    }
                }

                if (!enemy.Bounds.Intersects(Bounds))
                    continue;

                if (enemy is Bullet)
                {
                    enemy.IsDead = true;
                    if (ShieldHealth > 0)
                    {
                        ShieldHealth -= Bullet.DAMAGE;
                    }
                    else
                    {
                        Health -= Bullet.DAMAGE;
                        World.SpawnEffect(Rendering.SpriteFX.EffectType.HIT, position);
                    }
                }

                if (enemy is Ship)
                {
                    Ship otherShip = enemy as Ship;
                    if (ShieldHealth > 0)
                    {
                        otherShip.Health -= SHIELD_DAMAGE * delta;
                        ShieldHealth -= SHIELD_DAMAGE * delta * 1.5f;
                        if (MathUtils.Random(0, 1f) < 0.01f)
                            World.SpawnEffect(Rendering.SpriteFX.EffectType.HIT, otherShip.position);
                    }
                }

                if (enemy is Powerup)
                {
                    enemy.IsDead = true;
                    Player.ActivePowerup = (Powerup)enemy;
                    Player.ActivePowerup.Init(Player);
                    World.SpawnEffect(Rendering.SpriteFX.EffectType.POWERUP, enemy.Position);
                }

            }
        }

        public bool CanShoot()
        {
            return shootDelayTimer >= FIRE_RATE;
        }

        public void ActivateShield()
        {
            if (ShieldHealth <= 0)
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
            desiredVelocity *= MaxSpeed;
            velocity += (desiredVelocity - velocity) * 0.14f;
            // MaxSpeed();
        }

        private void CapSpeed(float delta)
        {
            velocity.Normalize();
            Speed = MathHelper.Lerp(Speed, MaxSpeed, delta * 1f);
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
