using Roton.Emulation.Data;

namespace Roton.Emulation.Conditions
{
    public interface ICondition
    {
        string Name { get; }
        bool? Execute(IOopContext context);
    }
}