using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [Context(Context.Original, 0x0D)]
    [Context(Context.Super, 0x0D)]
    public sealed class BombAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public BombAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            if (actor.P1 <= 0) return;

            actor.P1--;
            Engine.UpdateBoard(actor.Location);
            switch (actor.P1)
            {
                case 1:
                    Engine.PlaySound(1, Engine.Sounds.BombExplode);
                    Engine.UpdateRadius(actor.Location, RadiusMode.Explode);
                    break;
                case 0:
                    var location = actor.Location.Clone();
                    Engine.RemoveActor(index);
                    Engine.UpdateRadius(location, RadiusMode.Clear);
                    break;
                default:
                    Engine.PlaySound(1, (actor.P1 & 0x01) == 0 ? Engine.Sounds.BombTock : Engine.Sounds.BombTick);
                    break;
            }
        }
    }
}