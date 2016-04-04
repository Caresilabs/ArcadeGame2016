﻿using CloudColony.Framework;
using CloudColony.Logic;
using Microsoft.Xna.Framework;
using System;

namespace CloudColony.GameObjects.Entities
{
    public class Bullet : Entity
    {
        public const float DAMAGE = 8;

        public const float COST = 1;

        public Bullet(World world, Player owner, TextureRegion region, float x, float y, Vector2 direction)
            : base(world, owner, region, x, y, 0.22f, 0.22f)
        {
            velocity = direction * 8.5f;
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            CheckDead();
        }

        private void CheckDead()
        {
            if (position.X < Size.X / 2f || position.X > World.WORLD_WIDTH - Size.X / 2f || position.Y < Size.Y / 2f || position.Y > World.WORLD_HEIGHT - Size.Y / 2f)
            {
                IsDead = true;
            }
        }
    }
}
