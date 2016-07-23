using Roton.Core;

namespace Roton.Common
{
    public interface IGameTerminal : ITerminal
    {
        IKeyboard Keyboard { get; }
        bool AutoSize { get; set; }
        int Height { get; set; }
        int Left { get; set; }
        IRasterFont TerminalFont { get; set; }
        IPalette TerminalPalette { get; set; }
        int Top { get; set; }
        bool Visible { get; set; }
        int Width { get; set; }
        void SetScale(int xScale, int yScale);
    }
}