using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Super, 0x40)]
    public sealed class StoneInteraction : IInteraction
    {
        private readonly IEngine _engine;
        
        public StoneInteraction(IEngine engine)
        {
            _engine = engine;
        }

        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (_engine.World.Stones < 0)
                _engine.World.Stones = 0;

            _engine.World.Stones++;
            _engine.Destroy(location);
            _engine.Hud.UpdateStatus();
            _engine.SetMessage(0xC8, _engine.Alerts.StoneMessage);
        }
    }
}