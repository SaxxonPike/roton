namespace Roton.Emulation.Core;

public interface IGame
{
    /// <remarks>
    /// RoZ: GamePlayLoop, GameTitleLoop
    /// </remarks>
    void MainLoop(bool doFade);

    void StepOnce();
}