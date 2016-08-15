using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Roton.Interface.Video.Palettes
{
    public interface IPaletteComposer
    {
        Color ComposeColor(int index);
    }
}
