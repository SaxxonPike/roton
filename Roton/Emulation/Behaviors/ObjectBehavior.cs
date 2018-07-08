using Roton.Core;
using Roton.Emulation.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class ObjectBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        public ObjectBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override string KnownName => KnownNames.Object;

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            if (actor.Instruction >= 0)
            {
                _engine.ExecuteCode(index, actor, @"Interaction");
            }
            if (actor.Vector.IsZero()) return;

            var target = actor.Location.Sum(actor.Vector);
            if (_engine.Tiles.ElementAt(target).IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else
            {
                _engine.BroadcastLabel(-index, KnownLabels.Thud, false);
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(_engine.ActorAt(location).P1, _engine.Tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var objectIndex = _engine.ActorIndexAt(location);
            _engine.BroadcastLabel(-objectIndex, KnownLabels.Touch, false);
        }
    }
}