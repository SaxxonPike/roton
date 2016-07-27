using Roton.Core;

namespace Roton.Emulation.Mapping
{
    public interface ISearchContext
    {
        IEngine Engine { get; }
        int SearchIndex { get; set; }
        int SearchOffset { get; set; }
        string SearchTarget { get; set; }
    }
}