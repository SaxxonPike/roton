namespace Roton.Emulation.Core;

public interface IPlayerUpdater
{
    /// <remarks>
    /// RoZ: GamePlayLoop (player color update only)
    /// </remarks>
    void ForcePlayerColor(int index);

    /// <remarks>
    /// RoZ: GamePlayLoop (pause movement cleanup only)
    /// </remarks>
    void CleanUpPauseMovement();

    /// <remarks>
    /// RoZ: BoardPassageTeleport (tile cleanup only)
    /// </remarks>
    void CleanUpPassageMovement();
}