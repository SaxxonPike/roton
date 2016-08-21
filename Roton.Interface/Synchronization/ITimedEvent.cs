namespace Roton.Interface.Synchronization
{
    public interface ITimedEvent
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}