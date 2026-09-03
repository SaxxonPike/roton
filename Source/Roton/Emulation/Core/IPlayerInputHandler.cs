using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IPlayerInputHandler
{
    bool HandleTitleInput();
    void HandlePlayerInput(IActor actor);
}