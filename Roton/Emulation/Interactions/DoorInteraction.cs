using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Zzt, 0x09)]
    [ContextEngine(ContextEngine.SuperZzt, 0x09)]
    public sealed class DoorInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public DoorInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
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