namespace Roton.Emulation.Core;

public interface IActorLocker
{
    void LockActor(int index);
    void UnlockActor(int index);
    bool IsActorLocked(int index);
}