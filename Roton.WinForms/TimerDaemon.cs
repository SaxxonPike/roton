using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Roton.WinForms
{
    public partial class TimerDaemon : Component
    {
        bool _disposed;
        bool _paused;

        private class TimerDaemonInfo
        {
            public double Frequency { get; }
            public Action Method { get; }
            public bool Running { get; set; }
            public Thread Thread { get; set; }

            public TimerDaemonInfo(Action m, double f, Thread t)
            {
                Method = m;
                Frequency = f;
                Running = false;
                Thread = t;
            }
        }

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

        protected override void Dispose(bool disposing)
        {
            StopAll();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            _disposed = true;
            base.Dispose(disposing);
        }

        void Initialize()
        {
            TimerThreads = new Dictionary<int, TimerDaemonInfo>();
        }

        public void Pause()
        {
            _paused = true;
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

        public void Resume()
        {
            _paused = false;
        }

        public int Start(Action executionMethod, double frequency)
        {
            var thread = new Thread(TimerThreadMethod);
            var info = new TimerDaemonInfo(executionMethod, frequency, thread);
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

        void TimerThreadMethod(object timerDaemonInfo)
        {
            var info = timerDaemonInfo as TimerDaemonInfo;
            var method = info.Method;
            var sw = new Stopwatch();
            var freq = (long) ((double) Stopwatch.Frequency/(double) info.Frequency);
            long next = 0;
            long now = 0;
            var repetitions = 0;
            sw.Start();

            info.Running = true;
            while (info.Running && !_disposed)
            {
                Thread.Sleep(1);
                now = sw.ElapsedTicks;
                repetitions = 0;
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
            info.Thread = null;
        }

        private int TimerThreadIndex { get; set; }

        private Dictionary<int, TimerDaemonInfo> TimerThreads { get; set; }
    }
}