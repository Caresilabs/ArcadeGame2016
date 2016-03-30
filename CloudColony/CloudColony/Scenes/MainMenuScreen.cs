using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CloudColony.Scenes
{
    public class MainMenuScreen : Screen
    {
        public bool PlayerRedReady { get; private set; }
        public bool PlayerBlueReady { get; private set; }

        public override void Init()
        {
            this.PlayerRedReady = false;
            this.PlayerBlueReady = false;
        }

        public override void Update(float delta)
        {
            InputHandler.IsButtonDown(Microsoft.Xna.Framework.PlayerIndex.One, PlayerInput.Side);

            for (int i = 1; i < Enum.GetValues(typeof(PlayerInput)).Length; i++)
            {
                if (InputHandler.GetButtonState(Microsoft.Xna.Framework.PlayerIndex.One, (PlayerInput)i) == InputState.Released)
                    PlayerRedReady = true;

                if (InputHandler.GetButtonState(Microsoft.Xna.Framework.PlayerIndex.Two, (PlayerInput)i) == InputState.Released)
                    PlayerBlueReady = true;
            }

            if (PlayerRedReady && PlayerBlueReady)
            {
                SetScreen(new GameScreen());
            }
            
        }

        public override void Draw(SpriteBatch batch)
        {

        }
      
        public override void Dispose()
        {
        }
    }
}
