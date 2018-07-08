using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class RuffianBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Ruffian;

        public RuffianBehavior(IEngine engine) : base(engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];

            if (actor.Vector.IsZero())
            {
                if (actor.P2 + 8 <= _engine.Random.Synced(17))
                {
                    if (actor.P1 >= _engine.Random.Synced(9))
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
                if (actor.Location.X == _engine.Player.Location.X || actor.Location.Y == _engine.Player.Location.Y)
                {
                    if (actor.P1 >= _engine.Random.Synced(9))
                    {
                        actor.Vector.CopyFrom(_engine.Seek(actor.Location));
                    }
                }

                var target = actor.Location.Sum(actor.Vector);
                if (_engine.Tiles.ElementAt(target).Id == _engine.Elements.PlayerId)
                {
                    _engine.Attack(index, target);
                }
                else if (_engine.ElementAt(target).IsFloor)
                {
                    _engine.MoveActor(index, target);
                    if (actor.P2 + 8 <= _engine.Random.Synced(17))
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