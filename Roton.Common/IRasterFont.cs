using System.Collections.Generic;
using System.Drawing;

namespace Roton.Common
{
    public interface IRasterFont
    {
        IGlyph this[int index] { get; }
        int Count { get; }
        int Height { get; }
        int OriginalHeight { get; }
        int OriginalWidth { get; }
        int Width { get; }

        Bitmap Render(int character, Color foreground, Color background);
        void Render(IFastBitmap bitmap, int x, int y, int character, int foreColor, int backColor);
        Bitmap RenderUnscaled(int character, Color foreground, Color background);
        void SetScale(int xScale, int yScale);
    }
}