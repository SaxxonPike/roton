using System.Drawing;

namespace Roton.Composers.Video.Palettes.Impl;

internal sealed class CachedPaletteComposer(IPaletteComposer paletteComposer) : IPaletteComposer
{
    private readonly Color[] _colors = [.. paletteComposer.ComposeAllColors()];

    public Color ComposeColor(int index)
    {
        return _colors[index];
    }
}