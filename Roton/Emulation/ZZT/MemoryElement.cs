namespace Roton.Emulation.ZZT
{
    internal sealed class MemoryElement : MemoryElementBase
    {
        public MemoryElement(Memory memory, int index)
            : base(memory, 0x4AD4 + index*0x00C3)
        {
            Index = index;
        }

        public override string Board
        {
            get { return Memory.ReadString(Offset + 0x82); }
            internal set { Memory.WriteString(Offset + 0x82, value); }
        }

        public override string Category
        {
            get { return Memory.ReadString(Offset + 0x2E); }
            internal set { Memory.WriteString(Offset + 0x2E, value); }
        }

        public override int Character
        {
            get { return Memory.Read8(Offset + 0x00); }
            internal set { Memory.Write8(Offset + 0x00, value); }
        }

        public override string Code
        {
            get { return Memory.ReadString(Offset + 0xAC); }
            internal set { Memory.WriteString(Offset + 0xAC, value); }
        }

        public override int Color
        {
            get { return Memory.Read8(Offset + 0x01); }
            internal set { Memory.Write8(Offset + 0x01, value); }
        }

        internal override void CopyFrom(MemoryElementBase other)
        {
            other.Memory.Read(other.Offset, 0x00C3);
        }

        public override int Cycle
        {
            get { return Memory.Read16(Offset + 0x0C); }
            internal set { Memory.Write16(Offset + 0x0C, value); }
        }

        public override bool Destructible
        {
            get { return Memory.ReadBool(Offset + 0x02); }
            internal set { Memory.WriteBool(Offset + 0x02, value); }
        }

        public override bool DrawCodeEnable
        {
            get { return Memory.ReadBool(Offset + 0x07); }
            internal set { Memory.WriteBool(Offset + 0x07, value); }
        }

        public override bool EditorFloor
        {
            get { return Memory.ReadBool(Offset + 0x05); }
            internal set { Memory.WriteBool(Offset + 0x05, value); }
        }

        public override bool Floor
        {
            get { return Memory.ReadBool(Offset + 0x06); }
            internal set { Memory.WriteBool(Offset + 0x06, value); }
        }

        public override int Key
        {
            get { return Memory.Read8(Offset + 0x18); }
            internal set { Memory.Write8(Offset + 0x18, value); }
        }

        public override int Menu
        {
            get { return Memory.Read16(Offset + 0x16); }
            internal set { Memory.Write16(Offset + 0x16, value); }
        }

        public override string Name
        {
            get { return Memory.ReadString(Offset + 0x19); }
            internal set { Memory.WriteString(Offset + 0x19, value); }
        }

        public override string P1
        {
            get { return Memory.ReadString(Offset + 0x43); }
            internal set { Memory.WriteString(Offset + 0x43, value); }
        }

        public override string P2
        {
            get { return Memory.ReadString(Offset + 0x58); }
            internal set { Memory.WriteString(Offset + 0x58, value); }
        }

        public override string P3
        {
            get { return Memory.ReadString(Offset + 0x6D); }
            internal set { Memory.WriteString(Offset + 0x6D, value); }
        }

        public override int Points
        {
            get { return Memory.Read16(Offset + 0xC1); }
            internal set { Memory.Write16(Offset + 0xC1, value); }
        }

        public override bool Pushable
        {
            get { return Memory.ReadBool(Offset + 0x03); }
            internal set { Memory.WriteBool(Offset + 0x03, value); }
        }

        public override bool Shown
        {
            get { return Memory.ReadBool(Offset + 0x04); }
            set { Memory.WriteBool(Offset + 0x04, value); }
        }

        public override string Step
        {
            get { return Memory.ReadString(Offset + 0x97); }
            internal set { Memory.WriteString(Offset + 0x97, value); }
        }
    }
}