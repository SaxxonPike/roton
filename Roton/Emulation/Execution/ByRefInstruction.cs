using Roton.Core;

namespace Roton.Emulation.Execution
{
    internal sealed class ByRefInstruction : IExecutable
    {
        public ByRefInstruction() : this(0)
        {
        }

        public ByRefInstruction(int initialValue)
        {
            Instruction = initialValue;
        }

        public int Instruction { get; set; }
    }
}