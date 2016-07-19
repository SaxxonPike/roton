using System.Diagnostics;
using System.Threading;

namespace Roton.Emulation
{
    // this is the master core timer, it'll automatically start and stop when it is queried regularly
    internal static class CoreTimer
    {
        private static void Initialize()
        {
            if (!Initialized)
            {
                Initialized = true;
                var thread = new Thread(ThreadLoop);
                thread.Start();
            }
        }

        public static bool Initialized { get; private set; }

        private static int _maxLaggedTicks;

        public static int MaxLaggedTicks
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

        private static void ResetShutdown()
        {
            TicksUntilShutdown = 50;
        }

        private static void ThreadLoop()
        {
            var timer = new Stopwatch();
            var frequency = Stopwatch.Frequency*10L/718L;
            var lastTime = timer.ElapsedTicks;
            var currentTime = lastTime;
            var timesRun = 0;
            ResetShutdown();
            timer.Start();
            while (TicksUntilShutdown > 0)
            {
                currentTime = timer.ElapsedTicks;
                if (lastTime > currentTime)
                {
                    // this will prevent Int64 wrap-around bugs at the expense of ~1 missed tick
                    lastTime = currentTime;
                }

                timesRun = 0;
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

        private static int _tick;

        public static int Tick
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

        private static int TicksUntilShutdown { get; set; }

        private static void UnInitialize()
        {
            Initialized = false;
        }
    }
}