using System.Collections.Generic;
using Roton.Composers.Extensions;

namespace Roton.Composers.Video.Glyphs.Impl
{
    public class CachedGlyphComposer : IGlyphComposer
    {
        private readonly IList<IGlyph> _glyphs;

        public CachedGlyphComposer(IGlyphComposer source)
        {
            _glyphs = source.ComposeAllGlyphs();
            MaxWidth = source.MaxWidth;
            MaxHeight = source.MaxHeight;
        }

        public IGlyph ComposeGlyph(int index)
        {
            return _glyphs[index];
        }

        public int MaxWidth { get; }
        public int MaxHeight { get; }
    }
}
