using System.Collections;
using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

public abstract class Tiles(IMemory memory, IElementList elementList, int offset, int width, int height)
    : ITiles
{
    public int Count =>
        TotalWidth * TotalHeight;

    private int TotalHeight =>
        Height + 2;

    private int TotalWidth =>
        Width + 2;

    public int Height { get; } = height;

    public ref Tile this[Location location] =>
        ref this[location.X * TotalHeight + location.Y];

    public ref Tile this[int index] =>
        ref memory.GetRef<Tile>(offset + index * 2);

    public int Width { get; } = width;

    private IEnumerable<Tile> GetTileEnumerable()
    {
        for (var x = 1; x < TotalWidth - 1; x++)
        for (var y = 1; y < TotalHeight - 1; y++)
            yield return this[new Location(x, y)];
    }

    public IEnumerator<Tile> GetEnumerator() =>
        GetTileEnumerable().GetEnumerator();

    public override string ToString() =>
        $"TileGrid ({Width}x{Height})";

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IElement ElementAt(Location location) =>
        elementList[this[location].Id];

    public bool FindTile(Tile kind, ref Location location)
    {
        location.X++;
        while (location.Y <= Height)
        {
            while (location.X <= Width)
            {
                var tile = this[location];
                if (tile.Id == kind.Id)
                {
                    if (kind.Color == 0 || ColorMatch(this[location]) == kind.Color)
                    {
                        return true;
                    }
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
        var element = elementList[tile.Id];

        if (element.Color < 0xF0) return element.Color & 7;
        if (element.Color == 0xFE) return ((tile.Color >> 4) & 0x0F) + 8;
        return tile.Color & 0x0F;
    }
}