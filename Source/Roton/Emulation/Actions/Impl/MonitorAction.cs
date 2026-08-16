using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x03)]
[Context(Context.Super, 0x03)]
public sealed class MonitorAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        if (Engine.State.KeyPressed != 0)
            Engine.State.BreakGameLoop = true;
            
        Engine.MoveActorOnRiver(index);
    }
}