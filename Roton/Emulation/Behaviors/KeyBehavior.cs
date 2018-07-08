using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class KeyBehavior : ElementBehavior
    {
        private readonly ITiles _tiles;
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly ISounds _sounds;
        private readonly ISounder _sounder;
        private readonly IHud _hud;
        private readonly IMessenger _messenger;
        private readonly IMisc _misc;

        public override string KnownName => KnownNames.Key;

        public KeyBehavior(ITiles tiles, IWorld world, IAlerts alerts, ISounds sounds, ISounder sounder,
            IHud hud, IMessenger messenger, IMisc misc)
        {
            _tiles = tiles;
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
            var color = _tiles[location].Color & 0x07;
            var keyIndex = color - 1;
            if (_world.Keys[keyIndex])
            {
                _messenger.SetMessage(0xC8, _alerts.KeyAlreadyMessage(color));
                _engine.PlaySound(2, _sounds.KeyAlready);
            }
            else
            {
                _world.Keys[keyIndex] = true;
                _misc.RemoveItem(location);
                _hud.UpdateStatus();
                _messenger.SetMessage(0xC8, _alerts.KeyPickupMessage(color));
                _engine.PlaySound(2, _sounds.Key);
            }
        }
    }
}