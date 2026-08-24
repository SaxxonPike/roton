using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalFeatures(
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
    IBoardTime boardTime)
    : IFeatures
{
    private IEngine Engine => engine.Instance;


    public void LockActor(int index)
    {
        actorList[index].P2 = 1;
    }

    public void UnlockActor(int index)
    {
        actorList[index].P2 = 0;
    }

    public bool IsActorLocked(int index)
    {
        return actorList[index].P2 != 0;
    }

    public string GetHighScoreName(string baseName) => $"{baseName}.HI";

    public void EnterBoard()
    {
        boardTime.Reset();
        board.Entrance = actorList.Player.Location;
        if (board.IsDark && alerts.Dark)
        {
            Engine.SetMessage(facts.LongMessageDuration, alerts.DarkMessage);
            alerts.Dark = false;
        }

        world.TimePassed = 0;
        hud.UpdateStatus();
    }

    public IScrollState? ExecuteMessage(ref OopContext context)
    {
        var message = context.GetMessage();

        switch (message)
        {
            case { Count: 1 }:
                Engine.SetMessage(facts.LongMessageDuration, new Message(message));
                return null;
            case { Count: > 1 }:
                state.KeyVector = Vector.Idle;
                return hud.ShowScroll(false, context.Name, [.. message]);
            default:
                return null;
        }
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
                            Engine.SetMessage(facts.LongMessageDuration, alerts.NoTorchMessage);
                            alerts.NoTorches = false;
                        }
                    }
                    else if (!board.IsDark)
                    {
                        if (alerts.NotDark)
                        {
                            Engine.SetMessage(facts.LongMessageDuration, alerts.NotDarkMessage);
                            alerts.NotDark = false;
                        }
                    }
                    else
                    {
                        world.Torches--;
                        world.TorchCycles = 0xC8;
                        Engine.UpdateRadius(actor.Location, RadiusMode.Update);
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

    public void CleanUpPassageMovement()
    {
        tiles[actorList.Player.Location] = new Tile(elementList.EmptyId, 0);
    }

    public void ForcePlayerColor(int index)
    {
        var actor = actorList[index];
        var playerElement = elementList.Player();
        if (tiles[actor.Location].Color == playerElement.Color &&
            playerElement.Character == facts.PlayerCharacter)
            return;

        playerElement.Character = facts.PlayerCharacter;
        tiles[actor.Location].Color = playerElement.Color;
        Engine.UpdateBoard(actor.Location);
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

    public void CleanUpPauseMovement()
    {
        var target = actorList.Player.Location + state.KeyVector;

        if (Engine.ElementAt(actorList.Player.Location).Id == elementList.PlayerId)
        {
            Engine.MoveActor(0, target);
        }
        else
        {
            Engine.UpdateBoard(actorList.Player.Location);
            actorList.Player.Location += state.KeyVector;
            tiles[actorList.Player.Location] =
                new Tile(elementList.PlayerId, elementList.Player().Color);
            Engine.UpdateBoard(actorList.Player.Location);
            Engine.UpdateRadius(actorList.Player.Location, RadiusMode.Update);
            Engine.UpdateRadius(actorList.Player.Location - state.KeyVector, RadiusMode.Update);
        }
    }

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

    public void NotifyActorSentLabel(int index)
    {
        // Does nothing in the original engine.
    }

    public string GetSaveName(string baseName)
    {
        return $"{baseName}.SAV";
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
        Engine.UpdateBoard(location);
    }

    public void ShowInGameHelp()
    {
        hud.ShowHelp("Playing Roton", "GAME");
    }

    public string GetWorldName(string baseName)
    {
        return $"{baseName}.ZZT";
    }
}