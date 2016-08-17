using Roton.Core;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Palettes;

namespace Roton.Interface.Video.Controls
{
    public interface IGameTerminal : ITerminal
    {
        IKeyboard Keyboard { get; }
        bool AutoSize { get; set; }
        int Height { get; set; }
        int Left { get; set; }
        IGlyphComposer GlyphComposer { get; set; }
        IPaletteComposer PaletteComposer { get; set; }
        int Top { get; set; }
        bool Visible { get; set; }
        int Width { get; set; }
        void SetScale(int xScale, int yScale);
    }
}