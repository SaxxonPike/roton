using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IPlayerInputHandler
{
    /// <remarks>
    /// RoZ: GameTitleLoop (input handling only)
    /// </remarks>
    bool HandleTitleInput();

    /// <remarks>
    /// RoZ: GamePlayLoop (player input handling only)
    /// </remarks>
    void HandlePlayerInput(IActor actor);
}