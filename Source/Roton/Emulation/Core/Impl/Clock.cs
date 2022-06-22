using System;
using System.Diagnostics;
using System.Threading;

namespace Roton.Emulation.Core.Impl;

public sealed class Clock : IClock
{
    private readonly long _numerator;
    private readonly long _denominator;
    private bool _running;

    internal Clock(long numerator, long denominator)
    {
        _numerator = numerator;
        _denominator = denominator;
    }

    private bool _initialized;

    public event EventHandler OnTick;

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
        if (!_initialized)
        {
            _running = true;
            _initialized = true;
            var thread = new Thread(ThreadLoop);
            thread.Start();
        }
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
                OnTick?.Invoke(this, EventArgs.Empty);
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