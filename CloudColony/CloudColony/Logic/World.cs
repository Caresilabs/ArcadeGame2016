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

        public Player PlayerRed { get; private set; }
        public Player PlayerBlue { get; private set; }

        public World()
        {
            this.Entities = new List<Entity>();
            InitPopulation(25);
        }

        private void InitPopulation(int each)
        {
            // Init red
            PlayerRed = new Player(null, PlayerIndex.One);
            for (int i = 0; i < each; i++)
            {
                Ship ship = new Ship(this, null, PlayerRed, 9 - i, 7);
                Entities.Add(ship);
                PlayerRed.Ships.Add(ship);
            }

            // Init blue
            PlayerBlue = new Player(null, PlayerIndex.Two);
            for (int i = 0; i < each; i++)
            {
                Ship ship = new Ship(this, null, PlayerBlue, 1 + i, 4);
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
            }
        }
    }
}
