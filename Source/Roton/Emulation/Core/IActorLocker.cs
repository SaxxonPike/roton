namespace Roton.Emulation.Core;

public interface IActorLocker
{
    void Lock(int index);
    void Unlock(int index);
    bool IsLocked(int index);
}