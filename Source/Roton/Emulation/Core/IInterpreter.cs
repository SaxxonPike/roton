using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IInterpreter
{
    void Execute(IOopContext context);
}