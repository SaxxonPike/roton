using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class PairerBehavior : EnemyBehavior
    {
        public override string KnownName => KnownNames.Pairer;

        public PairerBehavior(IEngine engine) : base(engine)
        {
        }
    }
}