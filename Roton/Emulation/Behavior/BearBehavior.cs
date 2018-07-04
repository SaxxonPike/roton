using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BearBehavior : EnemyBehavior
    {
        private readonly IActorList _actorList;
        private readonly IEngine _engine;
        private readonly IElementList _elementList;

        public override string KnownName => KnownNames.Bear;

        public BearBehavior(IActorList actorList, IEngine engine, IElementList elementList)
        {
            _actorList = actorList;
            _engine = engine;
            _elementList = elementList;
        }
        
        public override void Act(int index)
        {
            var actor = _actorList[index];
            var vector = new Vector();

            if (_actorList.GetPlayer().Location.X == actor.Location.X ||
                (8 - actor.P1 < _actorList.GetPlayer().Location.Y.AbsDiff(actor.Location.Y)))
            {
                if (8 - actor.P1 < _actorList.GetPlayer().Location.X.AbsDiff(actor.Location.X))
                {
                    vector.SetTo(0, 0);
                }
                else
                {
                    vector.SetTo(0, (_actorList.GetPlayer().Location.Y - actor.Location.Y).Polarity());
                }
            }
            else
            {
                vector.SetTo((_actorList.GetPlayer().Location.X - actor.Location.X).Polarity(), 0);
            }

            var target = actor.Location.Sum(vector);
            var targetElement = _engine.ElementAt(target);

            if (targetElement.IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else if (targetElement.Id == _elementList.PlayerId || targetElement.Id == _elementList.BreakableId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}