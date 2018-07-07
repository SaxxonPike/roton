namespace Roton.Emulation.Mapping
{
    public class SearchContext : ISearchContext
    {
        public int SearchIndex { get; set; }
        public int SearchOffset { get; set; }
        public string SearchTarget { get; set; }
    }
}