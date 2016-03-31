using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using CloudColony.GameObjects.Entities;
using CloudColony.GameObjects.Targets;
using CloudColony.Logic;

namespace CloudColony.GameObjects
{
    public class Player : Target, IRenderable, IUpdate
    {
        public const float PLAYER_SPEED = 5;

        public const float STAMINA_GAIN = 25;
        public const float STAMINA_MAX = 100;

        public bool Done { get { return false; } }

        public Vector2 Position { get { return position; } }
        private Vector2 position;

        public List<Ship> Ships { get; private set; }

        public PlayerIndex Index { get; private set; }

        public float Stamina { get; private set; }

        public World World { get; private set; }

        private readonly Sprite pointer;

        public Player(World world, TextureRegion pointer, PlayerIndex index, float x, float y)
        {
            this.Ships = new List<Ship>();
            this.Index = index;
            this.World = world;
            this.pointer = new Sprite(pointer, x, y, 0.7f, 0.7f);
            this.position = new Vector2(x, y);
            this.Stamina = STAMINA_MAX;
        }

        public void Update(float delta)
        {
            pointer.SetPosition(position);

            Stamina = Math.Min(Stamina + STAMINA_GAIN * delta, STAMINA_MAX);

            UpdateInput(delta);

            KeepInside();
        }

        private void UpdateInput(float delta)
        {
            // Movement
            if (PressedButton(PlayerInput.Up))
                position.Y += delta * -PLAYER_SPEED;

            if (PressedButton(PlayerInput.Down))
                position.Y += delta * PLAYER_SPEED;

            if (PressedButton(PlayerInput.Left))
                position.X += delta * -PLAYER_SPEED;

            if (PressedButton(PlayerInput.Right))
                position.X += delta * PLAYER_SPEED;

            // Dont allow on ready / gameover
            if (World.State == World.WorldState.RUNNING)
            {
                // Behaviors
                if (PressedButton(PlayerInput.Blue))
                {
                    if (TryDrainStamina(FlankTarget.COST))
                    {
                        for (int i = 0; i < Ships.Count / 2; i++)
                        {
                            Ships[i].Target = new FlankTarget(Ships[i], this, -1);
                        }
                        for (int i = Ships.Count / 2; i < Ships.Count; i++)
                        {
                            Ships[i].Target = new FlankTarget(Ships[i], this, 1);
                        }
                    }
                }

                // Attack def
                if (PressedButton(PlayerInput.Red))
                {
                    foreach (var ship in Ships)
                    {
                        if (ship.CanShoot() && TryDrainStamina(Bullet.COST))
                        {
                            ship.Shoot();
                        }
                    }
                }
            }
        }

        private bool TryDrainStamina(float cost)
        {
            if (Stamina - cost < 0)
                return false;

            Stamina -= cost;

            return true;
        }

        private bool PressedButton(PlayerInput button)
        {
            return InputHandler.GetButtonState(Index, button) == InputState.Down;
        }

        public void Draw(SpriteBatch batch)
        {
            pointer.Draw(batch);
        }

        private void KeepInside()
        {
            if (position.X < pointer.Size.X / 2f)
            {
                position.X = pointer.Size.X / 2f;
            }

            if (position.X > World.WORLD_WIDTH - pointer.Size.X / 2f)
            {
                position.X = World.WORLD_WIDTH - pointer.Size.X / 2f;
            }

            if (position.Y < pointer.Size.Y / 2f)
            {
                position.Y = pointer.Size.Y / 2f;
            }

            if (position.Y > World.WORLD_HEIGHT - pointer.Size.Y / 2f)
            {
                position.Y = World.WORLD_HEIGHT - pointer.Size.Y / 2f;
            }
        }
    }
}
