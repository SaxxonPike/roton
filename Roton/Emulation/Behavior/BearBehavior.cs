using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BearBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IEngine _engine;
        private readonly IElements _elements;
        private readonly IGrid _grid;

        public override string KnownName => KnownNames.Bear;

        public BearBehavior(IActors actors, IEngine engine, IElements elements, IGrid grid) : base(engine)
        {
            _actors = actors;
            _engine = engine;
            _elements = elements;
            _grid = grid;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            var vector = new Vector();

            if (_actors.Player.Location.X == actor.Location.X ||
                (8 - actor.P1 < _actors.Player.Location.Y.AbsDiff(actor.Location.Y)))
            {
                if (8 - actor.P1 < _actors.Player.Location.X.AbsDiff(actor.Location.X))
                {
                    vector.SetTo(0, 0);
                }
                else
                {
                    vector.SetTo(0, (_actors.Player.Location.Y - actor.Location.Y).Polarity());
                }
            }
            else
            {
                vector.SetTo((_actors.Player.Location.X - actor.Location.X).Polarity(), 0);
            }

            var target = actor.Location.Sum(vector);
            var targetElement = _grid.ElementAt(target);

            if (targetElement.IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else if (targetElement.Id == _elements.PlayerId || targetElement.Id == _elements.BreakableId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}