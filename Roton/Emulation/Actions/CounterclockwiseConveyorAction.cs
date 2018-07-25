using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x11)]
    [ContextEngine(ContextEngine.Super, 0x11)]
    public sealed class CounterclockwiseConveyorAction : IAction
    {
        private readonly IEngine _engine;

        public CounterclockwiseConveyorAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            _engine.UpdateBoard(actor.Location);
            _engine.Convey(actor.Location, -1);
        }
    }
}