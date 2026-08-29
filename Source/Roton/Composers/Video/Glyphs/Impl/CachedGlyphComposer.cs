using System.Collections.Generic;

namespace Roton.Composers.Video.Glyphs.Impl;

internal sealed class CachedGlyphComposer(IGlyphComposer source) : IGlyphComposer
{
    private readonly IReadOnlyList<Glyph?> _glyphs = source.ComposeAllGlyphs();

    public Glyph? ComposeGlyph(int index) => 
        _glyphs[index];

    public int MaxWidth { get; } = source.MaxWidth;
    public int MaxHeight { get; } = source.MaxHeight;
}