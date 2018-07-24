using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Composers.Video.Palettes.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class PaletteComposerFactory : IPaletteComposerFactory
    {
        private readonly IComposerResourceService _composerResourceService;

        public PaletteComposerFactory(IComposerResourceService composerResourceService)
        {
            _composerResourceService = composerResourceService;
        }
        
        public IPaletteComposer Get(byte[] data)
        {
            var result = new VgaPaletteComposer(data ?? _composerResourceService.GetPaletteData());
            return new CachedPaletteComposer(result);
        }
    }
}