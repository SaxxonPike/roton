using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperFeatures(IEngineAccessor engine) : IFeatures
{
    private IEngine Engine => engine.Instance;

    public void LockActor(int index)
    {
        Engine.Actors[index].P3 = 1;
    }

    public void UnlockActor(int index)
    {
        Engine.Actors[index].P3 = 0;
    }

    public bool IsActorLocked(int index)
    {
        return Engine.Actors[index].P3 != 0;
    }

    public void RemoveItem(Location location)
    {
        var result = new Tile(Engine.ElementList.FloorId, 0x00);

        for (var i = 0; i < 4; i++)
        {
            var targetVector = Engine.GetCardinalVector(i);
            var targetLocation = new Location(location.X + targetVector.X, location.Y + targetVector.Y);
            var adjacentTile = Engine.Tiles[targetLocation];

            if (Engine.ElementList[adjacentTile.Id].Cycle >= 0)
                adjacentTile = Engine.ActorAt(targetLocation).UnderTile;

            var adjacentElement = adjacentTile.Id;

            if (adjacentElement == Engine.ElementList.EmptyId ||
                adjacentElement == Engine.ElementList.SliderEwId ||
                adjacentElement == Engine.ElementList.SliderNsId ||
                adjacentElement == Engine.ElementList.BoulderId)
            {
                result.Color = 0;
                break;
            }

            if (adjacentElement == Engine.ElementList.FloorId)
                result.Color = adjacentTile.Color;
        }

        if (result.Color == 0)
            Engine.Tiles[location].Id = Engine.ElementList.EmptyId;
        else
            Engine.Tiles[location] = result;

        Engine.UpdateBoard(location);
    }

    public string GetWorldName(string baseName) => $"{baseName}.SZT";

    public string GetHighScoreName(string baseName) => $"{baseName}.HGS";

    public void EnterBoard()
    {
        Engine.Hud.UpdateBorder();
        Engine.BroadcastLabel(0, Engine.Facts.EnterLabel, false);
        Engine.Board.Entrance = Engine.Actors.Player.Location;
        Engine.Hud.UpdateCamera();
        Engine.World.TimePassed = 0;
        Engine.Hud.UpdateStatus();
    }

    public bool HandleTitleInput()
    {
        switch (Engine.State.KeyPressed.ToUpperCase())
        {
            case EngineKeyCode.Enter: // Enter
                return true;
            case EngineKeyCode.W: // W
                Engine.OpenWorld();
                break;
            case EngineKeyCode.R: // R
                return Engine.RestoreWorld();
            case EngineKeyCode.H: // H
                ShowInGameHelp();
                break;
            case EngineKeyCode.QuestionMark: // ?
                break;
            case EngineKeyCode.Escape: // esc
            case EngineKeyCode.Q: // Q
                Engine.State.QuitEngine = Engine.Hud.QuitEngineConfirmation();
                break;
        }

        return false;
    }

    public void ShowInGameHelp()
    {
        Engine.BroadcastLabel(0, Engine.Facts.HintLabel, false);
    }

    public IScrollState? ExecuteMessage(ref OopContext context)
    {
        if (!context.HasMessage)
            return null;

        var message = context.GetMessage();

        switch (message.Count)
        {
            case 1:
                Engine.SetMessage(Engine.Facts.LongMessageDuration, new Message(string.Empty, message[0]));
                return null;
            case 2:
                Engine.SetMessage(Engine.Facts.LongMessageDuration,
                    new Message(message[0], message[1]));
                return null;
            case 0:
                return null;
            default:
                Engine.State.KeyVector = Vector.Idle;
                return Engine.Hud.ShowScroll(false, context.Name, [.. message]);
        }
    }

    public void HandlePlayerInput(IActor actor)
    {
        // todo: this
    }

    public bool CanPutTile(Location location)
    {
        // do not allow #put on the bottom row
        return location.Y < Engine.Tiles.Height;
    }

    public void ClearForest(Location location)
    {
        Engine.Tiles[location] = new Tile(Engine.ElementList.FloorId, 0x02);
    }

    public void CleanUpPauseMovement()
    {
        var target = Engine.Player.Location + Engine.State.KeyVector;

        if (Engine.ElementAt(Engine.Player.Location).Id == Engine.ElementList.PlayerId)
        {
            Engine.MoveActor(0, target);
        }
        else
        {
            Engine.UpdateBoard(Engine.Player.Location);
            Engine.Player.Location += Engine.State.KeyVector;
            Engine.Player.UnderTile = Engine.Tiles[Engine.Player.Location];
            Engine.Tiles[Engine.Player.Location] = new Tile(Engine.ElementList.PlayerId, Engine.ElementList.Player().Color);
            Engine.UpdateBoard(Engine.Player.Location);
            Engine.UpdateRadius(Engine.Player.Location, RadiusMode.Update);
            Engine.UpdateRadius(Engine.Player.Location - Engine.State.KeyVector, RadiusMode.Update);
        }
    }

    public string? OpenWorld() => 
        Engine.ShowLoad("Super ZZT Worlds", "szt");

    public string? RestoreWorld() => 
        Engine.ShowLoad("Saved Games", "sav");

    public void CleanUpOop(ref OopContext context)
    {
        var location = context.Actor.Location;
        Engine.PlotTile(location, context.DeathTile);
    }

    public int GetColorMatchValue(int color)
    {
        return color & 0x07;
    }

    public void NotifyActorSentLabel(int index)
    {
        // When an object receives a label, the current
        // in-progress movement counter is reset.

        Engine.Actors[index].P2 = 0;
    }

    public string GetSaveName(string baseName)
    {
        return $"{baseName}.SAV";
    }

    private bool TestAdjacent(Location location, int id)
    {
        var eId = Engine.Tiles[location].Id;
        if (eId == id || eId == Engine.ElementList.BoardEdgeId)
            return true;

        if (Engine.ElementAt(location).Cycle >= 0)
        {
            eId = Engine.ActorAt(location).UnderTile.Id;
            if (eId == id || eId == Engine.ElementList.BoardEdgeId)
                return true;
        }

        return false;
    }
    
    public int GetAdjacent(Location location, int id) =>
        (TestAdjacent(location + Vector.North, id) ? 1 : 0) |
        (TestAdjacent(location + Vector.South, id) ? 2 : 0) |
        (TestAdjacent(location + Vector.West, id) ? 4 : 0) |
        (TestAdjacent(location + Vector.East, id) ? 8 : 0);

    public void CleanUpPassageMovement()
    {
        Engine.Tiles[Engine.Player.Location] = Engine.Player.UnderTile;
    }

    public void ForcePlayerColor(int index)
    {
        // Super does not enforce player's background color.
    }

    public string[] GetMessageLines()
    {
        return string.IsNullOrEmpty(Engine.State.Message2)
            ? [string.Empty, Engine.State.Message]
            : [Engine.State.Message, Engine.State.Message2];
    }

    public void ShowAbout()
    {
    }

    public int BaseMemoryUsage => 203044;
}