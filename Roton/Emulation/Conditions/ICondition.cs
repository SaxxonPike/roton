using Roton.Emulation.Data;

namespace Roton.Emulation.Conditions
{
    public interface ICondition
    {
        bool? Execute(IOopContext context);
    }
}