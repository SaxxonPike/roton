using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public class WaterBehavior : ElementBehavior
    {
        private readonly ISounds _sounds;
        private readonly IAlerts _alerts;
        private readonly ISounder _sounder;
        private readonly IMessager _messager;
        public override string KnownName => KnownNames.Water;

        public WaterBehavior(ISounds sounds, IAlerts alerts, ISounder sounder, IMessager messager)
        {
            _sounds = sounds;
            _alerts = alerts;
            _sounder = sounder;
            _messager = messager;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _sounder.Play(3, _sounds.Water);
            _messager.SetMessage(0x64, _alerts.WaterMessage);
        }
    }
}