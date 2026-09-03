using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalPlayerInputHandler(
    IState state,
    IWorldManager worldManager,
    IDialogs dialogs,
    IHud hud,
    IWorld world,
    IAlerts alerts,
    IMessenger messenger,
    IFacts facts,
    IBoard board,
    IRadiusUpdater radiusUpdater)
    : IPlayerInputHandler
{
    public bool HandleTitleInput()
    {
        switch (state.KeyPressed.ToUpperCase())
        {
            case EngineKeyCode.P:
                return true;
            case EngineKeyCode.W:
                worldManager.OpenWorld();
                break;
            case EngineKeyCode.A:
                dialogs.ShowAbout();
                break;
            case EngineKeyCode.E:
                break;
            case EngineKeyCode.S:
                hud.CreateStatusText();
                state.GameSpeed = hud.SelectParameter(
                    true, 0x42, 0x15, "Game speed:;FS", state.GameSpeed, null);
                break;
            case EngineKeyCode.R:
                return worldManager.RestoreWorld();
            case EngineKeyCode.H:
                dialogs.ShowHighScores();
                break;
            case EngineKeyCode.QuestionMark:
                hud.EnterCheat();
                break;
            case EngineKeyCode.Escape:
            case EngineKeyCode.Q:
                state.QuitEngine = hud.QuitEngineConfirmation();
                break;
        }

        return false;
    }

    public void HandlePlayerInput(IActor actor)
    {
        switch (state.KeyPressed.ToUpperCase())
        {
            case EngineKeyCode.T:
                if (world.TorchCycles <= 0)
                {
                    if (world.Torches <= 0)
                    {
                        if (alerts.NoTorches)
                        {
                            messenger.SetMessage(facts.LongMessageDuration, alerts.NoTorchMessage);
                            alerts.NoTorches = false;
                        }
                    }
                    else if (!board.IsDark)
                    {
                        if (alerts.NotDark)
                        {
                            messenger.SetMessage(facts.LongMessageDuration, alerts.NotDarkMessage);
                            alerts.NotDark = false;
                        }
                    }
                    else
                    {
                        world.Torches--;
                        world.TorchCycles = 0xC8;
                        radiusUpdater.UpdateRadius(actor.Location, RadiusMode.Update);
                        hud.UpdateStatus();
                    }
                }

                break;
            case EngineKeyCode.F:
                break;
        }
    }
}