using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IInterpreter
{
    /// <remarks>
    /// RoZ: OopExecute:ReadCommand
    /// </remarks>
    void Execute(ref OopContext context, ref Word instruction);
}