using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class PairerBehavior : EnemyBehavior
    {
        public override string KnownName => KnownNames.Pairer;

        public PairerBehavior(IEngine engine) : base(engine)
        {
        }
    }
}