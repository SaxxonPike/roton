using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x24)]
    [ContextEngine(ContextEngine.Super, 0x24)]
    public sealed class ObjectAction : IAction
    {
        private readonly IEngine _engine;

        public ObjectAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            if (actor.Instruction >= 0)
            {
                _engine.ExecuteCode(index, actor, @"Interaction");
            }
            if (actor.Vector.IsZero()) return;

            var target = actor.Location.Sum(actor.Vector);
            if (_engine.Tiles.ElementAt(target).IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else
            {
                _engine.BroadcastLabel(-index, KnownLabels.Thud, false);
            }
        }
    }
}