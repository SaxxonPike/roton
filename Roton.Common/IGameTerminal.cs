using Roton.Core;

namespace Roton.Common
{
    public interface IGameTerminal : ITerminal
    {
        RasterFont TerminalFont { get; set; }
        Palette TerminalPalette { get; set; }
    }
}