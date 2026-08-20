using System;

namespace Roton.Composers.Video.Glyphs;

public interface IGlyphComposerFactory
{
    IGlyphComposer Get(ReadOnlyMemory<byte> data, bool wide);
}