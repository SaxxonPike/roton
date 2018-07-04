using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class ObjectBehavior : ElementBehavior
    {
        private readonly IConfig _config;
        private readonly IActors _actors;
        private readonly IEngine _engine;
        private readonly IGrid _grid;

        public ObjectBehavior(IConfig config, IActors actors, IEngine engine, IGrid grid)
        {
            _config = config;
            _actors = actors;
            _engine = engine;
            _grid = grid;
        }

        public override string KnownName => KnownNames.Object;

        public override void Act(int index)
        {
            var actor = _actors[index];
            if (actor.Instruction >= 0)
            {
                _engine.ExecuteCode(index, actor, @"Interaction");
            }
            if (actor.Vector.IsZero()) return;

            var target = actor.Location.Sum(actor.Vector);
            if (_grid.ElementAt(target).IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else
            {
                _engine.BroadcastLabel(-index, @"THUD", false);
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_actors.ActorAt(location).P1, _grid[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var objectIndex = _actors.ActorIndexAt(location);
            _engine.BroadcastLabel(-objectIndex, KnownLabels.Touch, false);
        }
    }
}