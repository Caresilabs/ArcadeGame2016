using CloudColony.Framework;
using CloudColony.GameObjects;
using CloudColony.GameObjects.Entities;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace CloudColony.Logic
{
    public class World : IUpdate
    {
        public const float WORLD_WIDTH = 17.5f;
        public const float WORLD_HEIGHT = 10f;

        public List<Entity> Entities { get; private set; }
        public List<Entity> DeadEntities { get; private set; }

        public Player PlayerRed { get; private set; }
        public Player PlayerBlue { get; private set; }

        public World()
        {
            this.Entities = new List<Entity>();
            this.DeadEntities = new List<Entity>();
            InitPopulation(25);
        }

        private void InitPopulation(int each)
        {
            // Init red
            PlayerRed = new Player(CC.Pointer, PlayerIndex.One, WORLD_WIDTH - 1.5f, 7);
            for (int i = 0; i < each; i++)
            {
                Ship ship = new Ship(this, CC.Ship, PlayerRed, WORLD_WIDTH - ( i % WORLD_WIDTH), 7 + (int)(i / WORLD_WIDTH));
                Entities.Add(ship);
                PlayerRed.Ships.Add(ship);
            }

            // Init blue
            PlayerBlue = new Player(CC.Pointer, PlayerIndex.Two, 1.5f, 4);
            for (int i = 0; i < each; i++)
            {
                Ship ship = new Ship(this, CC.Ship, PlayerBlue, 1 + (i % WORLD_WIDTH), 4 - (int)(i / WORLD_WIDTH));
                Entities.Add(ship);
                PlayerBlue.Ships.Add(ship);
            }
        }

        public void Update(float delta)
        {
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
