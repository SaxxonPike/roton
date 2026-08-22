using Roton.Emulation.Actions;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original, 0x03)]
public sealed class OriginalMonitorAction(
    IEngineAccessor engine,
    IState state)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        if (state.KeyPressed == EngineKeyCode.None)
            return;

        state.BreakGameLoop |= state.KeyPressed.ToUpperCase() switch
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