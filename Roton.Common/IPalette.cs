using System.Drawing;

namespace Roton.Common
{
    public interface IPalette
    {
        int[] Colors { get; }
        int Count { get; }
        Color this[int index] { get; }
    }
}