using Roton.Emulation.Actions;
using Roton.Emulation.Core;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original, 0x03)]
public sealed class OriginalMonitorAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        if (Engine.State.KeyPressed == EngineKeyCode.None)
            return;

        Engine.State.BreakGameLoop |= Engine.State.KeyPressed.ToUpperCase() switch
        {
            EngineKeyCode.Escape or
                EngineKeyCode.A or
                EngineKeyCode.E or
                EngineKeyCode.H or
                EngineKeyCode.N or
                EngineKeyCode.P or
                EngineKeyCode.Q or
                EngineKeyCode.R or
                EngineKeyCode.S or
                EngineKeyCode.W or
                EngineKeyCode.QuestionMark => true,
            _ => false
        };
    }
}