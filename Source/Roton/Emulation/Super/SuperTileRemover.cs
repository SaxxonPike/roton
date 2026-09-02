using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperTileRemover(
    IElementList elements,
    ITiles tiles,
    IActorList actors,
    IBoardUpdater boardUpdater,
    IEngineAccessor engine,
    IState state)
    : ITileRemover
{
    private IEngine Engine => engine.Instance;

    public void RemoveItem(Location location)
    {
        var result = new Tile(elements.FloorId, 0x00);

        for (var i = 0; i < 4; i++)
        {
            var targetVector = state.GetCardinalVector(i);
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
}