using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x10)]
    [ContextEngine(ContextEngine.Super, 0x10)]
    public sealed class ClockwiseConveyorAction : IAction
    {
        private readonly IEngine _engine;

        public ClockwiseConveyorAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            _engine.UpdateBoard(actor.Location);
            _engine.Convey(actor.Location, 1);
        }
    }
}