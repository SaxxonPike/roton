using System.Drawing;

namespace Roton.Composers.Video.Palettes.Impl;

public sealed class VgaPaletteComposer(byte[] data) : IPaletteComposer
{
    public Color ComposeColor(int index)
    {
        var offset = (index & 0xF)*3;
        var red = (int) data[offset];
        var green = (int) data[offset + 1];
        var blue = (int) data[offset + 2];
        var adjustedRed = (red << 2) | (red >> 4);
        var adjustedGreen = (green << 2) | (green >> 4);
        var adjustedBlue = (blue << 2) | (blue >> 4);
        return Color.FromArgb(adjustedRed, adjustedGreen, adjustedBlue);
    }
}