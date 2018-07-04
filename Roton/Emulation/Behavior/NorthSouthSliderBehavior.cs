using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class NorthSouthSliderBehavior : PuzzleBehavior
    {
        public override string KnownName => KnownNames.SliderNs;

        public NorthSouthSliderBehavior(IEngine engine, ISounds sounds) : base(engine, sounds)
        {
        }
    }
}