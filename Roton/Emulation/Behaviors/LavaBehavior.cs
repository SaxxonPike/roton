using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class LavaBehavior : WaterBehavior
    {
        public override string KnownName => KnownNames.Lava;

        public LavaBehavior(IEngine engine) : base(engine)
        {
        }
    }
}