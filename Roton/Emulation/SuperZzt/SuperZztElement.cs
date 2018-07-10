using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.SuperZzt
{
    public sealed class SuperZztElement : Element
    {
        public SuperZztElement(IMemory memory, int index)
            : base(memory, 0x7CAA + index * 0x00C2, index)
        {
        }

        public override string BoardEditText
        {
            get => Memory.ReadString(Offset + 0x81);
            set => Memory.WriteString(Offset + 0x81, value);
        }

        public override int Character
        {
            get => Memory.Read8(Offset + 0x00);
            set => Memory.Write8(Offset + 0x00, value);
        }

        public override string CodeEditText
        {
            get => Memory.ReadString(Offset + 0xAB);
            set => Memory.WriteString(Offset + 0xAB, value);
        }

        public override int Color
        {
            get => Memory.Read8(Offset + 0x01);
            set => Memory.Write8(Offset + 0x01, value);
        }

        public override int Cycle
        {
            get => Memory.Read16(Offset + 0x0B);
            set => Memory.Write16(Offset + 0x0B, value);
        }

        public override string EditorCategory
        {
            get => Memory.ReadString(Offset + 0x2D);
            set => Memory.WriteString(Offset + 0x2D, value);
        }

        public override bool HasDrawCode
        {
            get => Memory.ReadBool(Offset + 0x06);
            set => Memory.WriteBool(Offset + 0x06, value);
        }

        public override bool IsAlwaysVisible { get; set; }

        public override bool IsDestructible
        {
            get => Memory.ReadBool(Offset + 0x02);
            set => Memory.WriteBool(Offset + 0x02, value);
        }

        public override bool IsEditorFloor
        {
            get => Memory.ReadBool(Offset + 0x04);
            set => Memory.WriteBool(Offset + 0x04, value);
        }

        public override bool IsFloor
        {
            get => Memory.ReadBool(Offset + 0x05);
            set => Memory.WriteBool(Offset + 0x05, value);
        }

        public override bool IsPushable
        {
            get => Memory.ReadBool(Offset + 0x03);
            set => Memory.WriteBool(Offset + 0x03, value);
        }

        public override int MenuIndex
        {
            get => Memory.Read16(Offset + 0x15);
            set => Memory.Write16(Offset + 0x15, value);
        }

        public override int MenuKey
        {
            get => Memory.Read8(Offset + 0x17);
            set => Memory.Write8(Offset + 0x17, value);
        }

        public override string Name
        {
            get => Memory.ReadString(Offset + 0x18);
            set => Memory.WriteString(Offset + 0x18, value);
        }

        public override string P1EditText
        {
            get => Memory.ReadString(Offset + 0x42);
            set => Memory.WriteString(Offset + 0x42, value);
        }

        public override string P2EditText
        {
            get => Memory.ReadString(Offset + 0x57);
            set => Memory.WriteString(Offset + 0x57, value);
        }

        public override string P3EditText
        {
            get => Memory.ReadString(Offset + 0x6C);
            set => Memory.WriteString(Offset + 0x6C, value);
        }

        public override int Points
        {
            get => Memory.Read16(Offset + 0xC0);
            set => Memory.Write16(Offset + 0xC0, value);
        }

        public override string StepEditText
        {
            get => Memory.ReadString(Offset + 0x96);
            set => Memory.WriteString(Offset + 0x96, value);
        }
    }
}