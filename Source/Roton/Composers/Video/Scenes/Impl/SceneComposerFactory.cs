using Roton.Composers.Video.Glyphs;
using Roton.Composers.Video.Palettes;
using Roton.Infrastructure;

namespace Roton.Composers.Video.Scenes.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class SceneComposerFactory(
    IGlyphComposerFactory glyphComposerFactory,
    IPaletteComposerFactory paletteComposerFactory)
    : ISceneComposerFactory
{
    public ISceneComposer Get()
    {
        var composer = new SceneComposer(paletteComposerFactory, glyphComposerFactory);
        composer.SetSize(80, 25, false);
        return composer;
    }
}