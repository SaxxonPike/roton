using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztBoard : IBoard
    {
        private readonly IMemory _memory;

        public SuperZztBoard(IMemory memory)
        {
            _memory = memory;
        }

        public IXyPair Camera => new MemoryLocation16(_memory, 0x776F);

        public IXyPair Entrance => new MemoryLocation(_memory, 0x776D);

        public int ExitEast
        {
            get { return _memory.Read8(0x776B); }
            set { _memory.Write8(0x776B, value); }
        }

        public int ExitNorth
        {
            get { return _memory.Read8(0x7768); }
            set { _memory.Write8(0x7768, value); }
        }

        public int ExitSouth
        {
            get { return _memory.Read8(0x7769); }
            set { _memory.Write8(0x7769, value); }
        }

        public int ExitWest
        {
            get { return _memory.Read8(0x776A); }
            set { _memory.Write8(0x776A, value); }
        }

        public bool IsDark
        {
            get { return false; }
            set { }
        }

        public int MaximumShots
        {
            get { return _memory.Read8(0x7767); }
            set { _memory.Write8(0x7767, value); }
        }

        public string Name
        {
            get { return _memory.ReadString(0x2BAE); }
            set { _memory.WriteString(0x2BAE, value); }
        }

        public bool RestartOnZap
        {
            get { return _memory.ReadBool(0x776C); }
            set { _memory.WriteBool(0x776C, value); }
        }

        public int TimeLimit
        {
            get { return _memory.Read16(0x7773); }
            set { _memory.Write16(0x7773, value); }
        }
    }
}