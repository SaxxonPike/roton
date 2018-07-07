using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class FakeWallBehavior : ElementBehavior
    {
        private readonly IAlerts _alerts;
        private readonly IMessager _messager;

        public override string KnownName => KnownNames.Fake;

        public FakeWallBehavior(IAlerts alerts, IMessager messager)
        {
            _alerts = alerts;
            _messager = messager;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (!_alerts.FakeWall) return;

            _alerts.FakeWall = false;
            _messager.SetMessage(0xC8, _alerts.FakeMessage);
        }
    }
}