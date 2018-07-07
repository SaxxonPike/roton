namespace Roton.Core
{
    public interface ILocker
    {
        void Lock(int index);
        void Unlock(int index);
        bool IsLocked(int index);
    }
}