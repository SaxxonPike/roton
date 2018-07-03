﻿using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class SharkBehavior : ElementBehavior
    {
        public override string KnownName => "Shark";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 > engine.SyncRandomNumber(10)
                ? engine.Seek(actor.Location)
                : engine.Rnd());

            var target = actor.Location.Sum(vector);
            var targetElement = engine.ElementAt(target);

            if (targetElement.Id == engine.Elements.WaterId || targetElement.Id == engine.Elements.LavaId)
            {
                engine.MoveActor(index, target);
            }
            else if (targetElement.Id == engine.Elements.PlayerId)
            {
                engine.Attack(index, target);
            }
        }
    }
}