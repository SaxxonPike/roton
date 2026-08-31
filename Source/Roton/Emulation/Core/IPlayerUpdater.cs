namespace Roton.Emulation.Core;

public interface IPlayerUpdater
{
    void ForcePlayerColor(int index);
    void CleanUpPauseMovement();
}