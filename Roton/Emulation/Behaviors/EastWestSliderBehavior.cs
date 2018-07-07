using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Behaviors
{
    public sealed class EastWestSliderBehavior : PuzzleBehavior
    {
        public override string KnownName => KnownNames.SliderEw;

        public EastWestSliderBehavior(ISounds sounds, ISounder sounder, IMover mover) : base(sounds, sounder, mover)
        {
        }
    }
}