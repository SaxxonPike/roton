using Roton.Core;

namespace Roton.Emulation.Conditions
{
    public interface ICondition
    {
        string Name { get; }
        bool? Execute(IOopContext context);
    }
}