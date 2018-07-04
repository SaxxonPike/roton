using System.Collections.Generic;

namespace Roton.Core
{
    public interface IGrid : IEnumerable<ITile>
    {
        int Height { get; }
        ITile this[int index] { get; }
        ITile this[IXyPair location] { get; }
        int Width { get; }

        IElement ElementAt(IXyPair location);
    }
}