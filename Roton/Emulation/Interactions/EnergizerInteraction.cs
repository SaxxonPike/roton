using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Original, 0x0E)]
    [ContextEngine(ContextEngine.Super, 0x0E)]
    public sealed class EnergizerInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public EnergizerInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
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