namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryElement : MemoryElementBase
    {
        public MemoryElement(Memory memory, int index)
            : base(memory, 0x7CAA + index*0x00C2)
        {
            Index = index;
        }

        public override string Board
        {
            get { return Memory.ReadString(Offset + 0x81); }
            internal set { Memory.WriteString(Offset + 0x81, value); }
        }

        public override string Category
        {
            get { return Memory.ReadString(Offset + 0x2D); }
            internal set { Memory.WriteString(Offset + 0x2D, value); }
        }

        public override int Character
        {
            get { return Memory.Read8(Offset + 0x00); }
            internal set { Memory.Write8(Offset + 0x00, value); }
        }

        public override string Code
        {
            get { return Memory.ReadString(Offset + 0xAB); }
            internal set { Memory.WriteString(Offset + 0xAB, value); }
        }

        public override int Color
        {
            get { return Memory.Read8(Offset + 0x01); }
            internal set { Memory.Write8(Offset + 0x01, value); }
        }

        internal override void CopyFrom(MemoryElementBase other)
        {
            Memory.Write(Offset, other.Memory.Read(other.Offset, 0x00C2));
        }

        public override int Cycle
        {
            get { return Memory.Read16(Offset + 0x0B); }
            internal set { Memory.Write16(Offset + 0x0B, value); }
        }

        public override bool Destructible
        {
            get { return Memory.ReadBool(Offset + 0x02); }
            internal set { Memory.WriteBool(Offset + 0x02, value); }
        }

        public override bool DrawCodeEnable
        {
            get { return Memory.ReadBool(Offset + 0x06); }
            internal set { Memory.WriteBool(Offset + 0x06, value); }
        }

        public override bool EditorFloor
        {
            get { return Memory.ReadBool(Offset + 0x04); }
            internal set { Memory.WriteBool(Offset + 0x04, value); }
        }

        public override bool Floor
        {
            get { return Memory.ReadBool(Offset + 0x05); }
            internal set { Memory.WriteBool(Offset + 0x05, value); }
        }

        public override int Key
        {
            get { return Memory.Read8(Offset + 0x17); }
            internal set { Memory.Write8(Offset + 0x17, value); }
        }

        public override int Menu
        {
            get { return Memory.Read16(Offset + 0x15); }
            internal set { Memory.Write16(Offset + 0x15, value); }
        }

        public override string Name
        {
            get { return Memory.ReadString(Offset + 0x18); }
            internal set { Memory.WriteString(Offset + 0x18, value); }
        }

        public override string P1
        {
            get { return Memory.ReadString(Offset + 0x42); }
            internal set { Memory.WriteString(Offset + 0x42, value); }
        }

        public override string P2
        {
            get { return Memory.ReadString(Offset + 0x57); }
            internal set { Memory.WriteString(Offset + 0x57, value); }
        }

        public override string P3
        {
            get { return Memory.ReadString(Offset + 0x6C); }
            internal set { Memory.WriteString(Offset + 0x6C, value); }
        }

        public override int Points
        {
            get { return Memory.Read16(Offset + 0xC0); }
            internal set { Memory.Write16(Offset + 0xC0, value); }
        }

        public override bool Pushable
        {
            get { return Memory.ReadBool(Offset + 0x03); }
            internal set { Memory.WriteBool(Offset + 0x03, value); }
        }

        public override string Step
        {
            get { return Memory.ReadString(Offset + 0x96); }
            internal set { Memory.WriteString(Offset + 0x96, value); }
        }
    }
}