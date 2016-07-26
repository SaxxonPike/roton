using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal abstract class PuzzleBehavior : ElementBehavior
    {
        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.Push(location, vector);
            engine.PlaySound(2, engine.SoundSet.Push);
        }
    }
}
