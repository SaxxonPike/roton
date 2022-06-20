using Roton.Emulation.Data;

namespace Roton.Emulation.Targets;

public interface ITarget
{
    bool Execute(ISearchContext context);
}