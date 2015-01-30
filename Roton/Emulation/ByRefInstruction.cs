using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
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
