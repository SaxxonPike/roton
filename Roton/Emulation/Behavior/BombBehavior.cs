using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class BombBehavior : ElementBehavior
    {
        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            if (actor.P1 <= 0) return;

            actor.P1--;
            engine.UpdateBoard(actor.Location);
            switch (actor.P1)
            {
                case 1:
                    engine.PlaySound(1, engine.Sounds.BombExplode);
                    engine.UpdateRadius(actor.Location, RadiusMode.Explode);
                    break;
                case 0:
                    var location = actor.Location.Clone();
                    engine.RemoveActor(index);
                    engine.UpdateRadius(location, RadiusMode.Clear);
                    break;
                default:
                    engine.PlaySound(1, (actor.P1 & 0x01) == 0 ? engine.Sounds.BombTock : engine.Sounds.BombTick);
                    break;
            }
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            var p1 = engine.Actors[engine.ActorIndexAt(location)].P1;
            return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, engine.Tiles[location].Color);
        }

        public override string KnownName => "Bomb";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            var actor = engine.ActorAt(location);
            if (actor.P1 == 0)
            {
                actor.P1 = 9;
                engine.UpdateBoard(location);
                engine.SetMessage(0xC8, engine.Alerts.BombMessage);
                engine.PlaySound(4, engine.Sounds.BombActivate);
            }
            else
            {
                engine.Push(location, vector);
            }
        }
    }
}
