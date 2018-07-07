using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public class LionBehavior : EnemyBehavior
    {
        private readonly IRandom _random;
        private readonly ITiles _tiles;
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly ICompass _compass;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Lion;

        public LionBehavior(IRandom random, ITiles tiles, IActors actors, IElements elements,
            ICompass compass, IMover mover) : base(mover)
        {
            _random = random;
            _tiles = tiles;
            _actors = actors;
            _elements = elements;
            _compass = compass;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 >= _random.Synced(10)
                ? _compass.Seek(actor.Location)
                : _compass.Rnd());

            var target = actor.Location.Sum(vector);
            var element = _tiles.ElementAt(target);
            if (element.IsFloor)
            {
                _mover.MoveActor(index, target);
            }
            else if (element.Id == _elements.PlayerId)
            {
                _mover.Attack(index, target);
            }
        }
    }
}