using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [ContextEngine(ContextEngine.Original, 0x23)]
    [ContextEngine(ContextEngine.Super, 0x23)]
    public sealed class RuffianAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public RuffianAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = Engine.Actors[index];

            if (actor.Vector.IsZero())
            {
                if (actor.P2 + 8 <= Engine.Random.GetNext(17))
                {
                    if (actor.P1 >= Engine.Random.GetNext(9))
                    {
                        actor.Vector.CopyFrom(Engine.Seek(actor.Location));
                    }
                    else
                    {
                        actor.Vector.CopyFrom(Engine.Rnd());
                    }
                }
            }
            else
            {
                if (actor.Location.X == Engine.Player.Location.X || actor.Location.Y == Engine.Player.Location.Y)
                {
                    if (actor.P1 >= Engine.Random.GetNext(9))
                    {
                        actor.Vector.CopyFrom(Engine.Seek(actor.Location));
                    }
                }

                var target = actor.Location.Sum(actor.Vector);
                if (Engine.Tiles.ElementAt(target).Id == Engine.ElementList.PlayerId)
                {
                    Engine.Attack(index, target);
                }
                else if (Engine.ElementAt(target).IsFloor)
                {
                    Engine.MoveActor(index, target);
                    if (actor.P2 + 8 <= Engine.Random.GetNext(17))
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