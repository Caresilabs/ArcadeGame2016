using CloudColony.Framework;
using Microsoft.Xna.Framework;

namespace CloudColony.GameObjects.Targets
{
    public interface Target : IUpdate
    {
        Vector2 Position { get; }

        bool Done { get; }
    }
}
