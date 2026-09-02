using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Engine : IEngine, IDisposable
{
    private readonly IClock _clock;
    private readonly IBoardTime _boardTime;
    private readonly IScheduler _scheduler;
    private readonly IGame _game;
    private readonly IGameThread _gameThread;

    public Engine(
        IClock clock,
        IEngineAccessor engineAccessor,
        IBoardTime boardTime,
        IScheduler scheduler,
        IGame game,
        IGameThread gameThread)
    {
        engineAccessor.Instance = this;

        _clock = clock;
        _boardTime = boardTime;
        _scheduler = scheduler;
        _game = game;
        _gameThread = gameThread;
    }


    public void StepOnce()
    {
        _gameThread.Step = true;
        _game.MainLoop(true);
        _gameThread.Step = false;
    }

    public void Delay(int msec)
    {
        var waitUntil = DateTime.Now + TimeSpan.FromMilliseconds(msec);
        while (DateTime.Now < waitUntil)
            _scheduler.WaitForTick();
    }

    public int ResetBoardTimeHsec() =>
        _boardTime.Elapse();

    public void Dispose() =>
        _clock.Stop();
}