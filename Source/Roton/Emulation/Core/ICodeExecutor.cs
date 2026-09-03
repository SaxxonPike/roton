using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ICodeExecutor
{
    /// <remarks>
    /// RoZ: OopExecute
    /// </remarks>
    void ExecuteCode(int index, ref Word instruction, string name);
}