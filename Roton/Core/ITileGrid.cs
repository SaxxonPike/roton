using System.Collections.Generic;

namespace Roton.Core
{
    public interface ITileGrid : IEnumerable<ITile>
    {
        int Height { get; }
        ITile this[int index] { get; }
        ITile this[IXyPair location] { get; }
        int Width { get; }
    }
}