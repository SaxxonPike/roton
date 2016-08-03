using System.Drawing;

namespace Roton.Interface.Video
{
    public interface IGlyph
    {
        int[] Data { get; }
        int Height { get; }
        int this[int index] { get; }
        int Width { get; }

        int[] Render(int foreColor, int backColor);
        Bitmap Render(Color foreColor, Color backColor);
        void Render(IFastBitmap bitmap, int x, int y, int foreColor, int backColor);
    }
}