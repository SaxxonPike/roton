using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Zzt, 0x05)]
    [ContextEngine(ContextEngine.SuperZzt, 0x05)]
    public sealed class AmmoInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public AmmoInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.World.Ammo += _engine.Config.AmmoPerPickup;
            _engine.RemoveItem(location);
            _engine.UpdateStatus();
            _engine.PlaySound(2, _engine.Sounds.Ammo);
            if (_engine.Alerts.AmmoPickup)
            {
                _engine.SetMessage(0xC8, _engine.Alerts.AmmoMessage);
                _engine.Alerts.AmmoPickup = false;
            }
        }
    }
}