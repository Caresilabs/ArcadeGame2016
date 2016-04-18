using CloudColony.Framework;
using CloudColony.Framework.Tools;

namespace CloudColony.Rendering
{

    public class SpriteFX : Sprite, IPoolable
    {
        public enum EffectType
        {
            HIT, DESTROY, POWERUP
        }

        public bool Done { get { return Animations.CurrentAnimation.GetPercent() == 1; } }

        public SpriteFX() : base(null, 0, 0, 0, 0)
        {
        }

        public void Reset()
        {
            Animations.Clear();
        }
    }
}
