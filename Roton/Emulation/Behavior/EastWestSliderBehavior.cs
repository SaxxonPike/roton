using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class EastWestSliderBehavior : PuzzleBehavior
    {
        public override string KnownName => KnownNames.SliderEw;

        public EastWestSliderBehavior(IEngine engine, ISounds sounds) : base(engine, sounds)
        {
        }
    }
}