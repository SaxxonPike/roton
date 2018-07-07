using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class ObjectBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IEngine _engine;
        private readonly ITiles _tiles;
        private readonly IBroadcaster _broadcaster;
        private readonly IMover _mover;

        public ObjectBehavior(IActors actors, IEngine engine, ITiles tiles, IBroadcaster broadcaster, IMover mover)
        {
            _actors = actors;
            _engine = engine;
            _tiles = tiles;
            _broadcaster = broadcaster;
            _mover = mover;
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
            if (_tiles.ElementAt(target).IsFloor)
            {
                _mover.MoveActor(index, target);
            }
            else
            {
                _broadcaster.BroadcastLabel(-index, KnownLabels.Thud, false);
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_actors.ActorAt(location).P1, _tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var objectIndex = _actors.ActorIndexAt(location);
            _broadcaster.BroadcastLabel(-objectIndex, KnownLabels.Touch, false);
        }
    }
}