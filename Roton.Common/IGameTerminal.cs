namespace Roton.Common {
    public interface IGameTerminal : ITerminal
    {
        Font TerminalFont { get; set; }
        Palette TerminalPalette { get; set; }
    }
}
