namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class ScaledGlyphComposer(IGlyphComposer glyphComposer, int scaleX, int scaleY) : IGlyphComposer
{
    public IGlyph? ComposeGlyph(int index)
    {
        var glyph = glyphComposer.ComposeGlyph(index);
        if (glyph == null)
            return null;

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
                var bits = glyph.Data[src++];
                for (var i = 0; i < scaleX; i++)
                    scaledRow[dest++] = bits;
            }

            for (var i = 0; i < scaleY; i++)
            {
                scaledRow.CopyTo(scaledData.Slice(destBase));
                destBase += scaledRow.Length;
            }
        }

        return new Glyph(index, glyph.Width * scaleX, glyph.Height * scaleY, scaledData);
    }

    public int MaxWidth { get; } = glyphComposer.MaxWidth*scaleX;
    public int MaxHeight { get; } = glyphComposer.MaxHeight*scaleY;
}