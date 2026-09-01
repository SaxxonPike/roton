using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public class SuperPlayerInputHandler(
    IState state,
    IWorldUnit worldUnit,
    IDialogs dialogs,
    IHud hud)
    : IPlayerInputHandler
{
    public bool HandleTitleInput()
    {
        switch (state.KeyPressed.ToUpperCase())
        {
            case EngineKeyCode.Enter: // Enter
                return true;
            case EngineKeyCode.W: // W
                worldUnit.OpenWorld();
                break;
            case EngineKeyCode.R: // R
                return worldUnit.RestoreWorld();
            case EngineKeyCode.H: // H
                dialogs.ShowHelp();
                break;
            case EngineKeyCode.QuestionMark: // ?
                break;
            case EngineKeyCode.Escape: // esc
            case EngineKeyCode.Q: // Q
                state.QuitEngine = hud.QuitEngineConfirmation();
                break;
        }

        return false;
    }

    public void HandlePlayerInput(IActor actor)
    {
        // todo: this
    }
}