using System.Drawing;
using System.Linq;
using OpenTK.Graphics;
using Roton.Interface.Extensions;

namespace Roton.Interface.Video.Palettes
{
    public class CachedPaletteComposer : IPaletteComposer
    {
        private readonly Color4[] _colors;

        public CachedPaletteComposer(IPaletteComposer paletteComposer)
        {
            _colors = paletteComposer.ComposeAllColors().ToArray();
        }

        public Color4 ComposeColor(int index)
        {
            return _colors[index];
        }
    }
}
