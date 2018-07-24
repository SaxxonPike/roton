using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Composers.Video.Palettes.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class PaletteComposerFactory : IPaletteComposerFactory
    {
        public IPaletteComposer Get(byte[] data)
        {
            var result = new VgaPaletteComposer(data);
            return new CachedPaletteComposer(result);
        }
    }
}