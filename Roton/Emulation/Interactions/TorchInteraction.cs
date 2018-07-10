using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Interactions
{
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