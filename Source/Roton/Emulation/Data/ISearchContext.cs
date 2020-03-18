namespace Roton.Emulation.Data
{
    public interface ISearchContext
    {
        int SearchIndex { get; set; }
        int SearchOffset { get; set; }
    }
}