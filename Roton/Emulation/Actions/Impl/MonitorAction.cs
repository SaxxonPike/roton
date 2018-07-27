using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [Context(Context.Original, 0x03)]
    [Context(Context.Super, 0x03)]
    public sealed class MonitorAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public MonitorAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            if (Engine.State.KeyPressed != 0)
                Engine.State.BreakGameLoop = true;
            
            Engine.MoveActorOnRiver(index);
        }
    }
}