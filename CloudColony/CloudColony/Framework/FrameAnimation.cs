using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CloudColony.Framework
{
    public class FrameAnimation : Animation
    {
        private Texture2D texture;
        private Point origin;
        private float stateTime;
        private int width;
        private int height;
        private int frames;
        private float frameDuration;

        public FrameAnimation(Texture2D tex, int x, int y, int width, int height, int frames, float frameDuration)
        {
            this.origin = new Point(x, y);
            this.width = width;
            this.height = height;
            this.stateTime = 0;
            this.frames = frames;
            this.frameDuration = frameDuration;
            this.texture = tex;
        }

        public override bool HasNext()
        {
            return lastFrame != currentFrame;
        }

        public override void Update(float delta)
        {
            lastFrame = currentFrame;
            stateTime += delta;
            currentFrame = (int)(stateTime / (float)frameDuration) % frames;
        }

        public override TextureRegion GetRegion()
        {
            return new TextureRegion(
                texture, origin.X + (width * currentFrame), origin.Y, width, height
           );
        }
    }
}
