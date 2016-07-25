using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class RuffianBehavior : ElementBehavior
    {
        public override string KnownName => "Ruffian";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];

            if (actor.Vector.IsZero())
            {
                if (actor.P2 + 8 <= engine.RandomNumberDeterministic(17))
                {
                    if (actor.P1 >= engine.RandomNumberDeterministic(9))
                    {
                        actor.Vector.CopyFrom(engine.Seek(actor.Location));
                    }
                    else
                    {
                        actor.Vector.CopyFrom(engine.Rnd());
                    }
                }
            }
            else
            {
                if (actor.Location.X == engine.Player.Location.X || actor.Location.Y == engine.Player.Location.Y)
                {
                    if (actor.P1 >= engine.RandomNumberDeterministic(9))
                    {
                        actor.Vector.CopyFrom(engine.Seek(actor.Location));
                    }
                }

                var target = actor.Location.Sum(actor.Vector);
                if (engine.ElementAt(target).Id == engine.Elements.PlayerId)
                {
                    engine.Attack(index, target);
                }
                else if (engine.ElementAt(target).IsFloor)
                {
                    engine.MoveActor(index, target);
                    if (actor.P2 + 8 <= engine.RandomNumberDeterministic(17))
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
