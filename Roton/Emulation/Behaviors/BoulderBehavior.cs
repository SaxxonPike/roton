using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class BoulderBehavior : PuzzleBehavior
    {
        public override string KnownName => KnownNames.Boulder;

        public BoulderBehavior(IEngine engine) : base(engine)
        {
        }
    }
}