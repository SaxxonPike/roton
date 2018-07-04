using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class RotonBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        private readonly IActors _actors;
        private readonly IRandom _random;
        private readonly IGrid _grid;
        private readonly IElements _elements;

        public override string KnownName => KnownNames.Roton;

        public RotonBehavior(IEngine engine, IActors actors, IRandom random, IGrid grid, IElements elements) : base(engine)
        {
            _engine = engine;
            _actors = actors;
            _random = random;
            _grid = grid;
            _elements = elements;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];

            actor.P3--;
            if (actor.P3 < -actor.P2%10)
            {
                actor.P3 = actor.P2*10 + _random.Synced(10);
            }

            actor.Vector.CopyFrom(_engine.Seek(actor.Location));
            if (actor.P1 <= _random.Synced(10))
            {
                var temp = actor.Vector.X;
                actor.Vector.X = -actor.P2.Polarity()*actor.Vector.Y;
                actor.Vector.Y = actor.P2.Polarity()*temp;
            }

            var target = actor.Location.Sum(actor.Vector);
            if (_grid.ElementAt(target).IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else if (_grid.TileAt(target).Id == _elements.PlayerId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}