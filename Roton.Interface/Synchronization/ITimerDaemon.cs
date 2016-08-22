using System;

namespace Roton.Interface.Synchronization
{
    public interface ITimerDaemon
    {
        bool Paused { get; set; }
        void Pause();
        void Resume();
        int Start(Action executionMethod, double frequency);
        void Stop(int handle);
        void StopAll();
    }
}