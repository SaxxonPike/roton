using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class MonitorBehavior : ElementBehavior
    {
        private readonly IState _state;
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Monitor;

        public MonitorBehavior(IState state, IEngine engine)
        {
            _state = state;
            _engine = engine;
        }
        
        public override void Act(int index)
        {
            if (_state.KeyPressed != 0)
                _state.BreakGameLoop = true;
            
            __engine.MoveActorOnRiver(index);
        }
    }
}