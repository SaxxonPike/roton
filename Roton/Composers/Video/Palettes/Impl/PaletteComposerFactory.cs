using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Composers.Video.Palettes.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class PaletteComposerFactory : IPaletteComposerFactory
    {
        private readonly Lazy<IComposerResourceService> _composerResourceService;

        public PaletteComposerFactory(Lazy<IComposerResourceService> composerResourceService)
        {
            _composerResourceService = composerResourceService;
        }
        
        public IPaletteComposer Get(byte[] data)
        {
            var result = new VgaPaletteComposer(data ?? _composerResourceService.Value.GetPaletteData());
            return new CachedPaletteComposer(result);
        }
    }
}