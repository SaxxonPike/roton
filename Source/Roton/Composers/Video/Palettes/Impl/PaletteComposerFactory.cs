using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Composers.Video.Palettes.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class PaletteComposerFactory(IComposerResourceService composerResourceService)
    : IPaletteComposerFactory
{
    public IPaletteComposer Get(byte[] data)
    {
        var result = new VgaPaletteComposer(data ?? composerResourceService.GetPaletteData());
        return new CachedPaletteComposer(result);
    }
}