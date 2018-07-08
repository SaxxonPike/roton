using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class FakeWallBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Fake;

        public FakeWallBehavior(IEngine engine)
        {
            _engine = engine;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (!_engine.Alerts.FakeWall) return;

            _engine.Alerts.FakeWall = false;
            _engine.SetMessage(0xC8, _engine.Alerts.FakeMessage);
        }
    }
}