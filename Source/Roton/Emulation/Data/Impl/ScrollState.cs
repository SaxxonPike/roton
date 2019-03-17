using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl
{
    public sealed class ScrollState : IScrollState
    {
        public string Title { get; set; }
        public bool IsHelp { get; set; }
        public int Index { get; set; }
        public string Label { get; set; }
        public bool Cancelled { get; set; }
        public IList<string> Lines { get; set; }
    }
}