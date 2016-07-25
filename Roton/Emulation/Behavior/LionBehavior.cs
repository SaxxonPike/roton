﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class LionBehavior : ElementBehavior
    {
        public override string KnownName => "Lion";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var vector = new Vector();
            if (actor.P1 >= engine.RandomNumberDeterministic(10))
            {
                vector.CopyFrom(engine.Seek(actor.Location));
            }
            else
            {
                vector.CopyFrom(engine.Rnd());
            }
            var target = actor.Location.Sum(vector);
            var element = engine.ElementAt(target);
            if (element.IsFloor)
            {
                engine.MoveActor(index, target);
            }
            else if (element.Id == engine.Elements.PlayerId)
            {
                engine.Attack(index, target);
            }
        }
    }
}