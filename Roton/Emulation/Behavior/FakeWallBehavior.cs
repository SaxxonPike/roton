using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class FakeWallBehavior : ElementBehavior
    {
        public override string KnownName => "Fake Wall";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            if (!engine.Alerts.FakeWall) return;

            engine.Alerts.FakeWall = false;
            engine.SetMessage(0xC8, engine.Alerts.FakeMessage);
        }
    }
}
