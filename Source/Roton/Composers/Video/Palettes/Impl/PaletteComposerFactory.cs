using System;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Composers.Video.Palettes.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class PaletteComposerFactory(IComposerResourceService composerResourceService)
    : IPaletteComposerFactory
{
    public IPaletteComposer Get(ReadOnlyMemory<byte> data)
    {
        var result = new VgaPaletteComposer(data.IsEmpty ? composerResourceService.GetPaletteData() : data);
        return new CachedPaletteComposer(result);
    }
}