using System;

namespace Roton.Interface.Synchronization
{
    public class TimedEvent : ITimedEvent
    {
        private readonly ITimerDaemon _daemon;
        private readonly Action _action;
        private readonly double _frequency;
        private int _handle;

        public TimedEvent(ITimerDaemon daemon, Action action, double frequency)
        {
            _daemon = daemon;
            _action = action;
            _frequency = frequency;
        }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (!IsRunning)
                _handle = _daemon.Start(_action, _frequency);
            IsRunning = true;
        }

        public void Stop()
        {
            if (IsRunning)
                _daemon.Stop(_handle);
            IsRunning = false;
        }
    }
}
