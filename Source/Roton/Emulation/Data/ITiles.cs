using System.Collections.Generic;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data;

public interface ITiles : IEnumerable<Tile>
{
    int Height { get; }
    ref Tile this[Location location] { get; }
    int Width { get; }
    bool FindTile(Tile kind, ref Location location);
    IElement ElementAt(Location location);
}