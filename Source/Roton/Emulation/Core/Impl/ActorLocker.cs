using Roton.Emulation.Data;

namespace Roton.Emulation.Core.Impl;

public abstract class ActorLocker : IActorLocker
{
    public void LockActor(int index) =>
        GetLockedRef(index) = true;

    public void UnlockActor(int index) =>
        GetLockedRef(index) = false;

    public bool IsActorLocked(int index) =>
        GetLockedRef(index);

    protected abstract ref Bool GetLockedRef(int index);
}