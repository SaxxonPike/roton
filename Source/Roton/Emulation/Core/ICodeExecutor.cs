using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ICodeExecutor
{
    void ExecuteCode(int index, ref Word instruction, string name);
}