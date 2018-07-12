using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x03)]
    [ContextEngine(ContextEngine.Super, 0x03)]
    public sealed class MonitorAction : IAction
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