using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalFeatures(
    IEngineAccessor engine,
    IActorList actorList,
    IAlerts alerts,
    IBoard board,
    IWorld world,
    IFacts facts,
    IState state,
    IHud hud,
    ITiles tiles,
    IElementList elementList,
    IWorldUnit worldUnit,
    IBoardTime boardTime,
    IBoardUpdater boardUpdater,
    IRadiusUpdater radiusUpdater,
    IMessenger messenger)
    : IFeatures
{
    private IEngine Engine => engine.Instance;

    public void EnterBoard()
    {
        boardTime.Reset();
        board.Entrance = actorList.Player.Location;
        if (board.IsDark && alerts.Dark)
        {
            messenger.SetMessage(facts.LongMessageDuration, alerts.DarkMessage);
            alerts.Dark = false;
        }

        world.TimePassed = 0;
        hud.UpdateStatus();
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

    public bool CanPutTile(Location location)
    {
        // do not allow #put on the bottom row
        return location.Y < tiles.Height;
    }

    public void ClearForest(Location location)
    {
        RemoveItem(location);
    }

    public string[] GetMessageLines()
    {
        return [state.Message];
    }

    public void ShowAbout()
    {
        hud.ShowHelp("About Roton...", "ABOUT");
    }

    public int BaseMemoryUsage => 205791;

    public void CleanUpOop(ref OopContext context)
    {
        var location = context.Actor.Location;
        Engine.Harm(context.Index);
        Engine.PlotTile(location, context.DeathTile);
    }

    public int GetColorMatchValue(int color)
    {
        return color;
    }

    private bool TestAdjacent(Location location, int id)
    {
        var eId = tiles[location].Id;
        return eId == id || eId == elementList.BoardEdgeId;
    }

    public int GetAdjacent(Location location, int id) =>
        (TestAdjacent(location + Vector.North, id) ? 1 : 0) |
        (TestAdjacent(location + Vector.South, id) ? 2 : 0) |
        (TestAdjacent(location + Vector.West, id) ? 4 : 0) |
        (TestAdjacent(location + Vector.East, id) ? 8 : 0);

    public bool HandleTitleInput()
    {
        switch (state.KeyPressed.ToUpperCase())
        {
            case EngineKeyCode.P:
                return true;
            case EngineKeyCode.W:
                worldUnit.OpenWorld();
                break;
            case EngineKeyCode.A:
                ShowAbout();
                break;
            case EngineKeyCode.E:
                break;
            case EngineKeyCode.S:
                hud.CreateStatusText();
                state.GameSpeed = hud.SelectParameter(
                    true, 0x42, 0x15, "Game speed:;FS", state.GameSpeed, null);
                break;
            case EngineKeyCode.R:
                return worldUnit.RestoreWorld();
            case EngineKeyCode.H:
                Engine.ShowHighScores();
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

    public void RemoveItem(Location location)
    {
        tiles[location].Id = elementList.EmptyId;
        boardUpdater.UpdateBoard(location);
    }

    public void ShowInGameHelp()
    {
        hud.ShowHelp("Playing Roton", "GAME");
    }
}