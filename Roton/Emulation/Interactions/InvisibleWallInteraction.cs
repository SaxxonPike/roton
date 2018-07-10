using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Interactions
{
    public class InvisibleWallInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public InvisibleWallInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Tiles[location].Id = _engine.ElementList.NormalId;
            _engine.UpdateBoard(location);
            _engine.PlaySound(3, _engine.Sounds.Invisible);
            _engine.SetMessage(0x64, _engine.Alerts.InvisibleMessage);
        }
    }
}