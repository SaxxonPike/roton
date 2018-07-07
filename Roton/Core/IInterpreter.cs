using Roton.Emulation.Mapping;

namespace Roton.Core
{
    public interface IInterpreter
    {
        void Cheat(string input);
        void Execute(IOopContext context);
    }
}