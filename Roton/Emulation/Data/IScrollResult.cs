namespace Roton.Emulation.Data
{
    public interface IScrollResult
    {
        int Index { get; }
        string Label { get; }
        bool Cancelled { get; }
    }
}