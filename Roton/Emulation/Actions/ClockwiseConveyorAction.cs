using Roton.Emulation.Core;

namespace Roton.Emulation.Actions
{
    public class ClockwiseConveyorAction : IAction
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