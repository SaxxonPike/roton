using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Super
{
    public sealed class SuperElement : Element
    {
        private readonly int _offset;
        private readonly IMemory _memory;

        public SuperElement(IMemory memory, int index)
            : base(index)
        {
            _memory = memory;
            _offset = 0x7CAA + index * 0x00C2;
        }

        public override string BoardEditText
        {
            get => _memory.ReadString(_offset + 0x81);
            set => _memory.WriteString(_offset + 0x81, value);
        }

        public override int Character
        {
            get => _memory.Read8(_offset + 0x00);
            set => _memory.Write8(_offset + 0x00, value);
        }

        public override string CodeEditText
        {
            get => _memory.ReadString(_offset + 0xAB);
            set => _memory.WriteString(_offset + 0xAB, value);
        }

        public override int Color
        {
            get => _memory.Read8(_offset + 0x01);
            set => _memory.Write8(_offset + 0x01, value);
        }

        public override int Cycle
        {
            get => _memory.Read16(_offset + 0x0B);
            set => _memory.Write16(_offset + 0x0B, value);
        }

        public override string EditorCategory
        {
            get => _memory.ReadString(_offset + 0x2D);
            set => _memory.WriteString(_offset + 0x2D, value);
        }

        public override bool HasDrawCode
        {
            get => _memory.ReadBool(_offset + 0x06);
            set => _memory.WriteBool(_offset + 0x06, value);
        }

        public override bool IsAlwaysVisible { get; set; }

        public override bool IsDestructible
        {
            get => _memory.ReadBool(_offset + 0x02);
            set => _memory.WriteBool(_offset + 0x02, value);
        }

        public override bool IsEditorFloor
        {
            get => _memory.ReadBool(_offset + 0x04);
            set => _memory.WriteBool(_offset + 0x04, value);
        }

        public override bool IsFloor
        {
            get => _memory.ReadBool(_offset + 0x05);
            set => _memory.WriteBool(_offset + 0x05, value);
        }

        public override bool IsPushable
        {
            get => _memory.ReadBool(_offset + 0x03);
            set => _memory.WriteBool(_offset + 0x03, value);
        }

        public override int MenuIndex
        {
            get => _memory.Read16(_offset + 0x15);
            set => _memory.Write16(_offset + 0x15, value);
        }

        public override int MenuKey
        {
            get => _memory.Read8(_offset + 0x17);
            set => _memory.Write8(_offset + 0x17, value);
        }

        public override string Name
        {
            get => _memory.ReadString(_offset + 0x18);
            set => _memory.WriteString(_offset + 0x18, value);
        }

        public override string P1EditText
        {
            get => _memory.ReadString(_offset + 0x42);
            set => _memory.WriteString(_offset + 0x42, value);
        }

        public override string P2EditText
        {
            get => _memory.ReadString(_offset + 0x57);
            set => _memory.WriteString(_offset + 0x57, value);
        }

        public override string P3EditText
        {
            get => _memory.ReadString(_offset + 0x6C);
            set => _memory.WriteString(_offset + 0x6C, value);
        }

        public override int Points
        {
            get => _memory.Read16(_offset + 0xC0);
            set => _memory.Write16(_offset + 0xC0, value);
        }

        public override string StepEditText
        {
            get => _memory.ReadString(_offset + 0x96);
            set => _memory.WriteString(_offset + 0x96, value);
        }
    }
}