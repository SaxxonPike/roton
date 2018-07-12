using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Original, 0x1B)]
    [ContextEngine(ContextEngine.Super, 0x1B)]
    public sealed class FakeWallInteraction : IInteraction
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