using Roton.Emulation.Actions;
using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public class CounterclockwiseConveyorAction : IAction
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