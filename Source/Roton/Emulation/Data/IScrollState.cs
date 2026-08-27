namespace Roton.Emulation.Data;

public interface IScrollState
{
    string? Title { get; }
    bool IsHelp { get; set; }
    int Index { get; set; }
    string? Label { get; set; }
    bool Cancelled { get; set; }
}