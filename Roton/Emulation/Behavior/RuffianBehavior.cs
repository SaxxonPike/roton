using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class RuffianBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IEngine _engine;
        private readonly IRandom _random;
        private readonly IGrid _grid;
        private readonly IElements _elements;

        public override string KnownName => KnownNames.Ruffian;

        public RuffianBehavior(IActors actors, IEngine engine, IRandom random, IGrid grid, IElements elements) : base(engine)
        {
            _actors = actors;
            _engine = engine;
            _random = random;
            _grid = grid;
            _elements = elements;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];

            if (actor.Vector.IsZero())
            {
                if (actor.P2 + 8 <= _random.Synced(17))
                {
                    if (actor.P1 >= _random.Synced(9))
                    {
                        actor.Vector.CopyFrom(_engine.Seek(actor.Location));
                    }
                    else
                    {
                        actor.Vector.CopyFrom(_engine.Rnd());
                    }
                }
            }
            else
            {
                if (actor.Location.X == _actors.Player.Location.X || actor.Location.Y == _actors.Player.Location.Y)
                {
                    if (actor.P1 >= _random.Synced(9))
                    {
                        actor.Vector.CopyFrom(_engine.Seek(actor.Location));
                    }
                }

                var target = actor.Location.Sum(actor.Vector);
                if (_grid.ElementAt(target).Id == _elements.PlayerId)
                {
                    _engine.Attack(index, target);
                }
                else if (_grid.ElementAt(target).IsFloor)
                {
                    _engine.MoveActor(index, target);
                    if (actor.P2 + 8 <= _random.Synced(17))
                    {
                        actor.Vector.SetTo(0, 0);
                    }
                }
                else
                {
                    actor.Vector.SetTo(0, 0);
                }
            }
        }
    }
}