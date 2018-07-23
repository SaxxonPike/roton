namespace Roton.Emulation.Data.Impl
{
    public sealed class ScrollResult : IScrollResult
    {
        public int Index { get; set; }
        public string Label { get; set; }
        public bool Cancelled { get; set; }
    }
}