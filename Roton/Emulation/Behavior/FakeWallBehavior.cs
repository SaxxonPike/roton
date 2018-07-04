using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class FakeWallBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        private readonly IAlerts _alerts;
        
        public override string KnownName => KnownNames.Fake;

        public FakeWallBehavior(IEngine engine, IAlerts alerts)
        {
            _engine = engine;
            _alerts = alerts;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (!_alerts.FakeWall) return;

            _alerts.FakeWall = false;
            _engine.SetMessage(0xC8, _alerts.FakeMessage);
        }
    }
}