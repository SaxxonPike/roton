using Roton.Core;

namespace Roton.Emulation.Execution
{
    internal class ByRefInstruction : ICodeSeekable
    {
        public ByRefInstruction()
        {
            Instruction = 0;
        }

        public ByRefInstruction(int initialValue)
        {
            Instruction = initialValue;
        }

        public int Instruction { get; set; }
    }
}