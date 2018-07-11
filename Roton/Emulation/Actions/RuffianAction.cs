using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Zzt, 0x23)]
    [ContextEngine(ContextEngine.SuperZzt, 0x23)]
    public sealed class RuffianAction : IAction
    {
        private readonly IEngine _engine;
        
        public RuffianAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
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
                if (_engine.Tiles.ElementAt(target).Id == _engine.ElementList.PlayerId)
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