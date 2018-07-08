using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Behaviors
{
    public sealed class EnergizerBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Energizer;

        public EnergizerBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.PlaySound(9, _engine.Sounds.Energizer);
            _engine.RemoveItem(location);
            _engine.World.EnergyCycles = 0x4B;
            _engine.Hud.UpdateStatus();
            _engine.UpdateBoard(location);
            if (_engine.Alerts.EnergizerPickup)
            {
                _engine.Alerts.EnergizerPickup = false;
                _engine.SetMessage(0xC8, _engine.Alerts.EnergizerMessage);
            }

            _engine.BroadcastLabel(0, KnownLabels.Energize, false);
        }
    }
}