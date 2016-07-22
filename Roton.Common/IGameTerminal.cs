using Roton.Core;

namespace Roton.Common
{
    public interface IGameTerminal : ITerminal
    {
        IRasterFont TerminalFont { get; set; }
        IPalette TerminalPalette { get; set; }
    }
}