using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Behavior
{
    public sealed class LavaBehavior : WaterBehavior
    {
        public override string KnownName => KnownNames.Lava;

        public LavaBehavior(ISounds sounds, IAlerts alerts, ISounder sounder, IMessenger messenger) : base(sounds, alerts, sounder, messenger)
        {
        }
    }
}