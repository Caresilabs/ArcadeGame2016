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

        public static TextureRegion ShipBlue { get; private set; }
        public static TextureRegion PointerBlue { get; private set; }
        public static TextureRegion BulletBlue { get; private set; }

        public static TextureRegion ShipRed { get; private set; }
        public static TextureRegion PointerRed { get; private set; }
        public static TextureRegion BulletRed { get; private set; }

        // UI
        public static TextureRegion ScoreBoard { get; private set; }

        // Fonts
        public static SpriteFont Font { get; private set; }

        public static void Load(ContentManager content)
        {
            LoadAssets(content);
        }

        private static void LoadAssets(ContentManager content)
        {
            Atlas = content.Load<Texture2D>("FirstEditionAtlas");


            ShipBlue = new TextureRegion(Atlas, 32, 2, 32, 32);
            PointerBlue = new TextureRegion(Atlas, 256 + 20 + 32, 0, 32, 32);
            BulletBlue = new TextureRegion(Atlas, 0, 0, 2, 2);

            ShipRed = new TextureRegion(Atlas, 0, 2, 32, 32);
            PointerRed = new TextureRegion(Atlas, 256 + 20, 0, 32, 32); // todo correct x
            BulletRed = new TextureRegion(Atlas, 2, 0, 2, 2);


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
    }
}
