namespace Roton.Interface.Video.Glyphs
{
    /// <summary>
    /// Interface for getting 32-bit two-dimensional bitmaps for use as character glyphs.
    /// </summary>
    public interface IGlyphComposer
    {
        /// <summary>
        /// Render a glyph bitmap.
        /// </summary>
        IComposedGlyph ComposeGlyph(int index);
    }
}
