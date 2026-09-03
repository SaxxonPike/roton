using System;
using System.Diagnostics;
using System.Threading;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Clock(IConfig config) : IClock
{
    private readonly long _numerator = config.MasterClockNumerator;
    private readonly long _denominator = config.MasterClockDenominator;

    private bool _running;
    private bool _initialized;

    public event Action? OnTick;

    public void Start()
    {
        Initialize();
    }

    public void Stop()
    {
        _running = false;
    }

    private void Initialize()
    {
        if (_initialized) 
            return;

        _running = true;
        _initialized = true;

        var thread = new Thread(ThreadLoop);
        thread.Start();
    }

    private void ThreadLoop()
    {
        var timer = new Stopwatch();
        var frequency = Stopwatch.Frequency * _numerator / _denominator;
        var lastTime = timer.ElapsedTicks;
        timer.Start();

        SpinWait.SpinUntil(() =>
        {
            if (!_running)
                return true;

            var currentTime = timer.ElapsedTicks;
            if (lastTime > currentTime)
            {
                // this will prevent Int64 wrap-around bugs at the expense of ~1 missed tick
                lastTime = currentTime;
            }

            while (currentTime - lastTime > frequency)
            {
                lastTime += frequency;
                OnTick?.Invoke();
            }
            
            return false;
        });
        
        _initialized = false;
    }

    public void Dispose()
    {
        Stop();
    }
}