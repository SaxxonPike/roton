using System.Collections.Generic;
using Roton.Composers.Extensions;

namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class CachedGlyphComposer(IGlyphComposer source) : IGlyphComposer
{
    private readonly IReadOnlyList<IGlyph> _glyphs = source.ComposeAllGlyphs();

    public IGlyph ComposeGlyph(int index)
    {
        return _glyphs[index];
    }

    public int MaxWidth { get; } = source.MaxWidth;
    public int MaxHeight { get; } = source.MaxHeight;
}