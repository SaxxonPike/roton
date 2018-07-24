using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Composers.Video.Glyphs.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class GlyphComposerFactory : IGlyphComposerFactory
    {
        private readonly IComposerResourceService _composerResourceService;

        public GlyphComposerFactory(IComposerResourceService composerResourceService)
        {
            _composerResourceService = composerResourceService;
        }
        
        public IGlyphComposer Get(byte[] data, int scaleX, int scaleY)
        {
            IGlyphComposer result = new AutoDetectBinaryGlyphComposer(data ?? _composerResourceService.GetFontData());
            
            if (scaleX > 1 || scaleY > 1)
                result = new ScaledGlyphComposer(result, scaleX, scaleY);
            
            return new CachedGlyphComposer(result);
        }
    }
}