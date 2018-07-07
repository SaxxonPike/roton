using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Behavior
{
    public sealed class BoulderBehavior : PuzzleBehavior
    {
        public override string KnownName => KnownNames.Boulder;

        public BoulderBehavior(ISounds sounds, ISounder sounder, IMover mover) : base(sounds, sounder, mover)
        {
        }
    }
}