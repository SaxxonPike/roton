using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface ITiles : IEnumerable<ITile>
{
    int Height { get; }
    ITile this[int index] { get; }
    ITile this[IXyPair location] { get; }
    int Width { get; }
    bool FindTile(ITile kind, IXyPair location);
    IElement ElementAt(IXyPair location);
}