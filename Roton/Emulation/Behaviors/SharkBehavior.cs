using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class SharkBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IRandom _random;
        private readonly ITiles _tiles;
        private readonly IElements _elements;
        private readonly ICompass _compass;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Shark;

        public SharkBehavior(IActors actors, IRandom random, ITiles tiles, IElements elements, ICompass compass, IMover mover)
        {
            _actors = actors;
            _random = random;
            _tiles = tiles;
            _elements = elements;
            _compass = compass;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 > _random.Synced(10)
                ? _compass.Seek(actor.Location)
                : _compass.Rnd());

            var target = actor.Location.Sum(vector);
            var targetElement = _tiles.ElementAt(target);

            if (targetElement.Id == _elements.WaterId || targetElement.Id == _elements.LavaId)
            {
                _mover.MoveActor(index, target);
            }
            else if (targetElement.Id == _elements.PlayerId)
            {
                _mover.Attack(index, target);
            }
        }
    }
}