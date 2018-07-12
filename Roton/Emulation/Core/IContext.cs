namespace Roton.Emulation.Core
{
    public interface IContext
    {
        void ExecuteOnce();
        void Start();
        void Stop();
    }
}