using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class AmmoBehavior : ElementBehavior
    {
        private readonly IConfig _config;
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly ISounds _sounds;
        private readonly IEngine _engine;

        public AmmoBehavior(IConfig config, IWorld world, IAlerts alerts, ISounds sounds, IEngine engine)
        {
            _config = config;
            _world = world;
            _alerts = alerts;
            _sounds = sounds;
            _engine = engine;
        }

        public override string KnownName => KnownNames.Ammo;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _world.Ammo += _config.AmmoPerPickup;
            _engine.RemoveItem(location);
            _engine.UpdateStatus();
            __engine.PlaySound(2, _sounds.Ammo);
            if (_alerts.AmmoPickup)
            {
                _engine.SetMessage(0xC8, _alerts.AmmoMessage);
                _alerts.AmmoPickup = false;
            }
        }
    }
}