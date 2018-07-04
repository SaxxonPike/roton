using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class StoneBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        private readonly IActors _actors;
        private readonly IGrid _grid;
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly IRandom _random;

        public override string KnownName => KnownNames.Stone;

        public StoneBehavior(IEngine engine, IActors actors, IGrid grid, IWorld world, IAlerts alerts, IRandom random)
        {
            _engine = engine;
            _actors = actors;
            _grid = grid;
            _world = world;
            _alerts = alerts;
            _random = random;
        }
        
        public override void Act(int index)
        {
            _engine.UpdateBoard(_actors[index].Location);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0x41 + _random.NonSynced(0x1A), _grid[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (_world.Stones < 0)
                _world.Stones = 0;
            
            _world.Stones++;
            _engine.Destroy(location);
            _engine.UpdateStatus();
            _engine.SetMessage(0xC8, _alerts.StoneMessage);
        }
    }
}