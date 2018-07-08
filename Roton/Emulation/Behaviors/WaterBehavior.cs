using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public class WaterBehavior : ElementBehavior
    {
        private readonly ISounds _sounds;
        private readonly IAlerts _alerts;
        private readonly ISounder _sounder;
        private readonly IMessenger _messenger;
        public override string KnownName => KnownNames.Water;

        public WaterBehavior(ISounds sounds, IAlerts alerts, ISounder sounder, IMessenger messenger)
        {
            _sounds = sounds;
            _alerts = alerts;
            _sounder = sounder;
            _messenger = messenger;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.PlaySound(3, _sounds.Water);
            _messenger.SetMessage(0x64, _alerts.WaterMessage);
        }
    }
}