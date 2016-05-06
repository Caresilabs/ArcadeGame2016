using CloudColony.Framework;
using CloudColony.GameObjects;
using CloudColony.GameObjects.Entities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CloudColony.Framework.Tools;
using CloudColony.GameObjects.Powerups;
using CloudColony.Rendering;

namespace CloudColony.Logic
{
    public class World : IUpdate
    {
        public enum WorldState
        {
            READY, RUNNING, REDWON, BLUEWON
        }

        public const float WORLD_WIDTH = 17.5f;
        public const float WORLD_HEIGHT = 10f;

        public const int MAX_NUM_SHIPS = 30;

        public List<Entity> Entities { get; private set; }
        public List<Entity> DeadEntities { get; private set; }

        public List<SpriteFX> Effects { get; private set; }
        public SpriteFXPool FXPool { get; private set; }

        public SpatialHashGrid HashGrid { get; private set; }

        public Player PlayerRed { get; private set; }
        public Player PlayerBlue { get; private set; }
        public Player[] Players { get; private set; }

        public WorldState State { get; private set; }

        public float ReadyTime { get; private set; }

        public float SlowmoTime { get; private set; }

        public float PowerupTime { get; private set; }

        public World()
        {
            this.HashGrid = new SpatialHashGrid();
            this.HashGrid.Setup((int)(WORLD_WIDTH + 1), (int)(WORLD_HEIGHT + 1), 3.5f);
            this.Entities = new List<Entity>();
            this.DeadEntities = new List<Entity>();
            this.Effects = new List<SpriteFX>();
            this.FXPool = new SpriteFXPool();
            this.State = WorldState.READY;
            InitPopulation(MAX_NUM_SHIPS);
        }

        private void InitPopulation(int each)
        {
            // Init red
            PlayerRed = new Player(this, CC.PointerRed, PlayerIndex.One, 1.5f, 1.5f);

            // Init blue
            PlayerBlue = new Player(this, CC.PointerBlue, PlayerIndex.Two, WORLD_WIDTH - 1.5f, 8f);

            SpawnShips(each, each);

            this.Players = new Player[] { PlayerRed, PlayerBlue };
        }

        public void SpawnShips(int red, int blue, Vector2 pos)
        {
            for (int i = 0; i < red; i++)
            {
                Ship ship = new Ship(this, PlayerRed, CC.ShieldRed, PlayerRed, pos.X, pos.Y);
                Entities.Add(ship);
                PlayerRed.Ships.Add(ship);
            }

            for (int i = 0; i < blue; i++)
            {
                Ship ship = new Ship(this, PlayerBlue, CC.ShieldBlue, PlayerBlue, pos.X, pos.Y);
                Entities.Add(ship);
                PlayerBlue.Ships.Add(ship);
            }
        }

        public void SpawnShips(int red, int blue)
        {
            for (int i = 0; i < red; i++)
            {
                Ship ship = new Ship(this, PlayerRed, CC.ShieldRed, PlayerRed, 1 + (i % (WORLD_WIDTH / 2f)), 4 - (int)((i * 2) / WORLD_WIDTH));
                Entities.Add(ship);
                PlayerRed.Ships.Add(ship);
            }

            for (int i = 0; i < blue; i++)
            {
                Ship ship = new Ship(this, PlayerBlue, CC.ShieldBlue, PlayerBlue, WORLD_WIDTH - (i % (WORLD_WIDTH / 2f)), 7 + (int)((i * 2) / WORLD_WIDTH));
                Entities.Add(ship);
                PlayerBlue.Ships.Add(ship);
            }
        }

        public void SpawnEffect(SpriteFX.EffectType type, Vector2 pos, Color? color = null)
        {
            var fx = FXPool.GetObject();
            fx.SetPosition(pos);
            fx.ZIndex = 0.023f;
            fx.Color = color.HasValue ? color.Value : Color.White;

            switch (type)
            {
                case SpriteFX.EffectType.HIT:
                    fx.SetSize(0.55f, 0.55f);
                    fx.AddAnimation("fx", new FrameAnimation(CC.Atlas, 160, 386, 14, 18, 5, 0.08f, new Point(1, 0), false, false)).SetAnimation("fx");
                    break;
                case SpriteFX.EffectType.DESTROY:
                    fx.SetSize(1.1f, 1.1f);
                    fx.AddAnimation("fx", new FrameAnimation(CC.Atlas, 0, 476, 32, 32, 14, 0.06f, new Point(1, 0), false, false)).SetAnimation("fx");
                    break;
                case SpriteFX.EffectType.POWERUP:
                    fx.SetSize(0.46f, 0.46f);
                    fx.AddAnimation("fx", new FrameAnimation(CC.Atlas, 390, 386, 18, 16, 2, 0.17f, new Point(1, 0), false, false)).SetAnimation("fx");
                    break;
                case SpriteFX.EffectType.SHOOT_RED:
                    fx.SetSize(0.3f, 0.3f);
                    fx.AddAnimation("fx", new FrameAnimation(CC.Atlas, 100, 154, 8, 12, 4, 0.05f, new Point(1, 0), false, false)).SetAnimation("fx");
                    break;
                case SpriteFX.EffectType.SHOOT_BLUE:
                    fx.SetSize(0.4f, 0.4f);
                    fx.AddAnimation("fx", new FrameAnimation(CC.Atlas, 64, 154, 9, 10, 4, 0.05f, new Point(1, 0), false, false)).SetAnimation("fx");
                    break;
                default:
                    FXPool.ReleaseObject(fx);
                    return;
            }
            var sx = fx.Size * 0.5f;
            fx.DrawOffset = new Vector2(sx.X, sx.Y);
            Effects.Add(fx);
        }

        public void SetReady()
        {
            State = WorldState.READY;
            ReadyTime = 0;
        }

        public void SpawnBullet(Bullet bullet)
        {
            Entities.Add(bullet);
            SpawnEffect(bullet.Owner == PlayerRed ? SpriteFX.EffectType.SHOOT_RED : SpriteFX.EffectType.SHOOT_BLUE, 
                bullet.Position + bullet.Direction * 0.6f);
        }

        public void Update(float delta)
        {
            // Update FX
            foreach (var fx in Effects)
            {
                fx.Update(delta);

                if (fx.Done)
                    FXPool.ReleaseObject(fx);
            }
            Effects.RemoveAll(x => x.Done);


            if (SlowmoTime > 0)
            {
                SlowmoTime -= delta;
                delta *= 0.2f;
            }

            switch (State)
            {
                case WorldState.READY:
                    ReadyTime += delta;
                    if (ReadyTime >= 4)
                        State = WorldState.RUNNING;
                    break;
                case WorldState.RUNNING:
                    if (PlayerBlue.Ships.Count == 0)
                    {
                        State = WorldState.REDWON;
                        SlowmoTime = 2.5f;
                    }

                    if (PlayerRed.Ships.Count == 0)
                    {
                        State = WorldState.BLUEWON;
                        SlowmoTime = 2.5f;
                    }

                    UpdatePowerups(delta);
                    break;
                case WorldState.REDWON:
                    break;
                case WorldState.BLUEWON:
                    break;
                default:
                    break;
            }

            // Update our hashgrid
            HashGrid.ClearBuckets();
            HashGrid.AddObject(Entities);

            PlayerRed.Update(delta);
            PlayerBlue.Update(delta);

            foreach (var entity in Entities)
            {
                entity.Update(delta);

                if (entity.IsDead)
                    DeadEntities.Add(entity);
            }

            foreach (var dead in DeadEntities)
            {
                Entities.Remove(dead);
                if (dead is Ship)
                {
                    PlayerRed.Ships.Remove((Ship)dead);
                    PlayerBlue.Ships.Remove((Ship)dead);
                }
            }
            DeadEntities.Clear();
        }

        private void UpdatePowerups(float delta)
        {
            PowerupTime += delta;

            if (PowerupTime >= 7.3f && MathUtils.Random(1.0f) < 0.008f)
            {
                Powerup power = null;
                Vector2 pos = Vector2.Zero;

                // Find a safe place
                for (int i = 0; i < 20; i++)
                {
                    pos = new Vector2(MathUtils.Random(2, WORLD_WIDTH - 2), MathUtils.Random(2, WORLD_HEIGHT - 2));

                    bool collided = false;
                    foreach (var entity in Entities)
                    {
                        if ((entity.Position - pos).Length() <= 2.3f)
                        {
                            collided = true;
                            break;
                        }
                    }

                    if (!collided)
                        break;
                }

                // Random power
                switch (MathUtils.Random(3))
                {
                    case 0:
                        power = new SpeedPowerup(this, CC.RedPowerup, pos.X, pos.Y);
                        break;
                    case 1:
                        power = new UnlimitedStaminaPowerup(this, CC.GreenPowerup, pos.X, pos.Y);
                        break;
                    case 2:
                        power = new ReviveShipPowerup(this, CC.BluePowerup, pos.X, pos.Y);
                        break;
                    default:
                        break;
                }

                Entities.Add(power);
                PowerupTime = 0;
            }
        }

        public void Slowmotion()
        {
            SlowmoTime = 1.2f;
        }

    }
}
