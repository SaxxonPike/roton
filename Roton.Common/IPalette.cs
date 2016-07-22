using System.Collections.Generic;
using System.Drawing;

namespace Roton.Common
{
    public interface IPalette
    {
        Color this[int index] { get; }
        int[] Colors { get; }
        int Count { get; }
    }
}