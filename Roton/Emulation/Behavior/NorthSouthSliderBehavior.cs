using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Behavior
{
    public sealed class NorthSouthSliderBehavior : PuzzleBehavior
    {
        public override string KnownName => KnownNames.SliderNs;

        public NorthSouthSliderBehavior(ISounds sounds, ISounder sounder, IMover mover) : base(sounds, sounder, mover)
        {
        }
    }
}