namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryBoard : MemoryBoardBase
    {
        public MemoryBoard(Memory memory)
            : base(memory)
        {
        }

        public override Location Camera
        {
            get { return new MemoryLocation16(Memory, 0x776F); }
            set { new MemoryLocation16(Memory, 0x776F).CopyFrom(value); }
        }

        public override bool Dark
        {
            get { return false; }
            set { }
        }

        public override Location Enter
        {
            get { return new MemoryLocation(Memory, 0x776D); }
            set { new MemoryLocation(Memory, 0x776D).CopyFrom(value); }
        }

        public override int ExitEast
        {
            get { return Memory.Read8(0x776B); }
            set { Memory.Write8(0x776B, value); }
        }

        public override int ExitNorth
        {
            get { return Memory.Read8(0x7768); }
            set { Memory.Write8(0x7768, value); }
        }

        public override int ExitSouth
        {
            get { return Memory.Read8(0x7769); }
            set { Memory.Write8(0x7769, value); }
        }

        public override int ExitWest
        {
            get { return Memory.Read8(0x776A); }
            set { Memory.Write8(0x776A, value); }
        }

        public override string Name
        {
            get { return Memory.ReadString(0x2BAE); }
            set { Memory.WriteString(0x2BAE, value); }
        }

        public override bool RestartOnZap
        {
            get { return Memory.ReadBool(0x776C); }
            set { Memory.WriteBool(0x776C, value); }
        }

        public override int Shots
        {
            get { return Memory.Read8(0x7767); }
            set { Memory.Write8(0x7767, value); }
        }

        public override int TimeLimit
        {
            get { return Memory.Read16(0x7773); }
            set { Memory.Write16(0x7773, value); }
        }
    }
}