namespace Roton.Emulation.Core;

public interface IActorLocker
{
    /// <remarks>
    /// RoZ: OopExecute (#LOCK)
    /// </remarks>
    void Lock(int index);

    /// <remarks>
    /// RoZ: OopExecute (#UNLOCK)
    /// </remarks>
    void Unlock(int index);

    bool IsLocked(int index);
}