using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class InvisibleWallBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        public override string KnownName => KnownNames.Invisible;

        public InvisibleWallBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Tiles[location].Id = _engine.Elements.NormalId;
            _engine.UpdateBoard(location);
            _engine.PlaySound(3, _engine.Sounds.Invisible);
            _engine.SetMessage(0x64, _engine.Alerts.InvisibleMessage);
        }
    }
}