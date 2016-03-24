namespace CloudColony.Framework
{
    public abstract class Animation
    {
        protected int currentFrame;
        protected int lastFrame = -1;

        public abstract void Update(float delta);

        public abstract TextureRegion GetRegion();

        public abstract bool HasNext();
    }
}
