using System;
using System.Drawing;

namespace Roton.Composers.Video.Palettes.Impl;

internal sealed class VgaPaletteComposer(ReadOnlyMemory<byte> data) : IPaletteComposer
{
    public Color ComposeColor(int index)
    {
        var span = data.Span;
        var offset = (index & 0xF) * 3;
        var red = (int)span[offset];
        var green = (int)span[offset + 1];
        var blue = (int)span[offset + 2];
        var adjustedRed = (red << 2) | (red >> 4);
        var adjustedGreen = (green << 2) | (green >> 4);
        var adjustedBlue = (blue << 2) | (blue >> 4);
        return Color.FromArgb(adjustedRed, adjustedGreen, adjustedBlue);
    }
}