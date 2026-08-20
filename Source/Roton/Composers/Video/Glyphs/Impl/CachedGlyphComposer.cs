using System.Collections.Generic;

namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class CachedGlyphComposer(IGlyphComposer source) : IGlyphComposer
{
    private readonly IReadOnlyList<IGlyph?> _glyphs = source.ComposeAllGlyphs();

    public IGlyph? ComposeGlyph(int index) => 
        _glyphs[index];

    public int MaxWidth { get; } = source.MaxWidth;
    public int MaxHeight { get; } = source.MaxHeight;
}