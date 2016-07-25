using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class ScrollBehavior : ElementBehavior
    {
        public override string KnownName => "Scroll";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var color = engine.Tiles[actor.Location].Color;

            color++;
            if (color > 0x0F)
            {
                color = 0x09;
            }
            engine.Tiles[actor.Location].Color = color;
            engine.UpdateBoard(actor.Location);
        }

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            var scrollIndex = engine.ActorIndexAt(location);
            var actor = engine.Actors[scrollIndex];

            engine.PlaySound(2, engine.EncodeMusic(@"c-c+d-d+e-e+f-f+g-g"));
            engine.ExecuteCode(scrollIndex, actor, @"Scroll");
            engine.RemoveActor(scrollIndex);
        }
    }
}
