namespace Roton.Emulation.Mapping
{
    public interface ISearchContext
    {
        int SearchIndex { get; set; }
        int SearchOffset { get; set; }
        string SearchTarget { get; set; }
    }
}