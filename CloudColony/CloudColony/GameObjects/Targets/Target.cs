using Microsoft.Xna.Framework;

namespace CloudColony.GameObjects.Targets
{
    public interface Target
    {
        Vector2 Position { get; }

        bool Done { get; }
    }
}
