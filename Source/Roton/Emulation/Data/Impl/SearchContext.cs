namespace Roton.Emulation.Data.Impl
{
    public sealed class SearchContext : ISearchContext
    {
        internal SearchContext()
        {
        }

        public int SearchIndex { get; set; }
        public int SearchOffset { get; set; }
    }
}