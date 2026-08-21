using System.Collections.Generic;
using System.Linq;

namespace Roton.Composers.Video.Glyphs;

public static class GlyphComposerExtensions
{
    public static IReadOnlyList<Glyph?> ComposeAllGlyphs(this IGlyphComposer composer) => 
        [.. Enumerable.Range(0, 256).Select(composer.ComposeGlyph)];
}