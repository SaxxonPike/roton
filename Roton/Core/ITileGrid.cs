using System.Collections.Generic;

namespace Roton.Core
{
    public interface ITileGrid : IList<ITile>
    {
        ITile this[IXyPair location] { get; }
        int Height { get; }
        int Width { get; }
    }
}
