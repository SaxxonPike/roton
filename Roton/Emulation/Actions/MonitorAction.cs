using Roton.Emulation.Core;

namespace Roton.Emulation.Actions
{
    public class MonitorAction : IAction
    {
        private readonly IEngine _engine;

        public MonitorAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            if (_engine.State.KeyPressed != 0)
                _engine.State.BreakGameLoop = true;
            
            _engine.MoveActorOnRiver(index);
        }
    }
}