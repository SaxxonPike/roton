using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [Context(Context.Original, 0x22)]
    [Context(Context.Super, 0x22)]
    public sealed class BearAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public BearAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            var vector = new Vector();

            if (Engine.Player.Location.X == actor.Location.X ||
                8 - actor.P1 < Engine.Player.Location.Y.AbsDiff(actor.Location.Y))
            {
                if (8 - actor.P1 < Engine.Player.Location.X.AbsDiff(actor.Location.X))
                {
                    vector.SetTo(0, 0);
                }
                else
                {
                    vector.SetTo(0, (Engine.Player.Location.Y - actor.Location.Y).Polarity());
                }
            }
            else
            {
                vector.SetTo((Engine.Player.Location.X - actor.Location.X).Polarity(), 0);
            }

            var target = actor.Location.Sum(vector);
            var targetElement = Engine.Tiles.ElementAt(target);

            if (targetElement.IsFloor)
            {
                Engine.MoveActor(index, target);
            }
            else if (targetElement.Id == Engine.ElementList.PlayerId || targetElement.Id == Engine.ElementList.BreakableId)
            {
                Engine.Attack(index, target);
            }
        }
    }
}