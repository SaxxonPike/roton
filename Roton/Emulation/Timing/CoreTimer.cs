using System.Diagnostics;
using System.Threading;

namespace Roton.Emulation.Timing
{
    // this is the master core timer, it'll automatically start and stop when it is queried regularly
    public class CoreTimer
    {
        private int _maxLaggedTicks;

        private int _tick;

        public bool Initialized { get; private set; }

        public int MaxLaggedTicks
        {
            get
            {
                if (_maxLaggedTicks <= 0)
                {
                    _maxLaggedTicks = 5;
                }
                return _maxLaggedTicks;
            }
            set { _maxLaggedTicks = value; }
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
            set { _tick = value; }
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
}