using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using CloudColony.GameObjects.Entities;
using CloudColony.GameObjects.Targets;

namespace CloudColony.GameObjects
{
    public class Player : Target, IRenderable, IUpdate
    {
        public const float PLAYER_SPEED = 5;

        public const float STAMINA_GAIN = 20;
        public const float STAMINA_MAX = 100;

        public bool Done { get { return false; } }

        public Vector2 Position { get { return position; } }
        private Vector2 position;

        public List<Ship> Ships { get; private set; }

        public PlayerIndex Index { get; private set; }

        public float Stamina { get; private set; }

        private readonly Sprite pointer;

        public Player(TextureRegion pointer, PlayerIndex index, float x, float y)
        {
            this.Ships = new List<Ship>();
            this.Index = index;
            this.pointer = new Sprite(pointer, x, y, 0.2f, 0.2f);
            this.position = new Vector2(x, y);
            this.Stamina = STAMINA_MAX;
        }

        public void Update(float delta)
        {
            pointer.SetPosition(position);

            Stamina += STAMINA_GAIN;

            UpdateInput(delta);
        }

        private void UpdateInput(float delta)
        {
            if (PressedButton(PlayerInput.Up))
                position.Y += delta * -PLAYER_SPEED;

            if (PressedButton(PlayerInput.Down))
                position.Y += delta * PLAYER_SPEED;

            if (PressedButton(PlayerInput.Left))
                position.X += delta * -PLAYER_SPEED;

            if (PressedButton(PlayerInput.Right))
                position.X += delta * PLAYER_SPEED;

            // Movements
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
    }
}
