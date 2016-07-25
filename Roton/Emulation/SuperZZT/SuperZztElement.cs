using Roton.Core;
using Roton.Emulation.Behavior;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztElement : Element
    {
        public SuperZztElement(IMemory memory, int index, IBehavior behavior)
            : base(memory, 0x7CAA + index*0x00C2, behavior)
        {
            Id = index;
        }

        public override string BoardEditText
        {
            get { return Memory.ReadString(Offset + 0x81); }
            set { Memory.WriteString(Offset + 0x81, value); }
        }

        public override int Character
        {
            get { return Memory.Read8(Offset + 0x00); }
            set { Memory.Write8(Offset + 0x00, value); }
        }

        public override string CodeEditText
        {
            get { return Memory.ReadString(Offset + 0xAB); }
            set { Memory.WriteString(Offset + 0xAB, value); }
        }

        public override int Color
        {
            get { return Memory.Read8(Offset + 0x01); }
            set { Memory.Write8(Offset + 0x01, value); }
        }

        public override int Cycle
        {
            get { return Memory.Read16(Offset + 0x0B); }
            set { Memory.Write16(Offset + 0x0B, value); }
        }

        public override string EditorCategory
        {
            get { return Memory.ReadString(Offset + 0x2D); }
            set { Memory.WriteString(Offset + 0x2D, value); }
        }

        public override bool HasDrawCode
        {
            get { return Memory.ReadBool(Offset + 0x06); }
            set { Memory.WriteBool(Offset + 0x06, value); }
        }

        public override bool IsDestructible
        {
            get { return Memory.ReadBool(Offset + 0x02); }
            set { Memory.WriteBool(Offset + 0x02, value); }
        }

        public override bool IsEditorFloor
        {
            get { return Memory.ReadBool(Offset + 0x04); }
            set { Memory.WriteBool(Offset + 0x04, value); }
        }

        public override bool IsFloor
        {
            get { return Memory.ReadBool(Offset + 0x05); }
            set { Memory.WriteBool(Offset + 0x05, value); }
        }

        public override bool IsPushable
        {
            get { return Memory.ReadBool(Offset + 0x03); }
            set { Memory.WriteBool(Offset + 0x03, value); }
        }

        public override int MenuIndex
        {
            get { return Memory.Read16(Offset + 0x15); }
            set { Memory.Write16(Offset + 0x15, value); }
        }

        public override int MenuKey
        {
            get { return Memory.Read8(Offset + 0x17); }
            set { Memory.Write8(Offset + 0x17, value); }
        }

        public override string Name
        {
            get { return Memory.ReadString(Offset + 0x18); }
            set { Memory.WriteString(Offset + 0x18, value); }
        }

        public override string P1EditText
        {
            get { return Memory.ReadString(Offset + 0x42); }
            set { Memory.WriteString(Offset + 0x42, value); }
        }

        public override string P2EditText
        {
            get { return Memory.ReadString(Offset + 0x57); }
            set { Memory.WriteString(Offset + 0x57, value); }
        }

        public override string P3EditText
        {
            get { return Memory.ReadString(Offset + 0x6C); }
            set { Memory.WriteString(Offset + 0x6C, value); }
        }

        public override int Points
        {
            get { return Memory.Read16(Offset + 0xC0); }
            set { Memory.Write16(Offset + 0xC0, value); }
        }

        public override string StepEditText
        {
            get { return Memory.ReadString(Offset + 0x96); }
            set { Memory.WriteString(Offset + 0x96, value); }
        }
    }
}