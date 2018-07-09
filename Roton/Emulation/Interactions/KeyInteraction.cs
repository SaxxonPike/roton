using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Interactions;

namespace Roton.Emulation.Behaviors
{
    public class KeyInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public KeyInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = _engine.Tiles[location].Color & 0x07;
            var keyIndex = color - 1;
            if (_engine.World.Keys[keyIndex])
            {
                _engine.SetMessage(0xC8, _engine.Alerts.KeyAlreadyMessage(color));
                _engine.PlaySound(2, _engine.Sounds.KeyAlready);
            }
            else
            {
                _engine.World.Keys[keyIndex] = true;
                _engine.RemoveItem(location);
                _engine.Hud.UpdateStatus();
                _engine.SetMessage(0xC8, _engine.Alerts.KeyPickupMessage(color));
                _engine.PlaySound(2, _engine.Sounds.Key);
            }
        }
    }
}