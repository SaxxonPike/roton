using System.Drawing;

namespace Roton.Interface
{
    public interface IPalette
    {
        int[] Colors { get; }
        int Count { get; }
        Color this[int index] { get; }
    }
}