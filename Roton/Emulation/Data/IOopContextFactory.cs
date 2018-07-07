using Roton.Core;

namespace Roton.Emulation.Execution
{
    public interface IOopContextFactory
    {
        IOopContext Create(int index, IExecutable instructionSource, string name);
    }
}