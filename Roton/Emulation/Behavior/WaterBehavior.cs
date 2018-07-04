using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public class WaterBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        private readonly ISounds _sounds;
        private readonly IAlerts _alerts;
        public override string KnownName => KnownNames.Water;

        public WaterBehavior(IEngine engine, ISounds sounds, IAlerts alerts)
        {
            _engine = engine;
            _sounds = sounds;
            _alerts = alerts;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.PlaySound(3, _sounds.Water);
            _engine.SetMessage(0x64, _alerts.WaterMessage);
        }
    }
}