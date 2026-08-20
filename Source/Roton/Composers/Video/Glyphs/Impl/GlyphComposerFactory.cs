using System;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Composers.Video.Glyphs.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class GlyphComposerFactory(IComposerResourceService composerResourceService) : IGlyphComposerFactory
{
    public IGlyphComposer Get(ReadOnlyMemory<byte> data, bool wide)
    {
        IGlyphComposer result =
            new AutoDetectBinaryGlyphComposer(data.IsEmpty ? composerResourceService.GetFontData() : data);

        if (wide)
            result = new ScaledGlyphComposer(result, 2, 1);

        return new CachedGlyphComposer(result);
    }
}