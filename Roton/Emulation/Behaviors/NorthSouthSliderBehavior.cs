using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class NorthSouthSliderBehavior : PuzzleBehavior
    {
        public override string KnownName => KnownNames.SliderNs;

        public NorthSouthSliderBehavior(IEngine engine) : base(engine)
        {
        }
    }
}