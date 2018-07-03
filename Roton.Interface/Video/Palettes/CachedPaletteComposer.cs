using System.Drawing;
using System.Linq;
using OpenTK.Graphics;
using Roton.Interface.Extensions;

namespace Roton.Interface.Video.Palettes
{
    public class CachedPaletteComposer : IPaletteComposer
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
