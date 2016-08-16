using System.Collections.Generic;
using Roton.Interface.Extensions;

namespace Roton.Interface.Video.Glyphs
{
    public class CachedGlyphComposer : IGlyphComposer
    {
        private readonly IList<IGlyph> _glyphs;

        public CachedGlyphComposer(IGlyphComposer source)
        {
            _glyphs = source.ComposeAllGlyphs();
        }

        public IGlyph ComposeGlyph(int index)
        {
            return _glyphs[index];
        }
    }
}
