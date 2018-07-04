using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public abstract class PuzzleBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        private readonly ISounds _sounds;

        public PuzzleBehavior(IEngine engine, ISounds sounds)
        {
            _engine = engine;
            _sounds = sounds;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Push(location, vector);
            _engine.PlaySound(2, _sounds.Push);
        }
    }
}