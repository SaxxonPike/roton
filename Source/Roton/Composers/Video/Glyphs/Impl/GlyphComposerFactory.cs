using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Composers.Video.Glyphs.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class GlyphComposerFactory : IGlyphComposerFactory
{
    private readonly Lazy<IComposerResourceService> _composerResourceService;

    public GlyphComposerFactory(Lazy<IComposerResourceService> composerResourceService)
    {
        _composerResourceService = composerResourceService;
    }

    public IGlyphComposer Get(byte[] data, bool wide)
    {
        IGlyphComposer result =
            new AutoDetectBinaryGlyphComposer(data ?? _composerResourceService.Value.GetFontData());

        if (wide)
            result = new ScaledGlyphComposer(result, 2, 1);

        return new CachedGlyphComposer(result);
    }
}