using System.Collections.Generic;
using System.Linq;
using Roton.Interface.Video.Glyphs;

namespace Roton.Interface.Extensions
{
    public static class GlyphComposerExtensions
    {
        public static IList<IComposedGlyph> ComposeAllGlyphs(this IGlyphComposer composer)
        {
            return Enumerable.Range(0, 256).Select(composer.ComposeGlyph).ToArray();
        }
    }
}
