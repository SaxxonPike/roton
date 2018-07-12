using System.Diagnostics;
using System.Threading;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class Clock : IClock
    {
        private int _maxLaggedTicks;

        private int _tick;

        private bool Initialized { get; set; }

        private int MaxLaggedTicks
        {
            get
            {
                if (_maxLaggedTicks <= 0)
                {
                    _maxLaggedTicks = 5;
                }
                return _maxLaggedTicks;
            }
        }

        public int Tick
        {
            get
            {
                if (!Initialized)
                {
                    Initialize();
                }
                ResetShutdown();
                return _tick;
            }
            set => _tick = value;
        }

        private int TicksUntilShutdown { get; set; }

        private void Initialize()
        {
            if (!Initialized)
            {
                Initialized = true;
                var thread = new Thread(ThreadLoop);
                thread.Start();
            }
        }

        private void ResetShutdown()
        {
            TicksUntilShutdown = 50;
        }

        private void ThreadLoop()
        {
            var timer = new Stopwatch();
            var frequency = Stopwatch.Frequency*10L/718L;
            var lastTime = timer.ElapsedTicks;
            ResetShutdown();
            timer.Start();
            while (TicksUntilShutdown > 0)
            {
                var currentTime = timer.ElapsedTicks;
                if (lastTime > currentTime)
                {
                    // this will prevent Int64 wrap-around bugs at the expense of ~1 missed tick
                    lastTime = currentTime;
                }

                var timesRun = 0;
                while (timesRun < MaxLaggedTicks && currentTime - lastTime > frequency)
                {
                    lastTime += frequency;
                    _tick++;
                    timesRun++;
                }
                Thread.Sleep(1);
                TicksUntilShutdown--;
            }
            UnInitialize();
        }

        private void UnInitialize()
        {
            Initialized = false;
        }
    }

    public interface IClock
    {
        int Tick { get; }
    }
}