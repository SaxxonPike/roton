using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class TorchBehavior : ElementBehavior
    {
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly ISounds _sounds;
        private readonly ISounder _sounder;
        private readonly IHud _hud;
        private readonly IMessenger _messenger;
        private readonly IMisc _misc;

        public override string KnownName => KnownNames.Torch;

        public TorchBehavior(IWorld world, IAlerts alerts, ISounds sounds, ISounder sounder, IHud hud, IMessenger messenger, IMisc misc)
        {
            _world = world;
            _alerts = alerts;
            _sounds = sounds;
            _sounder = sounder;
            _hud = hud;
            _messenger = messenger;
            _misc = misc;
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _world.Torches++;
            _misc.RemoveItem(location);
            _hud.UpdateStatus();
            if (_alerts.TorchPickup)
            {
                _messenger.SetMessage(0xC8, _alerts.TorchMessage);
                _alerts.TorchPickup = false;
            }

            _sounder.Play(3, _sounds.Torch);
        }
    }
}