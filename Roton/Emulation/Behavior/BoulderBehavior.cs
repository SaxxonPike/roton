using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class BoulderBehavior : PuzzleBehavior
    {
        public override string KnownName => KnownNames.Boulder;

        public BoulderBehavior(IEngine engine, ISounds sounds) : base(engine, sounds)
        {
        }
    }
}