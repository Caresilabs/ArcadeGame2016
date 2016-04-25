using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using CloudColony.GameObjects.Entities;
using CloudColony.GameObjects.Targets;
using CloudColony.Logic;
using CloudColony.Rendering;
using CloudColony.GameObjects.Powerups;

namespace CloudColony.GameObjects
{
    public class Player : Target, IRenderable, IUpdate
    {
        public const float PLAYER_SPEED = 10.5f;

        public const float STAMINA_GAIN = 25;
        public const float STAMINA_MAX = 100;

        public bool Done { get { return false; } }

        public Vector2 Position { get { return position; } }
        private Vector2 position;

        public List<Ship> Ships { get; private set; }

        public PlayerIndex Index { get; private set; }

        public float Stamina { get; set; }

        public World World { get; private set; }

        public Powerup ActivePowerup { get; set; }

        public bool ShieldOn { get; set; }

        private readonly Sprite pointer;
        private readonly StaminaProgressBar staminaBar;

        public Player(World world, TextureRegion pointer, PlayerIndex index, float x, float y)
        {
            this.ShieldOn = false;
            this.Ships = new List<Ship>();
            this.Index = index;
            this.World = world;
            this.pointer = new Sprite(pointer, x, y, 0.9f, 0.9f);
            this.pointer.ZIndex = 0.01f;

            this.staminaBar = new StaminaProgressBar(Index == PlayerIndex.One ? Color.Red : Color.Blue);

            this.position = new Vector2(x, y);
            this.Stamina = STAMINA_MAX;
        }

        public void Update(float delta)
        {
            pointer.SetPosition(position);

            Stamina = Math.Min(Stamina + STAMINA_GAIN * delta, STAMINA_MAX);

            staminaBar.Position = position;
            staminaBar.SetPercentage(Stamina / STAMINA_MAX);

            UpdateInput(delta);

            KeepInside();

            UpdatePowerup(delta);
        }

        private void UpdatePowerup(float delta)
        {
            if (ActivePowerup != null)
            {
                ActivePowerup.Update(delta);
                ActivePowerup.RunPower();

                if (ActivePowerup.Done)
                    ActivePowerup = null;
            }
        }

        private void UpdateInput(float delta)
        {
            // Movement
            if (ButtonDown(PlayerInput.Up))
                position.Y += delta * -PLAYER_SPEED;

            if (ButtonDown(PlayerInput.Down))
                position.Y += delta * PLAYER_SPEED;

            if (ButtonDown(PlayerInput.Left))
                position.X += delta * -PLAYER_SPEED;

            if (ButtonDown(PlayerInput.Right))
                position.X += delta * PLAYER_SPEED;


            // Dont allow on ready / gameover
            if (World.State == World.WorldState.RUNNING)
            {
                // Behaviors
                if (PressedButton(PlayerInput.Blue))
                {
                    if (TryDrainStamina(FlankTarget.COST))
                    {
                        ///Ships = Ships.OrderBy(x => x.Position.Y).ToList();
                        for (int i = 0; i < Ships.Count / 2; i++)
                        {
                            Ships[i].Target = new FlankTarget(Ships[i], this, -1);
                            Ships[i].Speed = Ships[i].MaxSpeed * 1.6f;
                        }
                        for (int i = Ships.Count / 2; i < Ships.Count; i++)
                        {
                            Ships[i].Target = new FlankTarget(Ships[i], this, 1);
                            Ships[i].Speed = Ships[i].MaxSpeed * 1.6f;
                        }
                    }
                }

                if (PressedButton(PlayerInput.Red))
                {
                    if (TryDrainStamina(ExplosionTarget.COST))
                    {
                        foreach (var ship in Ships)
                        {
                            ship.Target = new ExplosionTarget(this);
                            ship.Speed = ship.MaxSpeed * 1.7f;
                        }
                    }
                }

                // Attack def
                if (ButtonDown(PlayerInput.Yellow))
                {
                    foreach (var ship in Ships)
                    {
                        if (ship.CanShoot())
                        {
                            ship.ShieldHealth = 0;
                            if (TryDrainStamina(Bullet.COST * MathHelper.Lerp(1.2f, 0.3f, Ships.Count / (float)World.MAX_NUM_SHIPS))) //Stamina >= Bullet.COST *  Ships.Count)
                            {
                                ship.Shoot();
                            }
                        }
                    }
                }

                // Shield
                if (PressedButton(PlayerInput.Green))
                {
                    // Nasty hack... Dont replicate ^^
                    float oldStamina = Stamina;
                    {
                        foreach (var ship in Ships)
                        {
                            ship.ActivateShield();
                        }

                        if (Stamina <= 0)
                        {
                            foreach (var ship in Ships)
                            {
                                ship.ActivateShield();
                            }
                            Stamina = oldStamina;
                        }
                    }
                }
            }
        }

        public bool TryDrainStamina(float cost)
        {
            if (Stamina - cost < 0)
                return false;

            Stamina -= cost;

            return true; // Stamina >= 15;
        }

        public void DrainStamina(float cost)
        {
            Stamina = Math.Max(0,  Stamina - cost);
        }

        private bool ButtonDown(PlayerInput button)
        {
            return InputHandler.GetButtonState(Index, button) == InputState.Down;
        }

        private bool PressedButton(PlayerInput button)
        {
            return InputHandler.GetButtonState(Index, button) == InputState.Released;
        }

        public void Draw(SpriteBatch batch)
        {
            pointer.Draw(batch);
            staminaBar.Draw(batch);
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
