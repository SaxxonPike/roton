using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core;

public interface IInterpreter
{
    void Execute(ref OopContext context, ref Word instruction);
}