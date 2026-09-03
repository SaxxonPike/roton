namespace Roton.Emulation.Data;

public struct ScrollResult
{
    public int Index { get; set; }
    public string? Label { get; set; }
    public bool Cancelled { get; set; }
    public bool Shown { get; set; }
}