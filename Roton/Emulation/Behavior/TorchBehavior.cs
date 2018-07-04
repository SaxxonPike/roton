using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class TorchBehavior : ElementBehavior
    {
        private readonly IWorld _world;
        private readonly IEngine _engine;
        private readonly IAlerts _alerts;
        private readonly ISounds _sounds;
        
        public override string KnownName => KnownNames.Torch;

        public TorchBehavior(IWorld world, IEngine engine, IAlerts alerts, ISounds sounds)
        {
            _world = world;
            _engine = engine;
            _alerts = alerts;
            _sounds = sounds;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _world.Torches++;
            _engine.RemoveItem(location);
            _engine.UpdateStatus();
            if (_alerts.TorchPickup)
            {
                _engine.SetMessage(0xC8, _alerts.TorchMessage);
                _alerts.TorchPickup = false;
            }
            _engine.PlaySound(3, _sounds.Torch);
        }
    }
}