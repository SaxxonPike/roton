using System.Collections.Generic;

namespace Roton.Core
{
    public interface ITileGrid : IEnumerable<ITile>
    {
        ITile this[int index] { get; }
        ITile this[IXyPair location] { get; }
        int Height { get; }
        int Width { get; }
    }
}