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

        public Actor Actor
        {
            get;
            set;
        }

        public int CommandsExecuted
        {
            get;
            set;
        }

        public Tile DeathTile
        {
            get;
            set;
        }

        public bool Died
        {
            get;
            set;
        }

        public bool Finished
        {
            get;
            set;
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

        public string Message
        {
            get;
            set;
        }

        public bool Moved
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool NextLine
        {
            get;
            set;
        }

        public int PreviousInstruction
        {
            get;
            set;
        }

        public bool Repeat
        {
            get;
            set;
        }
    }
}
