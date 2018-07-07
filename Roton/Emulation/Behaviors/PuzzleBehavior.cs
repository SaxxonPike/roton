using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public abstract class PuzzleBehavior : ElementBehavior
    {
        private readonly ISounds _sounds;
        private readonly ISounder _sounder;
        private readonly IMover _mover;

        public PuzzleBehavior(ISounds sounds, ISounder sounder, IMover mover)
        {
            _sounds = sounds;
            _sounder = sounder;
            _mover = mover;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _mover.Push(location, vector);
            _sounder.Play(2, _sounds.Push);
        }
    }
}