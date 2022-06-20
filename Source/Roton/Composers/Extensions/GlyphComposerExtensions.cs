using System.Collections.Generic;
using System.Linq;
using Roton.Composers.Video.Glyphs;

namespace Roton.Composers.Extensions;

public static class GlyphComposerExtensions
{
    public static IReadOnlyList<IGlyph> ComposeAllGlyphs(this IGlyphComposer composer)
    {
        return Enumerable.Range(0, 256).Select(composer.ComposeGlyph).ToArray();
    }
}