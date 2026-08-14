namespace Roton.Composers.Video.Glyphs;

/// <summary>
/// Interface for getting 32-bit two-dimensional bitmaps for use as character glyphs.
/// </summary>
public interface IGlyphComposer
{
    /// <summary>
    /// Render a glyph bitmap.
    /// </summary>
    IGlyph ComposeGlyph(int index);

    int MaxWidth { get; }

    int MaxHeight { get; }
}