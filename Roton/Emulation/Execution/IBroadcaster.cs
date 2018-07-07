using Roton.Emulation.Mapping;

namespace Roton.Emulation.Execution
{
    public interface IBroadcaster
    {
        bool BroadcastLabel(int sender, string label, bool force);
        bool ExecuteLabel(int sender, ISearchContext context, string prefix);
    }
}