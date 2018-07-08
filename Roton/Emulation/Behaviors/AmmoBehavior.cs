using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Behaviors
{
    public sealed class AmmoBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        public AmmoBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override string KnownName => KnownNames.Ammo;

        public override void Interact(IXyPair location, int index, IXyPair vector)
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