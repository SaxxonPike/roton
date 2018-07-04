using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class EnergizerBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        private readonly ISounds _sounds;
        private readonly IAlerts _alerts;
        private readonly IWorld _world;

        public override string KnownName => KnownNames.Energizer;

        public EnergizerBehavior(IEngine engine, ISounds sounds, IAlerts alerts, IWorld world)
        {
            _engine = engine;
            _sounds = sounds;
            _alerts = alerts;
            _world = world;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            __engine.PlaySound(9, _sounds.Energizer);
            _engine.RemoveItem(location);
            _world.EnergyCycles = 0x4B;
            _engine.UpdateStatus();
            _engine.UpdateBoard(location);
            if (_alerts.EnergizerPickup)
            {
                _alerts.EnergizerPickup = false;
                _engine.SetMessage(0xC8, _alerts.EnergizerMessage);
            }
            _engine.BroadcastLabel(0, KnownLabels.Energize, false);
        }
    }
}