using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Zzt, 0x06)]
    public sealed class TorchInteraction : IInteraction
    {
        private readonly IEngine _engine;
        
        public TorchInteraction(IEngine engine)
        {
            _engine = engine;
        }

        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.World.Torches++;
            _engine.RemoveItem(location);
            _engine.Hud.UpdateStatus();
            if (_engine.Alerts.TorchPickup)
            {
                _engine.SetMessage(0xC8, _engine.Alerts.TorchMessage);
                _engine.Alerts.TorchPickup = false;
            }

            _engine.PlaySound(3, _engine.Sounds.Torch);
        }
    }
}