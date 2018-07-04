using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public abstract class PuzzleBehavior : ElementBehavior
    {
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            engine.Push(location, vector);
            engine.PlaySound(2, engine.SoundSet.Push);
        }
    }
}