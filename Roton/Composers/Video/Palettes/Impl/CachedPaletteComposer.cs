using System.Drawing;
using System.Linq;
using Roton.Composers.Extensions;

namespace Roton.Composers.Video.Palettes.Impl
{
    public sealed class CachedPaletteComposer : IPaletteComposer
    {
        private readonly Color[] _colors;

        public CachedPaletteComposer(IPaletteComposer paletteComposer)
        {
            _colors = paletteComposer.ComposeAllColors().ToArray();
        }

        public Color ComposeColor(int index)
        {
            return _colors[index];
        }
    }
}
