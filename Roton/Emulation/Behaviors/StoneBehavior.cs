using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Behaviors
{
    public sealed class StoneBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Stone;

        public StoneBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            _engine.UpdateBoard(_engine.Actors[index].Location);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0x41 + _engine.Random.NonSynced(0x1A), _engine.Tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
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