using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class KeyBehavior : ElementBehavior
    {
        private readonly IGrid _grid;
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly ISounds _sounds;
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Key;

        public KeyBehavior(IGrid grid, IWorld world, IAlerts alerts, ISounds sounds, IEngine engine)
        {
            _grid = grid;
            _world = world;
            _alerts = alerts;
            _sounds = sounds;
            _engine = engine;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = _grid[location].Color & 0x07;
            var keyIndex = color - 1;
            if (_world.Keys[keyIndex])
            {
                _engine.SetMessage(0xC8, _alerts.KeyAlreadyMessage(color));
                __engine.PlaySound(2, _sounds.KeyAlready);
            }
            else
            {
                _world.Keys[keyIndex] = true;
                _engine.RemoveItem(location);
                _engine.UpdateStatus();
                _engine.SetMessage(0xC8, _alerts.KeyPickupMessage(color));
                __engine.PlaySound(2, _sounds.Key);
            }
        }
    }
}