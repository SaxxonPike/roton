using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Interactions;

namespace Roton.Emulation.Behaviors
{
    public class FakeWallInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public FakeWallInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (!_engine.Alerts.FakeWall) return;

            _engine.Alerts.FakeWall = false;
            _engine.SetMessage(0xC8, _engine.Alerts.FakeMessage);
        }
    }
}