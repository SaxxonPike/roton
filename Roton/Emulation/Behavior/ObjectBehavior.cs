using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class ObjectBehavior : ElementBehavior
    {
        private readonly bool _extendedMovement;

        public ObjectBehavior(bool extendedMovement)
        {
            _extendedMovement = extendedMovement;
        }

        public override string KnownName => "Object";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
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

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            var objectIndex = engine.ActorIndexAt(location);
            engine.BroadcastLabel(-objectIndex, @"TOUCH", false);
        }
    }
}
