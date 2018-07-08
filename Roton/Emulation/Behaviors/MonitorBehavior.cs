using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class MonitorBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        public override string KnownName => KnownNames.Monitor;

        public MonitorBehavior(IEngine engine)
        {
            _engine = engine;
        }
        
        public override void Act(int index)
        {
            if (_engine.State.KeyPressed != 0)
                _engine.State.BreakGameLoop = true;
            
            _engine.MoveActorOnRiver(index);
        }
    }
}