using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class MonitorBehavior : ElementBehavior
    {
        private readonly IState _state;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Monitor;

        public MonitorBehavior(IState state, IMover mover)
        {
            _state = state;
            _mover = mover;
        }
        
        public override void Act(int index)
        {
            if (_state.KeyPressed != 0)
                _state.BreakGameLoop = true;
            
            _mover.MoveActorOnRiver(index);
        }
    }
}