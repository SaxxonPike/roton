using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal sealed class BearBehavior : EnemyBehavior
    {
        public override string KnownName => "Bear";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var vector = new Vector();

            if (engine.Player.Location.X == actor.Location.X ||
                (8 - actor.P1 < engine.Player.Location.Y.AbsDiff(actor.Location.Y)))
            {
                if (8 - actor.P1 < engine.Player.Location.X.AbsDiff(actor.Location.X))
                {
                    vector.SetTo(0, 0);
                }
                else
                {
                    vector.SetTo(0, (engine.Player.Location.Y - actor.Location.Y).Polarity());
                }
            }
            else
            {
                vector.SetTo((engine.Player.Location.X - actor.Location.X).Polarity(), 0);
            }

            var target = actor.Location.Sum(vector);
            var targetElement = engine.ElementAt(target);

            if (targetElement.IsFloor)
            {
                engine.MoveActor(index, target);
            }
            else if (targetElement.Id == engine.Elements.PlayerId || targetElement.Id == engine.Elements.BreakableId)
            {
                engine.Attack(index, target);
            }
        }
    }
}