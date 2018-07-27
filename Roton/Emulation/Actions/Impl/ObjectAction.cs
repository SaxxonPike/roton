using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [Context(Context.Original, 0x24)]
    [Context(Context.Super, 0x24)]
    public sealed class ObjectAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public ObjectAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            if (actor.Instruction >= 0)
            {
                Engine.ExecuteCode(index, actor, @"Interaction");
            }
            if (actor.Vector.IsZero()) return;

            var target = actor.Location.Sum(actor.Vector);
            if (Engine.Tiles.ElementAt(target).IsFloor)
            {
                Engine.MoveActor(index, target);
            }
            else
            {
                Engine.BroadcastLabel(-index, KnownLabels.Thud, false);
            }
        }
    }
}