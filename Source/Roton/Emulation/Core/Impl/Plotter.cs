using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Plotter(
    ITiles tiles,
    IElementList elements,
    ISpawner spawner,
    IState state,
    IBoardUpdater boardUpdater,
    IDeferred<IPusher> pusher,
    IDeferred<IAttacker> attacker)
    : IPlotter
{
    private ITiles _tiles = tiles;

    public void Plot(Location location, Tile tile)
    {
        if (_tiles.ElementAt(location).Id == elements.PlayerId)
            return;

        var targetElement = elements[tile.Id];
        ref var existingTile = ref _tiles[location];
        var targetColor = tile.Color;
        if (targetElement.Color >= 0xF0)
        {
            if (targetColor == 0)
                targetColor = existingTile.Color;
            if (targetColor == 0)
                targetColor = 0x0F;
            if (targetElement.Color == 0xFE)
                targetColor = ((targetColor - 8) << 4) + 0x0F;
        }
        else
        {
            targetColor = targetElement.Color;
        }

        if (targetElement.Id == existingTile.Id)
        {
            existingTile.Color = targetColor;
        }
        else
        {
            attacker.Instance.Destroy(location);
            if (targetElement.Cycle < 0)
                existingTile = new Tile(targetElement.Id, targetColor);
            else
                spawner.SpawnActor(location, new Tile(targetElement.Id, targetColor), targetElement.Cycle,
                    state.DefaultActor);
        }

        boardUpdater.UpdateBoard(location);
    }

    public void Put(Location location, Vector vector, Tile kind)
    {
        if (!_tiles.CanPutTile(location))
            return;

        if (location.X >= 1 && location.X <= _tiles.Width && location.Y >= 1 &&
            location.Y <= _tiles.Height)
        {
            if (!_tiles.ElementAt(location).IsFloor)
                pusher.Instance.Push(location, vector);
            Plot(location, kind);
        }
    }
}