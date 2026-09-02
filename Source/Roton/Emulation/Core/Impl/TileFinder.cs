using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class TileFinder(
    IColorMatcher colorMatcher,
    ITiles tiles,
    IElementList elements)
    : ITileFinder
{
    private ITiles _tiles = tiles;

    public bool Find(Tile kind, Location location)
    {
        var matchColor = colorMatcher.GetColorMatchValue(kind.Color);

        location.X++;
        while (location.Y <= _tiles.Height)
        {
            while (location.X <= _tiles.Width)
            {
                ref var tile = ref _tiles[location];
                if (tile.Id == kind.Id)
                {
                    var foundColor = colorMatcher.GetColorMatchValue(ColorMatch(_tiles[location]));
                    if (kind.Color == 0 || foundColor == matchColor)
                        return true;
                }

                location.X++;
            }

            location.X = 1;
            location.Y++;
        }

        return false;
    }

    private int ColorMatch(Tile tile)
    {
        var element = elements[tile.Id];

        if (element.Color < 0xF0)
            return element.Color & 7;
        if (element.Color == 0xFE)
            return ((tile.Color >> 4) & 0x0F) + 8;
        return tile.Color & 0x0F;
    }
}