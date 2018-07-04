using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class SharkBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IRandom _random;
        private readonly IEngine _engine;
        private readonly IGrid _grid;
        private readonly IElements _elements;

        public override string KnownName => KnownNames.Shark;

        public SharkBehavior(IActors actors, IRandom random, IEngine engine, IGrid grid, IElements elements)
        {
            _actors = actors;
            _random = random;
            _engine = engine;
            _grid = grid;
            _elements = elements;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 > _random.Synced(10)
                ? _engine.Seek(actor.Location)
                : _engine.Rnd());

            var target = actor.Location.Sum(vector);
            var targetElement = _grid.ElementAt(target);

            if (targetElement.Id == _elements.WaterId || targetElement.Id == _elements.LavaId)
            {
                _engine.MoveActor(index, target);
            }
            else if (targetElement.Id == _elements.PlayerId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}