using Roton.Emulation.Data;

namespace Roton.Emulation.Commands
{
    public interface ICommand
    {
        string Name { get; }
        void Execute(IOopContext context);
    }
}