using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Commands;

public interface ICommand
{
    void Execute(ref OopContext context, ref Word instruction);
}