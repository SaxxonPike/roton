namespace Roton.Emulation.Data
{
    public interface ISearchContext
    {
        int Index { get; set; }
        int SearchIndex { get; set; }
        int SearchOffset { get; set; }
        string SearchTarget { get; set; }
    }
}