namespace Roton.Emulation.Data;

public readonly struct ScrollResult(int index, string? label, bool cancelled, bool shown)
{
    public int Index => index;
    public string? Label => label;
    public bool Cancelled => cancelled;
    public bool Shown => shown;
}