using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Core
{
    public interface ITileGrid : IList<ITile>
    {
        ITile this[IXyPair location] { get; }
        int Height { get; }
        int Width { get; }
    }
}
