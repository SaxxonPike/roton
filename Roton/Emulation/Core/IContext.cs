namespace Roton.Emulation.Core
{
    public interface IContext
    {
        IEngine Engine { get; }
        void ExecuteOnce();
        void Start();
        void Stop();
    }
}