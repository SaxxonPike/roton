using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Roton.Interface.Synchronization
{
    public partial class TimerDaemon : Component, ITimerDaemon
    {
        private bool _disposed;
        private bool _paused;

        public TimerDaemon()
        {
            InitializeComponent();
            Initialize();
        }

        public TimerDaemon(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            Initialize();
        }

        public bool Paused
        {
            get { return _paused; }
            set
            {
                if (value)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }
        }

        private int TimerThreadIndex { get; set; }

        private Dictionary<int, TimerDaemonInfo> TimerThreads { get; set; }

        protected override void Dispose(bool disposing)
        {
            StopAll();
            if (disposing)
            {
                components?.Dispose();
            }
            _disposed = true;
            base.Dispose(disposing);
        }

        private void Initialize()
        {
            TimerThreads = new Dictionary<int, TimerDaemonInfo>();
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }

        public int Start(Action executionMethod, double frequency)
        {
            var thread = new Thread(TimerThreadMethod);
            var info = new TimerDaemonInfo(executionMethod, frequency);
            TimerThreads[TimerThreadIndex++] = info;
            thread.Start(info);
            return TimerThreadIndex - 1;
        }

        public void Stop(int handle)
        {
            if (TimerThreads.ContainsKey(handle))
            {
                TimerThreads[handle].Running = false;
                TimerThreads.Remove(handle);
            }
        }

        public void StopAll()
        {
            foreach (var item in TimerThreads)
            {
                // should cause all threads to stop
                item.Value.Running = false;
            }
            TimerThreads.Clear();
        }

        private void TimerThreadMethod(object timerDaemonInfo)
        {
            var info = (TimerDaemonInfo) timerDaemonInfo;
            var method = info.Method;
            var sw = new Stopwatch();
            var freq = (long) (Stopwatch.Frequency/info.Frequency);
            long next = 0;
            sw.Start();

            info.Running = true;
            while (info.Running && !_disposed)
            {
                Thread.Sleep(1);
                var now = sw.ElapsedTicks;
                var repetitions = 0;
                while (next < now && repetitions < 5)
                {
                    if (!_paused)
                    {
                        method();
                    }
                    repetitions++;
                    next += freq;
                }
                if (next < now)
                {
                    next = now;
                }
            }
        }

        private class TimerDaemonInfo
        {
            public TimerDaemonInfo(Action m, double f)
            {
                Method = m;
                Frequency = f;
                Running = false;
            }

            public double Frequency { get; }
            public Action Method { get; }
            public bool Running { get; set; }
        }
    }
}