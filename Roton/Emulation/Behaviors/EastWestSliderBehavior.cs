using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class EastWestSliderBehavior : PuzzleBehavior
    {
        public override string KnownName => KnownNames.SliderEw;

        public EastWestSliderBehavior(IEngine engine) : base(engine)
        {
        }
    }
}