using System;
using CloudColony.Framework;
using Microsoft.Xna.Framework.Content;

namespace CloudColony
{
    public class CC
    {
        // Globals

        public const int VIEWPORT_WIDTH = 1920;
        public const int VIEWPORT_HEIGHT = 10280;

        public static TextureRegion Ship { get; set; }

        public static void Load(ContentManager content)
        {
            LoadAssets(content);
        }

        private static void LoadAssets(ContentManager content)
        {

        }
    }
}
