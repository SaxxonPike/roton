using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Roton.Emulation
{
    // this is the master core timer, it'll automatically start and stop when it is queried regularly
    static internal class CoreTimer
    {
        static private void Initialize()
        {
            if (!Initialized)
            {
                Initialized = true;
                var thread = new Thread(new ThreadStart(ThreadLoop));
                thread.Start();
            }
        }

        static public bool Initialized
        {
            get;
            private set;
        }

        static private int _maxLaggedTicks;
        static public int MaxLaggedTicks
        {
            get
            {
                if (_maxLaggedTicks <= 0)
                {
                    _maxLaggedTicks = 5;
                }
                return _maxLaggedTicks;
            }
            set
            {
                _maxLaggedTicks = value;
            }
        }

        static private void ResetShutdown()
        {
            TicksUntilShutdown = 50;
        }

        static private void ThreadLoop()
        {
            Stopwatch timer = new Stopwatch();
            long frequency = Stopwatch.Frequency * 10L / 718L;
            long lastTime = timer.ElapsedTicks;
            long currentTime = lastTime;
            int timesRun = 0;
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

        static private int _tick;
        static public int Tick
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
            set
            {
                _tick = value;
            }
        }

        static private int TicksUntilShutdown
        {
            get;
            set;
        }

        static private void UnInitialize()
        {
            Initialized = false;
        }
    }
}
