using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CloudColony
{
    public class CC
    {
        // Globals

        public const int VIEWPORT_WIDTH = 1920;
        public const int VIEWPORT_HEIGHT = 1080;

        // Game
        public static TextureRegion Ship { get; private set; }
        public static TextureRegion Pointer { get; private set; }

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
            var tex = content.Load<Texture2D>("PreviewData/GamePreviewInfoImage");
            Ship = new TextureRegion(tex, 0, 0, 16, 16);

            Pointer = new TextureRegion(tex, 24, 24, 16, 16);
        }
    }
}
