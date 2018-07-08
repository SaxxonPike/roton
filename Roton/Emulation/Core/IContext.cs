namespace Roton.Core
{
    public interface IContext
    {
        void ExecuteOnce();
        void Start();
        void Stop();
    }
}