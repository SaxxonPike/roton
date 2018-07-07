using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class DoorBehavior : ElementBehavior
    {
        private readonly ITiles _tiles;
        private readonly IAlerts _alerts;
        private readonly IWorld _world;
        private readonly ISounds _sounds;
        private readonly ISounder _sounder;
        private readonly IHud _hud;
        private readonly IMessager _messager;
        private readonly IMisc _misc;

        public override string KnownName => KnownNames.Door;

        public DoorBehavior(ITiles tiles, IAlerts alerts, IWorld world, ISounds sounds, ISounder sounder,
            IHud hud, IMessager messager, IMisc misc)
        {
            _tiles = tiles;
            _alerts = alerts;
            _world = world;
            _sounds = sounds;
            _sounder = sounder;
            _hud = hud;
            _messager = messager;
            _misc = misc;
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = (_tiles[location].Color & 0x70) >> 4;
            var keyIndex = color - 1;
            if (!_world.Keys[keyIndex])
            {
                _messager.SetMessage(0xC8, _alerts.DoorLockedMessage(color));
                _sounder.Play(3, _sounds.DoorLocked);
            }
            else
            {
                _world.Keys[keyIndex] = false;
                _misc.RemoveItem(location);
                _hud.UpdateStatus();
                _messager.SetMessage(0xC8, _alerts.DoorOpenMessage(color));
                _sounder.Play(3, _sounds.DoorOpen);
            }
        }
    }
}