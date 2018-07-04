using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public class LionBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        private readonly IRandom _random;
        private readonly IGrid _grid;
        private readonly IActors _actors;
        private readonly IElements _elements;

        public override string KnownName => KnownNames.Lion;

        public LionBehavior(IEngine engine, IRandom random, IGrid grid, IActors actors, IElements elements) : base(engine)
        {
            _engine = engine;
            _random = random;
            _grid = grid;
            _actors = actors;
            _elements = elements;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 >= _random.Synced(10)
                ? _engine.Seek(actor.Location)
                : _engine.Rnd());

            var target = actor.Location.Sum(vector);
            var element = _grid.ElementAt(target);
            if (element.IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else if (element.Id == _elements.PlayerId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}