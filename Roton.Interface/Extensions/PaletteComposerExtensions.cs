using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics;
using Roton.Interface.Video.Palettes;

namespace Roton.Interface.Extensions
{
    public static class PaletteComposerExtensions
    {
        public static IList<Color4> ComposeAllColors(this IPaletteComposer composer)
        {
            return Enumerable.Range(0, 16).Select(composer.ComposeColor).ToArray();
        }
    }
}
