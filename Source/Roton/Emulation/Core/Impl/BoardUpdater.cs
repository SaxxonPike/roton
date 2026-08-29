using Roton.Emulation.Data;
using Roton.Emulation.Draws;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class BoardUpdater(
    IDrawList drawList,
    IElementList elementList,
    IActorList actorList,
    IFacts facts,
    IBoard board,
    IWorld world,
    IState state,
    ITiles tiles,
    IPlayField playField)
    : IBoardUpdater
{
    private ITiles _tiles = tiles;

    private static int Distance(Location a, Location b) =>
        (a.Y - b.Y).Square() * 2 + (a.X - b.X).Square();

    public void UpdateBoard(Location location) =>
        playField.DrawTile(location.X - 1, location.Y - 1, Draw(location));

    public AnsiChar Draw(Location location)
    {
        if (board.IsDark && !_tiles.ElementAt(location).IsAlwaysVisible &&
            (world.TorchCycles <= 0 || Distance(actorList.Player.Location, location) >= facts.TorchRadius) &&
            !state.EditorMode)
            return facts.DarknessTile;

        ref var tile = ref _tiles[location];
        var element = elementList[tile.Id];
        var elementCount = elementList.Count;

        if (tile.Id == elementList.EmptyId)
            return facts.EmptyTile;

        if (element.HasDrawCode)
            return drawList.Get(tile.Id)?.Draw(location) ?? new AnsiChar(0x4F, 0x41);

        if (tile.Id < elementCount - 7)
            return new AnsiChar(element.Character, tile.Color);

        return tile.Id != elementCount - 1
            ? new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F)
            : new AnsiChar(tile.Color, 0x0F);
    }
}