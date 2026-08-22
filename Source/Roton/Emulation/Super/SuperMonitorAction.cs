using Roton.Emulation.Actions;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super, 0x03)]
public sealed class SuperMonitorAction(
    IEngineAccessor engine,
    IState state)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        // Ordinarily, the game code will only check for Enter and Escape here.
        // However, since we still don't have the proper "title card" implementation
        // just yet, we will have to accept those inputs here.

        state.BreakGameLoop |= state.KeyPressed.ToUpperCase() switch
        {
            EngineKeyCode.Escape or
                EngineKeyCode.Enter or
                EngineKeyCode.Q or
                EngineKeyCode.R or
                EngineKeyCode.W or
                EngineKeyCode.QuestionMark => true,
            _ => false
        };

        Engine.MoveActorOnRiver(index);
    }
}