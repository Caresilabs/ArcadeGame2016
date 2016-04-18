using CloudColony.Framework;
using CloudColony.Framework.Tools;

namespace CloudColony.Rendering
{

    public class SpriteFX : Sprite, IPoolable
    {
        public enum EffectType
        {
            HIT, DESTROY, POWERUP, SHOOT_RED, SHOOT_BLUE
        }

        public bool Done { get; set; }

        public SpriteFX() : base(null, 0, 0, 0, 0)
        {
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            Done = Animations.CurrentAnimation.GetPercent() == 1;
        }

        public void Reset()
        {
            Animations.Clear();
        }
    }
}
