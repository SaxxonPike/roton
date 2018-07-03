using System.Drawing;
using OpenTK.Graphics;

namespace Roton.Interface.Video.Palettes
{
    /// <summary>
    /// Interface for converting a color palette to 32-bit RGBA color values.
    /// </summary>
    public interface IPaletteComposer
    {
        /// <summary>
        /// Retrieve the 32-bit RGBA color for a specific color index in the palette.
        /// </summary>
        Color ComposeColor(int index);
    }
}
