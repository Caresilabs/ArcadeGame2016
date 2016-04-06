using System.Collections.Generic;

namespace CloudColony.Framework
{
    public class Animations
    {
        private Dictionary<string, Animation> animations;
        private Animation currentAnimation;

        public Animations()
        {
            this.animations = new Dictionary<string, Animation>();
        }

        public void Update(float delta)
        {
            foreach (var animation in animations)
            {
                animation.Value.Update(delta);
            }
        }

        public TextureRegion GetRegion()
        {
            if (currentAnimation != null)
                return currentAnimation.GetRegion();
            else return null;
        }

        public void SetAnimation(string name)
        {
            currentAnimation = animations[name];
        }

        public void AddAnimation(string name, Animation animation)
        {
            animations[name] = animation;

            //if (animations.ContainsKey(name))
            //    animations.Remove(name);

            //animations.Add(name, animation);
        }

        public bool HasAnimations()
        {
            return animations.Count != 0;
        }

        public bool HasNext()
        {
            if (currentAnimation != null)
                return currentAnimation.HasNext();
            else return false;
        }
    }
}
