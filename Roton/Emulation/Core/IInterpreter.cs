using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IInterpreter
    {
        void Cheat(string input);
        void Execute(IOopContext context);
    }
}