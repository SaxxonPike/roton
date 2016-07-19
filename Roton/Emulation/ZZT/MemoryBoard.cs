namespace Roton.Emulation.ZZT
{
    internal sealed class MemoryBoard : MemoryBoardBase
    {
        public MemoryBoard(Memory memory)
            : base(memory)
        {
            Camera = new Location();
        }

        public override bool Dark
        {
            get { return Memory.ReadBool(0x4568); }
            set { Memory.WriteBool(0x4568, value); }
        }

        public override Location Enter
        {
            get { return new MemoryLocation(Memory, 0x45A9); }
            set { new MemoryLocation(Memory, 0x45A9).CopyFrom(value); }
        }

        public override int ExitEast
        {
            get { return Memory.Read8(0x456C); }
            set { Memory.Write8(0x456C, value); }
        }

        public override int ExitNorth
        {
            get { return Memory.Read8(0x4569); }
            set { Memory.Write8(0x4569, value); }
        }

        public override int ExitSouth
        {
            get { return Memory.Read8(0x456A); }
            set { Memory.Write8(0x456A, value); }
        }

        public override int ExitWest
        {
            get { return Memory.Read8(0x456B); }
            set { Memory.Write8(0x456B, value); }
        }

        public override string Name
        {
            get { return Memory.ReadString(0x2486); }
            set { Memory.WriteString(0x2486, value); }
        }

        public override bool RestartOnZap
        {
            get { return Memory.ReadBool(0x456D); }
            set { Memory.WriteBool(0x456D, value); }
        }

        public override int Shots
        {
            get { return Memory.Read8(0x4567); }
            set { Memory.Write8(0x4567, value); }
        }

        public override int TimeLimit
        {
            get { return Memory.Read16(0x45AB); }
            set { Memory.Write16(0x45AB, value); }
        }
    }
}