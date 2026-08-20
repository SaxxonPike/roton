using Roton.Emulation.Data;

namespace Roton.Emulation.Conditions;

public interface ICondition
{
    bool? Execute(ref OopContext context, ref Word instruction);
}