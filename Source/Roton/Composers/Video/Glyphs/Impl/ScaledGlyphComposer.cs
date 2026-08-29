namespace Roton.Composers.Video.Glyphs.Impl;

internal sealed class ScaledGlyphComposer(IGlyphComposer glyphComposer, int scaleX, int scaleY) : IGlyphComposer
{
    public Glyph? ComposeGlyph(int index)
    {
        var glyph = glyphComposer.ComposeGlyph(index);
        if (glyph == null)
            return null;

        var bytes = glyph.Data.Span;
        var scaledRow = (stackalloc int[glyph.Width*scaleX]);
        var scaledData = (stackalloc int[glyph.Data.Length*scaleX*scaleY]);
        var src = 0;
        var dest = 0;
        var destBase = 0;

        for (var y = 0; y < glyph.Height; y++)
        {
            dest = 0;

            for (var x = 0; x < glyph.Width; x++)
            {
                var bits = bytes[src++];
                for (var i = 0; i < scaleX; i++)
                    scaledRow[dest++] = bits;
            }

            for (var i = 0; i < scaleY; i++)
            {
                scaledRow.CopyTo(scaledData.Slice(destBase));
                destBase += scaledRow.Length;
            }
        }

        return new Glyph(index, glyph.Width * scaleX, glyph.Height * scaleY, scaledData.ToArray());
    }

    public int MaxWidth { get; } = glyphComposer.MaxWidth*scaleX;
    public int MaxHeight { get; } = glyphComposer.MaxHeight*scaleY;
}