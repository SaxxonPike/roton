using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Roton.Composers.Video.Palettes;

namespace Roton.Composers.Extensions
{
    public static class PaletteComposerExtensions
    {
        public static IEnumerable<Color> ComposeAllColors(this IPaletteComposer composer)
        {
            return Enumerable.Range(0, 16).Select(composer.ComposeColor);
        }
    }
}
