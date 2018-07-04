using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public class LionBehavior : EnemyBehavior
    {
        public override string KnownName => "Lion";

        public override void Act(int index)
        {
            var actor = _actorList[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 >= engine.SyncRandomNumber(10)
                ? engine.Seek(actor.Location)
                : engine.Rnd());

            var target = actor.Location.Sum(vector);
            var element = engine.ElementAt(target);
            if (element.IsFloor)
            {
                engine.MoveActor(index, target);
            }
            else if (element.Id == engine.Elements.PlayerId)
            {
                engine.Attack(index, target);
            }
        }
    }
}