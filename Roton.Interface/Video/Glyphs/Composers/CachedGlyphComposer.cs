using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Interface.Extensions;

namespace Roton.Interface.Video.Glyphs.Composers
{
    public class CachedGlyphComposer : IGlyphComposer
    {
        private readonly IList<IComposedGlyph> _glyphs;

        public CachedGlyphComposer(IGlyphComposer source)
        {
            _glyphs = source.ComposeAllGlyphs();
        }

        public IComposedGlyph ComposeGlyph(int index)
        {
            return _glyphs[index];
        }
    }
}
