using Roton.Emulation.Actions;
using Roton.Emulation.Core;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super, 0x03)]
public sealed class SuperMonitorAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        if (Engine.State.KeyPressed is EngineKeyCode.Enter or EngineKeyCode.Escape)
            Engine.State.BreakGameLoop = true;
            
        Engine.MoveActorOnRiver(index);
    }
}