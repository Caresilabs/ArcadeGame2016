using CloudColony.Framework;
using CloudColony.GameObjects;
using CloudColony.GameObjects.Entities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

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

        public List<Entity> Entities { get; private set; }
        public List<Entity> DeadEntities { get; private set; }

        public Player PlayerRed { get; private set; }
        public Player PlayerBlue { get; private set; }
        public Player[] Players { get; private set; }

        public WorldState State { get; private set; }

        public float ReadyTime { get; private set; }

        public World()
        {
            this.Entities = new List<Entity>();
            this.DeadEntities = new List<Entity>();
            this.State = WorldState.READY;
            InitPopulation(25);
        }

        private void InitPopulation(int each)
        {
            // Init red
            PlayerRed = new Player(this, CC.PointerRed, PlayerIndex.One,  1.5f, 1.5f);
            for (int i = 0; i < each; i++)
            {
                Ship ship = new Ship(this, PlayerRed, CC.ShipBlue, PlayerRed, 1 + (i % (WORLD_WIDTH / 2f)), 4 - (int)((i * 2) / WORLD_WIDTH));
                Entities.Add(ship);
                PlayerRed.Ships.Add(ship);
            }

            // Init blue
            PlayerBlue = new Player(this, CC.PointerBlue, PlayerIndex.Two, WORLD_WIDTH - 1.5f, 8f);
            for (int i = 0; i < each; i++)
            {
                Ship ship = new Ship(this, PlayerBlue, CC.ShipRed, PlayerBlue, WORLD_WIDTH - (i % (WORLD_WIDTH / 2f)), 7 + (int)((i * 2) / WORLD_WIDTH));
                Entities.Add(ship);
                PlayerBlue.Ships.Add(ship);
            }

            this.Players = new Player[] {PlayerRed, PlayerBlue };
        }

        public void SetReady()
        {
            State = WorldState.READY;
            ReadyTime = 0;
        }

        public void SpawnBullet(Bullet bullet)
        {
            Entities.Add(bullet);
        }

        public void Update(float delta)
        {
            switch (State)
            {
                case WorldState.READY:
                    ReadyTime += delta;
                    if (ReadyTime >= 4)
                        State = WorldState.RUNNING;
                    break;
                case WorldState.RUNNING:
                    if (PlayerBlue.Ships.Count == 0)
                        State = WorldState.REDWON;

                    if (PlayerRed.Ships.Count == 0)
                        State = WorldState.BLUEWON;
                    break;
                case WorldState.REDWON:
                    break;
                case WorldState.BLUEWON:
                    break;
                default:
                    break;
            }

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

        public List<Entity> GetNearbyEntities(float radius)
        {
            return Entities;
        }
    }
}
