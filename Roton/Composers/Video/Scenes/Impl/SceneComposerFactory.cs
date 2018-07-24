using Roton.Composers.Video.Glyphs;
using Roton.Composers.Video.Palettes;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Composers.Video.Scenes.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class SceneComposerFactory : ISceneComposerFactory
    {
        private readonly IGlyphComposerFactory _glyphComposerFactory;
        private readonly IPaletteComposerFactory _paletteComposerFactory;

        public SceneComposerFactory(
            IGlyphComposerFactory glyphComposerFactory,
            IPaletteComposerFactory paletteComposerFactory)
        {
            _glyphComposerFactory = glyphComposerFactory;
            _paletteComposerFactory = paletteComposerFactory;
        }

        public ISceneComposer Get()
        {
            return new SceneComposer(_paletteComposerFactory, _glyphComposerFactory);
        }
    }
}