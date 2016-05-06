using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

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
        public static Texture2D TransitionTexture { get; private set; }
        public static Texture2D ParallaxTexture { get; private set; }

        public static TextureRegion Pixel { get; private set; }

        public static TextureRegion PointerBlue { get; private set; }
        public static TextureRegion BulletBlue { get; private set; }
        public static TextureRegion ShieldBlue { get; private set; }

        public static TextureRegion PointerRed { get; private set; }
        public static TextureRegion BulletRed { get; private set; }
        public static TextureRegion ShieldRed { get; private set; }

        public static TextureRegion RedPowerup { get; private set; }
        public static TextureRegion BluePowerup { get; private set; }
        public static TextureRegion GreenPowerup { get; private set; }

        // UI
        public static TextureRegion ScoreBoard { get; private set; }
        public static TextureRegion Frame { get; private set; }
        public static TextureRegion GameBackground { get; private set; }
        public static TextureRegion MenuBackground { get; private set; }

        public static TextureRegion Button1 { get; private set; }
        public static TextureRegion Button2 { get; private set; }

        public static TextureRegion Logo { get; private set; }

        public static TextureRegion Simon { get; private set; }
        public static TextureRegion Sebastian { get; private set; }

        public static TextureRegion WinBlue { get; private set; }

        public static TextureRegion WinRed { get; private set; }


        // Fonts
        public static SpriteFont Font { get; private set; }
        public static TextureRegion CloudBackground { get; private set; }

        // Sound
        public static SoundEffect HitSound { get; private set; }
        public static SoundEffect PlayGameSound { get; private set; }
        public static SoundEffect PowerUpSound { get; private set; }
        public static SoundEffect ExlosionSound { get; private set; }
        public static SoundEffect ShieldHitSound { get; private set; }
        public static SoundEffect WinSound { get; private set; }
        public static SoundEffect ShootSound { get; private set; }

        public static void Load(ContentManager content)
        {
            LoadAssets(content);
        }

        private static void LoadAssets(ContentManager content)
        {
            Atlas = content.Load<Texture2D>("FirstEditionAtlas");

            PointerBlue = new TextureRegion(Atlas, 256 + 20 + 32, 0, 32, 32);
            BulletBlue = new TextureRegion(Atlas, 16, 64, 16, 16);
            ShieldBlue = new TextureRegion(Atlas, 64, 64, 32, 32);

            PointerRed = new TextureRegion(Atlas, 256 + 20, 0, 32, 32);
            BulletRed = new TextureRegion(Atlas, 0, 64, 16, 16);
            ShieldRed = new TextureRegion(Atlas, 32, 64, 32, 32);

            RedPowerup = new TextureRegion(Atlas, 68 + 32, 0, 16, 16);
            BluePowerup = new TextureRegion(Atlas, 68 + 16, 0, 16, 16);
            GreenPowerup = new TextureRegion(Atlas, 68, 0, 16, 16);

            // Parallax
            ParallaxTexture = content.Load<Texture2D>("SecondEditionAtlas");

            // Transition
            TransitionTexture = content.Load<Texture2D>("GateClosingWithoutFrame");

            // UI
            Logo = new TextureRegion(Atlas, 132, 154, 358, 63);

            GameBackground = new TextureRegion(Atlas, 0, 386, 160, 90);
            CloudBackground = new TextureRegion(Atlas, 230, 386, 160, 90);
            MenuBackground = new TextureRegion(Atlas, 322, 218, 160, 90);

            Button1 = new TextureRegion(Atlas, 438, 64, 64, 32);
            Button2 = new TextureRegion(Atlas, 0, 154, 64, 32);

            Simon = new TextureRegion(Atlas, 0, 218, 153, 168);
            Sebastian = new TextureRegion(Atlas, 153, 218, 169, 127);

            WinBlue = new TextureRegion(Atlas, 96, 64, 96, 64);
            WinRed = new TextureRegion(Atlas, 192, 64, 86, 58);

            Frame = new TextureRegion(Atlas, 278, 64, 160, 90);

            Pixel = new TextureRegion(Atlas, 422, 23, 1, 1);

            Font = content.Load<SpriteFont>("Font");


            // Sound
            SoundEffect.MasterVolume = 1.0f;

            MediaPlayer.Volume = 0.0f;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(content.Load<Song>("Sound/JuhaniJunkalaEpicBossBattle"));

            HitSound = content.Load<SoundEffect>("Sound/Hit");
            PlayGameSound = content.Load<SoundEffect>("Sound/StartGame");
            PowerUpSound = content.Load<SoundEffect>("Sound/PowerUp");
            ExlosionSound = content.Load<SoundEffect>("Sound/Explosion");
            ShieldHitSound = content.Load<SoundEffect>("Sound/ShieldHit");
            ShootSound = content.Load<SoundEffect>("Sound/Shoot");
            WinSound = content.Load<SoundEffect>("Sound/Jingle_Win_00"); 
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


        public static bool AnyKeyJustClicked(PlayerIndex index)
        {
            bool pressed = false;
            for (int i = 1; i < Enum.GetValues(typeof(PlayerInput)).Length; i++)
            {
                if (InputHandler.GetButtonState(index, (PlayerInput)i) == InputState.Pressed)
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
