using Roton.Emulation.Data;

namespace Roton.Emulation.Commands;

public interface ICommand
{
    void Execute(ref OopContext context, ref Word instruction);
}