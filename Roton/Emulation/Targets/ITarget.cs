using Roton.Emulation.Mapping;

namespace Roton.Emulation.Targets
{
    public interface ITarget
    {
        string Name { get; }
        bool Execute(ISearchContext context);
    }
}