using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal sealed class RotonBehavior : ElementBehavior
    {
        public override string KnownName => "Roton";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];

            actor.P3--;
            if (actor.P3 < -actor.P2%10)
            {
                actor.P3 = actor.P2*10 + engine.SyncRandomNumber(10);
            }

            actor.Vector.CopyFrom(engine.Seek(actor.Location));
            if (actor.P1 <= engine.SyncRandomNumber(10))
            {
                var temp = actor.Vector.X;
                actor.Vector.X = -actor.P2.Polarity()*actor.Vector.Y;
                actor.Vector.Y = actor.P2.Polarity()*temp;
            }

            var target = actor.Location.Sum(actor.Vector);
            if (engine.ElementAt(target).IsFloor)
            {
                engine.MoveActor(index, target);
            }
            else if (engine.TileAt(target).Id == engine.Elements.PlayerId)
            {
                engine.Attack(index, target);
            }
        }
    }
}