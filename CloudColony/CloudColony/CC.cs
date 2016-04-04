using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CloudColony
{
	
	// http://store.steampowered.com/app/232790/ <3
	
    public class CC
    {
        // Globals
        public const int VIEWPORT_WIDTH = 1920;
        public const int VIEWPORT_HEIGHT = 1080;

        // Game
        public static Texture2D Atlas { get; private set; }

        public static TextureRegion Pixel { get; private set; }

        public static TextureRegion ShipBlue { get; private set; }
        public static TextureRegion PointerBlue { get; private set; }
        public static TextureRegion BulletBlue { get; private set; }
        public static TextureRegion ShieldBlue { get; private set; }

        public static TextureRegion ShipRed { get; private set; }
        public static TextureRegion PointerRed { get; private set; }
        public static TextureRegion BulletRed { get; private set; }
        public static TextureRegion ShieldRed { get; private set; }

        // UI
        public static TextureRegion ScoreBoard { get; private set; }

        public static TextureRegion WinBlue { get; private set; }

        public static TextureRegion WinRed { get; private set; }

        // Fonts
        public static SpriteFont Font { get; private set; }

        public static void Load(ContentManager content)
        {
            LoadAssets(content);
        }

        private static void LoadAssets(ContentManager content)
        {
            Atlas = content.Load<Texture2D>("FirstEditionAtlas");


            ShipBlue = new TextureRegion(Atlas, 32, 4, 32, 32);
            PointerBlue = new TextureRegion(Atlas, 256 + 20 + 32, 0, 32, 32);
            BulletBlue = new TextureRegion(Atlas, 16, 64, 16, 16);
            ShieldBlue = new TextureRegion(Atlas, 64, 64, 32, 32);

            ShipRed = new TextureRegion(Atlas, 4, 0, 32, 32);
            PointerRed = new TextureRegion(Atlas, 256 + 20, 0, 32, 32); // todo correct x
            BulletRed = new TextureRegion(Atlas, 0, 64, 16, 16);
            ShieldRed = new TextureRegion(Atlas, 32, 64, 32, 32);


            // UI
            WinBlue = new TextureRegion(Atlas, 96, 58, 96, 64);

            WinRed = new TextureRegion(Atlas, 192, 64, 86, 58);

            Pixel = new TextureRegion(Atlas, 422, 23, 1, 1);


            Font = content.Load<SpriteFont>("Font");
        }

        public static bool AnyKeyPressed(PlayerIndex index)
        {
            bool pressed = false;
            for (int i = 1; i < Enum.GetValues(typeof(PlayerInput)).Length; i++)
            {
                if (InputHandler.GetButtonState(index, (PlayerInput)i) == InputState.Released)
                    pressed = true;
            }

            return pressed;
        }

        public static bool AnyKeyPressedNoJoystick(PlayerIndex index)
        {
            bool pressed = false;
            for (int i = 6; i < Enum.GetValues(typeof(PlayerInput)).Length; i++)
            {
                if (InputHandler.GetButtonState(index, (PlayerInput)i) == InputState.Released)
                    pressed = true;
            }

            return pressed;
        }
    }
}
