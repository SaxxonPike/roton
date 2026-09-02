using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ConfirmInputHandler(
    IGameThread gameThread,
    IInputReader inputReader,
    IState state,
    IScheduler scheduler)
    : IConfirmInputHandler

{
    public bool Confirm()
    {
        while (gameThread.ThreadActive)
        {
            scheduler.WaitForTick();
            inputReader.Read(true);
            switch (state.KeyPressed.ToUpperCase())
            {
                case EngineKeyCode.Y:
                    return true;
                case EngineKeyCode.N:
                case EngineKeyCode.Escape:
                    return false;
            }
        }

        return true;
    }
}