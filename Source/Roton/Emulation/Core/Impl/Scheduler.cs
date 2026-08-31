using System;
using System.Threading;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Startup)]
internal sealed class Scheduler : IScheduler
{
    public event EventHandler? Tick;

    private int _ticksToRun;

    private readonly IEngineAccessor _engine;
    private readonly IState _state;
    private readonly IConfig _config;
    private readonly IBoardTime _boardTime;
    private readonly IClock _clock;
    private readonly Func<bool> _waitForTickFastDelegate;
    private readonly Func<bool> _waitForTickNormalDelegate;

    public Scheduler(
        IEngineAccessor engine,
        IState state,
        IConfig config,
        IBoardTime boardTime,
        IClock clock)
    {
        _engine = engine;
        _state = state;
        _config = config;
        _boardTime = boardTime;
        _clock = clock;

        _waitForTickFastDelegate = WaitForTickFastCondition;
        _waitForTickNormalDelegate = WaitForTickNormalCondition;
    }

    private IEngine Engine => _engine.Instance;

    private bool WaitForTickFastCondition()
    {
        if (_ticksToRun <= 0)
            return true;

        Engine.UpdateSound();
        Tick?.Invoke(this, EventArgs.Empty);
        Interlocked.Decrement(ref _ticksToRun);

        return false;
    }

    private bool WaitForTickNormalCondition() =>
        _ticksToRun > 0 || !Engine.ThreadActive;

    public void Advance()
    {
        if (_ticksToRun < 3)
            _ticksToRun++;

        if (!_state.GamePaused)
            _boardTime.Advance();

        if (!Engine.ThreadActive)
            _clock.Stop();
    }

    public void Reset() =>
        _ticksToRun = 0;

    public void WaitForTick()
    {
        var isFast = _state.GameWaitTime <= 0 && _config.FastMode;

        if (isFast)
        {
            SpinWait.SpinUntil(_waitForTickFastDelegate);
        }
        else
        {
            Engine.UpdateSound();

            Tick?.Invoke(this, EventArgs.Empty);

            SpinWait.SpinUntil(_waitForTickNormalDelegate);

            if (_ticksToRun > 0)
                Interlocked.Decrement(ref _ticksToRun);
        }
    }
}