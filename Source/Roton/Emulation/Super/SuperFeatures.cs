using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperFeatures(
    IEngineAccessor engine,
    IActorList actors,
    IElementList elements,
    ITiles tiles,
    IFacts facts,
    IBoard board,
    IHud hud,
    IWorld world,
    IState state,
    IWorldUnit worldUnit,
    IBoardTime boardTime,
    IBoardUpdater boardUpdater,
    IBroadcaster broadcaster)
    : IFeatures
{
    private IEngine Engine => engine.Instance;

    public void RemoveItem(Location location)
    {
        var result = new Tile(elements.FloorId, 0x00);

        for (var i = 0; i < 4; i++)
        {
            var targetVector = Engine.GetCardinalVector(i);
            var targetLocation = new Location(location.X + targetVector.X, location.Y + targetVector.Y);
            var adjacentTile = tiles[targetLocation];

            if (elements[adjacentTile.Id].Cycle >= 0)
                adjacentTile = actors.ActorAt(targetLocation).UnderTile;

            var adjacentElement = adjacentTile.Id;

            if (adjacentElement == elements.EmptyId ||
                adjacentElement == elements.SliderEwId ||
                adjacentElement == elements.SliderNsId ||
                adjacentElement == elements.BoulderId)
            {
                result.Color = 0;
                break;
            }

            if (adjacentElement == elements.FloorId)
                result.Color = adjacentTile.Color;
        }

        if (result.Color == 0)
            tiles[location].Id = elements.EmptyId;
        else
            tiles[location] = result;

        boardUpdater.UpdateBoard(location);
    }

    public void EnterBoard()
    {
        boardTime.Reset();
        broadcaster.BroadcastLabel(0, facts.EnterLabel, false);
        board.Entrance = actors.Player.Location;
        hud.UpdateCamera();
        world.TimePassed = 0;
        hud.UpdateStatus();
    }

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
                ShowInGameHelp();
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

    public void ShowInGameHelp()
    {
        broadcaster.BroadcastLabel(0, facts.HintLabel, false);
    }

    public void HandlePlayerInput(IActor actor)
    {
        // todo: this
    }

    public bool CanPutTile(Location location)
    {
        // do not allow #put on the bottom row
        return location.Y < tiles.Height;
    }

    public void ClearForest(Location location)
    {
        tiles[location] = new Tile(elements.FloorId, 0x02);
    }

    public void CleanUpOop(ref OopContext context)
    {
        var location = context.Actor.Location;
        Engine.PlotTile(location, context.DeathTile);
    }

    private bool TestAdjacent(Location location, int id)
    {
        var eId = tiles[location].Id;
        if (eId == id || eId == elements.BoardEdgeId)
            return true;

        if (tiles.ElementAt(location).Cycle >= 0)
        {
            eId = actors.ActorAt(location).UnderTile.Id;
            if (eId == id || eId == elements.BoardEdgeId)
                return true;
        }

        return false;
    }

    public int GetAdjacent(Location location, int id) =>
        (TestAdjacent(location + Vector.North, id) ? 1 : 0) |
        (TestAdjacent(location + Vector.South, id) ? 2 : 0) |
        (TestAdjacent(location + Vector.West, id) ? 4 : 0) |
        (TestAdjacent(location + Vector.East, id) ? 8 : 0);

    public string[] GetMessageLines()
    {
        return string.IsNullOrEmpty(state.Message2)
            ? [string.Empty, state.Message]
            : [state.Message, state.Message2];
    }

    public int BaseMemoryUsage => 203044;
}