using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    internal class ExecuteCodeContext : ICodeSeekable
    {
        private ICodeSeekable _instructionSource;

        public ExecuteCodeContext(int index, ICodeSeekable instructionSource, string name)
        {
            _instructionSource = instructionSource;
            this.Index = index;
            this.Name = name;
        }

        public int Index
        {
            get;
            set;
        }

        public int Instruction
        {
            get { return _instructionSource.Instruction; }
            set { _instructionSource.Instruction = value; }
        }

        public string Name
        {
            get;
            set;
        }
    }
}
