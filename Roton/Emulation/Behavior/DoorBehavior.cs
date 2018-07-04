using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class DoorBehavior : ElementBehavior
    {
        private readonly IGrid _grid;
        private readonly IEngine _engine;
        private readonly IAlerts _alerts;
        private readonly IWorld _world;
        private readonly ISounds _sounds;
        
        public override string KnownName => KnownNames.Door;

        public DoorBehavior(IGrid grid, IEngine engine, IAlerts alerts, IWorld world, ISounds sounds)
        {
            _grid = grid;
            _engine = engine;
            _alerts = alerts;
            _world = world;
            _sounds = sounds;
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = (_grid.TileAt(location).Color & 0x70) >> 4;
            var keyIndex = color - 1;
            if (!_world.Keys[keyIndex])
            {
                _engine.SetMessage(0xC8, _alerts.DoorLockedMessage(color));
                __engine.PlaySound(3, _sounds.DoorLocked);
            }
            else
            {
                _world.Keys[keyIndex] = false;
                _engine.RemoveItem(location);
                _engine.UpdateStatus();
                _engine.SetMessage(0xC8, _alerts.DoorOpenMessage(color));
                __engine.PlaySound(3, _sounds.DoorOpen);
            }
        }
    }
}