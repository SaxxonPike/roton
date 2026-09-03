using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Engine(
    IClock clock,
    IGame game,
    IGameThread gameThread)
    : IEngine, IDisposable
{
    public void StepOnce()
    {
        gameThread.Step = true;
        game.MainLoop(true);
        gameThread.Step = false;
    }

    public void Dispose() =>
        clock.Stop();
}