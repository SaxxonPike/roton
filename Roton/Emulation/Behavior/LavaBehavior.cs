using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class LavaBehavior : WaterBehavior
    {
        public override string KnownName => KnownNames.Lava;

        public LavaBehavior(IEngine engine, ISounds sounds, IAlerts alerts) : base(engine, sounds, alerts)
        {
        }
    }
}