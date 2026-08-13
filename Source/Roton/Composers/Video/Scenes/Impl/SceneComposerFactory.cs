using Roton.Composers.Video.Glyphs;
using Roton.Composers.Video.Palettes;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Composers.Video.Scenes.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
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
        var composer = new SceneComposer(_paletteComposerFactory, _glyphComposerFactory);
        composer.SetSize(80, 25, false);
        return composer;
    }
}