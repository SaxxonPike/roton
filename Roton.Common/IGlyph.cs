using System.Collections.Generic;
using System.Drawing;

namespace Roton.Common
{
    public interface IGlyph
    {
        int this[int index] { get; }
        int[] Data { get; }
        int Height { get; }
        int Width { get; }

        int[] Render(int foreColor, int backColor);
        Bitmap Render(Color foreColor, Color backColor);
        void Render(IFastBitmap bitmap, int x, int y, int foreColor, int backColor);
    }
}