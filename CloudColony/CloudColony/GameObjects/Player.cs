using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using CloudColony.GameObjects.Entities;

namespace CloudColony.GameObjects
{
    public class Player : IRenderable, IUpdate
    {
        public const float PLAYER_SPEED = 2;

        public List<Ship> Ships { get; private set; }

        public Vector2 Position { get { return position;} }
        private Vector2 position;

        private readonly Sprite pointer;
        private readonly PlayerIndex index;

        public Player(TextureRegion pointer, PlayerIndex index)
        {
            this.Ships = new List<Ship>();
            this.index = index;
            this.pointer = new Sprite(pointer,0,0, 16,16);
        }

        public void Update(float delta)
        {
            if (InputHandler.GetButtonState(index, PlayerInput.Up) == InputState.Down) {
                position.X += delta * PLAYER_SPEED;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            pointer.Draw(batch);
        }
    }
}
