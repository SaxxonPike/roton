using System.Collections;
using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Tiles(IMemory memory, IElementList elements, IFacts facts)
    : ITiles
{
    private int TotalHeight =>
        Height + 2;

    public int Height => facts.BoardHeight;

    public ref Tile this[Location location] =>
        ref this[location.X * TotalHeight + location.Y];

    public ref Tile this[int index] =>
        ref memory.GetRef<Tile>(facts.TileOffset + index * 2);

    public int Width => facts.BoardWidth;

    public override string ToString() =>
        $"TileGrid ({Width}x{Height})";

    public IElement ElementAt(Location location) =>
        elements[this[location].Id];

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
        var element = elements[tile.Id];

        if (element.Color < 0xF0) return element.Color & 7;
        if (element.Color == 0xFE) return ((tile.Color >> 4) & 0x0F) + 8;
        return tile.Color & 0x0F;
    }

    public bool CanPutTile(Location location) =>
        location.X >= 1 && location.X <= facts.BoardWidth && location.Y >= 1 &&
        location.Y <= facts.MaxPutRow;
}