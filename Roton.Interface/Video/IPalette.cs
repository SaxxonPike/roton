using System.Drawing;

namespace Roton.Interface.Video
{
    public interface IPalette
    {
        int[] Colors { get; }
        int Count { get; }
        Color this[int index] { get; }
    }
}