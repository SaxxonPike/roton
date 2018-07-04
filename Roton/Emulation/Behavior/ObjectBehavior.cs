using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class ObjectBehavior : ElementBehavior
    {
        private readonly bool _extendedMovement;

        public ObjectBehavior(bool extendedMovement)
        {
            _extendedMovement = extendedMovement;
        }

        public override string KnownName => "Object";

        public override void Act(int index)
        {
            var actor = _actorList[index];
            if (actor.Instruction >= 0)
            {
                engine.ExecuteCode(index, actor, @"Interaction");
            }
            if (actor.Vector.IsZero()) return;

            var target = actor.Location.Sum(actor.Vector);
            if (engine.ElementAt(target).IsFloor)
            {
                engine.MoveActor(index, target);
            }
            else
            {
                engine.BroadcastLabel(-index, @"THUD", false);
            }
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            return new AnsiChar(engine.ActorAt(location).P1, engine.Tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var objectIndex = engine.ActorIndexAt(location);
            engine.BroadcastLabel(-objectIndex, @"TOUCH", false);
        }
    }
}