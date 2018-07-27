using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperBoard : IBoard
    {
        private readonly IMemory _memory;

        public SuperBoard(IMemory memory)
        {
            _memory = memory;
            Camera = new MemoryLocation16(_memory, 0x776F);
            Entrance = new MemoryLocation(_memory, 0x776D);
        }

        public IXyPair Camera { get; }

        public IXyPair Entrance { get; }

        public int ExitEast
        {
            get => _memory.Read8(0x776B);
            set => _memory.Write8(0x776B, value);
        }

        public int ExitNorth
        {
            get => _memory.Read8(0x7768);
            set => _memory.Write8(0x7768, value);
        }

        public int ExitSouth
        {
            get => _memory.Read8(0x7769);
            set => _memory.Write8(0x7769, value);
        }

        public int ExitWest
        {
            get => _memory.Read8(0x776A);
            set => _memory.Write8(0x776A, value);
        }

        public bool IsDark
        {
            get => false;
            set { }
        }

        public int MaximumShots
        {
            get => _memory.Read8(0x7767);
            set => _memory.Write8(0x7767, value);
        }

        public string Name
        {
            get => _memory.ReadString(0x2BAE);
            set => _memory.WriteString(0x2BAE, value);
        }

        public bool RestartOnZap
        {
            get => _memory.ReadBool(0x776C);
            set => _memory.WriteBool(0x776C, value);
        }

        public int TimeLimit
        {
            get => _memory.Read16(0x7773);
            set => _memory.Write16(0x7773, value);
        }
    }
}