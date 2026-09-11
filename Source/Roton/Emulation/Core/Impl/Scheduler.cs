using System;
using System.Threading;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Scheduler : IScheduler
{
    public event EventHandler? Tick;

    private int _ticksToRun;

    private readonly IState _state;
    private readonly IConfig _config;
    private readonly IClock _clock;
    private readonly IGameThread _gameThread;
    private readonly ISoundPlayer _soundPlayer;
    private readonly ITime _time;
    private readonly Func<bool> _waitForTickFastDelegate;
    private readonly Func<bool> _waitForTickNormalDelegate;

    public Scheduler(IState state,
        IConfig config,
        IClock clock,
        IGameThread gameThread,
        ISoundPlayer soundPlayer,
        ITime time)
    {
        _state = state;
        _config = config;
        _clock = clock;
        _gameThread = gameThread;
        _soundPlayer = soundPlayer;
        _time = time;

        _waitForTickFastDelegate = WaitForTickFastCondition;
        _waitForTickNormalDelegate = WaitForTickNormalCondition;
    }

    private bool WaitForTickFastCondition()
    {
        if (_ticksToRun <= 0)
            return true;

        _soundPlayer.UpdateSound();
        Tick?.Invoke(this, EventArgs.Empty);
        Interlocked.Decrement(ref _ticksToRun);
        _time.Tick();

        return false;
    }

    private bool WaitForTickNormalCondition() =>
        _ticksToRun > 0 || !_gameThread.ThreadActive;

    public void Advance()
    {
        if (_ticksToRun < 3)
            _ticksToRun++;

        if (!_gameThread.ThreadActive)
            _clock.Stop();
    }

    public void Reset()
    {
        _ticksToRun = 0;
        _state.SoundTimeCheckCounter = 36;
        _state.TimerTicks = 0;
        _state.PlayerTimer.Reset();
    }

    public void WaitForTick()
    {
        var isFast = _state.GameWaitTime <= 0 && _config.Engine.FastMode;

        if (isFast)
        {
            SpinWait.SpinUntil(_waitForTickFastDelegate);
        }
        else
        {
            _soundPlayer.UpdateSound();

            Tick?.Invoke(this, EventArgs.Empty);
            _time.Tick();

            SpinWait.SpinUntil(_waitForTickNormalDelegate);

            if (_ticksToRun > 0)
                Interlocked.Decrement(ref _ticksToRun);
        }
    }
}