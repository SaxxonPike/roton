using Roton.Emulation.Conditions;

namespace Roton.Emulation.Commands
{
    public interface IConditions
    {
        ICondition Get(string name);
    }
}