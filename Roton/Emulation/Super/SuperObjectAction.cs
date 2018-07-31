using System;
using Roton.Emulation.Actions;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super, 0x24)]
    public class SuperObjectAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public SuperObjectAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            if (actor.P2 == 0 && actor.Instruction >= 0)
            {
                Engine.ExecuteCode(index, actor, @"Interaction");
            }

            if (actor.Vector.IsZero())
            {
                if (actor.P2 > 0)
                    actor.P2--;
                return;
            }

            var target = actor.Location.Sum(actor.Vector);
            
            if (!Engine.Tiles.ElementAt(target).IsFloor)
                Engine.Push(target, actor.Vector);

            if (Engine.Tiles.ElementAt(target).IsFloor)
            {
                Engine.MoveActor(index, target);
                if (actor.P2 > 0)
                {
                    actor.P2--;
                    if (actor.P2 == 0)
                        actor.Vector.SetTo(0, 0);
                }
            }
            else
            {
                Engine.BroadcastLabel(-index, KnownLabels.Thud, false);
            }
        }        
    }
}