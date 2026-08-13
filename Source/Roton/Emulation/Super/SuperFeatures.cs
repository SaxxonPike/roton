using System;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperFeatures : IFeatures
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public SuperFeatures(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

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

    public void RemoveItem(IXyPair location)
    {
        var result = new Tile(Engine.ElementList.FloorId, 0x00);
        var finished = false;

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
                finished = true;
                result.Color = 0;
            }

            if (adjacentElement == Engine.ElementList.FloorId)
            {
                result.Color = adjacentTile.Color;
            }

            if (finished)
            {
                break;
            }
        }

        if (result.Color == 0)
        {
            result.Id = Engine.ElementList.EmptyId;
        }

        Engine.Tiles[location].CopyFrom(result);
    }

    public string GetWorldName(string baseName) => $"{baseName}.SZT";

    public string GetHighScoreName(string baseName) => $"{baseName}.HGS";

    public void EnterBoard()
    {
        Engine.BroadcastLabel(0, Engine.Facts.EnterLabel, false);
        Engine.Board.Entrance.CopyFrom(Engine.Actors.Player.Location);
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
                break;
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

    public IScrollState ExecuteMessage(IOopContext context)
    {
        switch (context.Message.Count)
        {
            case 1:
                Engine.SetMessage(Engine.Facts.LongMessageDuration, new Message(string.Empty, context.Message[0]));
                return null;
            case 2:
                Engine.SetMessage(Engine.Facts.LongMessageDuration, new Message(context.Message[0], context.Message[1]));
                return null;
            case 0:
                return null;
            default:
                Engine.State.KeyVector.SetTo(0, 0);
                return Engine.Hud.ShowScroll(false, context.Name, context.Message.ToArray());
        }
    }

    public void HandlePlayerInput(IActor actor)
    {
        // todo: this
    }

    public bool CanPutTile(IXyPair location)
    {
        return true;
    }

    public void ClearForest(IXyPair location)
    {
        Engine.Tiles[location].SetTo(Engine.ElementList.FloorId, 0x02);
    }

    public void CleanUpPauseMovement()
    {
        var target = Engine.Player.Location.Sum(Engine.State.KeyVector);
            
        if (Engine.ElementAt(Engine.Player.Location).Id == Engine.ElementList.PlayerId)
        {
            Engine.MoveActor(0, target);
        }
        else
        {
            Engine.UpdateBoard(Engine.Player.Location);
            Engine.Player.Location.Add(Engine.State.KeyVector);
            Engine.Player.UnderTile.CopyFrom(Engine.Tiles[Engine.Player.Location]);
            Engine.Tiles[Engine.Player.Location].SetTo(Engine.ElementList.PlayerId, Engine.ElementList.Player().Color);
            Engine.UpdateBoard(Engine.Player.Location);
            Engine.UpdateRadius(Engine.Player.Location, RadiusMode.Update);
            Engine.UpdateRadius(Engine.Player.Location.Difference(Engine.State.KeyVector), RadiusMode.Update);
        }
    }

    public string OpenWorld()
    {
        return Engine.ShowLoad("ZZT Worlds", "szt");
    }

    public void CleanUpPassageMovement()
    {
        Engine.Tiles[Engine.Player.Location].CopyFrom(Engine.Player.UnderTile);
    }

    public void ForcePlayerColor(int index)
    {
        // Super does not enforce player's background color.
    }

    public string[] GetMessageLines()
    {
        return string.IsNullOrEmpty(Engine.State.Message2)
            ? new[] {string.Empty, Engine.State.Message}
            : new[] {Engine.State.Message, Engine.State.Message2};
    }

    public void ShowAbout()
    {
    }

    public int BaseMemoryUsage => 203044;
}