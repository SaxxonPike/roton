using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class FakeWallBehavior : ElementBehavior
    {
        private readonly IAlerts _alerts;
        private readonly IMessenger _messenger;

        public override string KnownName => KnownNames.Fake;

        public FakeWallBehavior(IAlerts alerts, IMessenger messenger)
        {
            _alerts = alerts;
            _messenger = messenger;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (!_alerts.FakeWall) return;

            _alerts.FakeWall = false;
            _messenger.SetMessage(0xC8, _alerts.FakeMessage);
        }
    }
}