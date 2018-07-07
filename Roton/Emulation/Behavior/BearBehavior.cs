using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BearBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly ITiles _tiles;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Bear;

        public BearBehavior(IActors actors, IElements elements, ITiles tiles, IPlotter plotter, IMover mover) : base(mover)
        {
            _actors = actors;
            _elements = elements;
            _tiles = tiles;
            _mover = mover;
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
            var targetElement = _tiles.ElementAt(target);

            if (targetElement.IsFloor)
            {
                _mover.MoveActor(index, target);
            }
            else if (targetElement.Id == _elements.PlayerId || targetElement.Id == _elements.BreakableId)
            {
                _mover.Attack(index, target);
            }
        }
    }
}