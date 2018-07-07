using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class AmmoBehavior : ElementBehavior
    {
        private readonly IConfig _config;
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly ISounds _sounds;
        private readonly ISounder _sounder;
        private readonly IHud _hud;
        private readonly IMessenger _messenger;
        private readonly IMisc _misc;

        public AmmoBehavior(IConfig config, IWorld world, IAlerts alerts, ISounds sounds, ISounder sounder, IHud hud, IMessenger messenger, IMisc misc)
        {
            _config = config;
            _world = world;
            _alerts = alerts;
            _sounds = sounds;
            _sounder = sounder;
            _hud = hud;
            _messenger = messenger;
            _misc = misc;
        }

        public override string KnownName => KnownNames.Ammo;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _world.Ammo += _config.AmmoPerPickup;
            _misc.RemoveItem(location);
            _hud.UpdateStatus();
            _sounder.Play(2, _sounds.Ammo);
            if (_alerts.AmmoPickup)
            {
                _messenger.SetMessage(0xC8, _alerts.AmmoMessage);
                _alerts.AmmoPickup = false;
            }
        }
    }
}