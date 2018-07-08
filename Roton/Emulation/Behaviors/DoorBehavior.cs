using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Behaviors
{
    public sealed class DoorBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Door;

        public DoorBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = (_engine.Tiles[location].Color & 0x70) >> 4;
            var keyIndex = color - 1;
            if (!_engine.World.Keys[keyIndex])
            {
                _engine.SetMessage(0xC8, _engine.Alerts.DoorLockedMessage(color));
                _engine.PlaySound(3, _engine.Sounds.DoorLocked);
            }
            else
            {
                _engine.World.Keys[keyIndex] = false;
                _engine.RemoveItem(location);
                _engine.Hud.UpdateStatus();
                _engine.SetMessage(0xC8, _engine.Alerts.DoorOpenMessage(color));
                _engine.PlaySound(3, _engine.Sounds.DoorOpen);
            }
        }
    }
}