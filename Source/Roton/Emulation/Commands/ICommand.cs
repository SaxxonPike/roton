using Roton.Emulation.Data;

namespace Roton.Emulation.Commands;

public interface ICommand
{
    void Execute(IOopContext context);
}