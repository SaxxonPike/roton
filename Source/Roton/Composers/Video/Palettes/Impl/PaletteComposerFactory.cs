using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Composers.Video.Palettes.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class PaletteComposerFactory(Lazy<IComposerResourceService> composerResourceService)
    : IPaletteComposerFactory
{
    public IPaletteComposer Get(byte[] data)
    {
        var result = new VgaPaletteComposer(data ?? composerResourceService.Value.GetPaletteData());
        return new CachedPaletteComposer(result);
    }
}