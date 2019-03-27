using System.Runtime.CompilerServices;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalBoard : IBoard
    {
        private readonly IMemory _memory;

        public OriginalBoard(IMemory memory)
        {
            _memory = memory;
            Entrance = new MemoryLocation(_memory, 0x45A9);
        }

        public IXyPair Camera { get; } = new Location();

        public IXyPair Entrance { get; }

        public int ExitEast
        {
            get => _memory.Read8(0x456C);
            set => _memory.Write8(0x456C, value);
        }

        public int ExitNorth
        {
            get => _memory.Read8(0x4569);
            set => _memory.Write8(0x4569, value);
        }

        public int ExitSouth
        {
            get => _memory.Read8(0x456A);
            set => _memory.Write8(0x456A, value);
        }

        public int ExitWest
        {
            get => _memory.Read8(0x456B);
            set => _memory.Write8(0x456B, value);
        }

        public bool IsDark
        {
            get => _memory.ReadBool(0x4568);
            set => _memory.WriteBool(0x4568, value);
        }

        public int MaximumShots
        {
            get => _memory.Read8(0x4567);
            set => _memory.Write8(0x4567, value);
        }

        public string Name
        {
            get => _memory.ReadString(0x2486);
            set => _memory.WriteString(0x2486, value);
        }

        public bool RestartOnZap
        {
            get => _memory.ReadBool(0x456D);
            set => _memory.WriteBool(0x456D, value);
        }

        public int TimeLimit
        {
            get => _memory.Read16(0x45AB);
            set => _memory.Write16(0x45AB, value);
        }
    }
}