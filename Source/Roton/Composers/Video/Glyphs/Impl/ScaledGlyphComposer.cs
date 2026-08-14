using System.Linq;

namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class ScaledGlyphComposer(IGlyphComposer glyphComposer, int scaleX, int scaleY) : IGlyphComposer
{
    public IGlyph ComposeGlyph(int index)
    {
        var glyph = glyphComposer.ComposeGlyph(index);
        var scaledXScan = glyph.Data.SelectMany(pixel => Enumerable.Repeat(pixel, scaleX));
        var scaledY = scaledXScan.SelectMany(scan => Enumerable.Repeat(scan, scaleY));
        return new Glyph(index, glyph.Width * scaleX, glyph.Height * scaleY, scaledY);
    }

    public int MaxWidth { get; } = glyphComposer.MaxWidth*scaleX;
    public int MaxHeight { get; } = glyphComposer.MaxHeight*scaleY;
}