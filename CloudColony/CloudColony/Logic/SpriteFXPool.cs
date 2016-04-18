using CloudColony.Framework.Tools;
using CloudColony.Rendering;

namespace CloudColony.Logic
{
    public class SpriteFXPool : Pool<SpriteFX>
    {
        public override SpriteFX newObject()
        {
            return new SpriteFX();
        }
    }
}
