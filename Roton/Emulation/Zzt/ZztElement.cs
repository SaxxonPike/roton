﻿using Roton.Emulation.Behaviors;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztElement : Element
    {
        public ZztElement(IMemory memory, int index, IBehavior behavior)
            : base(memory, 0x4AD4 + index*0x00C3, behavior)
        {
            Id = index;
        }

        public override string BoardEditText
        {
            get { return Memory.ReadString(Offset + 0x82); }
            set { Memory.WriteString(Offset + 0x82, value); }
        }

        public override int Character
        {
            get { return Memory.Read8(Offset + 0x00); }
            set { Memory.Write8(Offset + 0x00, value); }
        }

        public override string CodeEditText
        {
            get { return Memory.ReadString(Offset + 0xAC); }
            set { Memory.WriteString(Offset + 0xAC, value); }
        }

        public override int Color
        {
            get { return Memory.Read8(Offset + 0x01); }
            set { Memory.Write8(Offset + 0x01, value); }
        }

        public override int Cycle
        {
            get { return Memory.Read16(Offset + 0x0C); }
            set { Memory.Write16(Offset + 0x0C, value); }
        }

        public override string EditorCategory
        {
            get { return Memory.ReadString(Offset + 0x2E); }
            set { Memory.WriteString(Offset + 0x2E, value); }
        }

        public override bool HasDrawCode
        {
            get { return Memory.ReadBool(Offset + 0x07); }
            set { Memory.WriteBool(Offset + 0x07, value); }
        }

        public override bool IsAlwaysVisible
        {
            get { return Memory.ReadBool(Offset + 0x04); }
            set { Memory.WriteBool(Offset + 0x04, value); }
        }

        public override bool IsDestructible
        {
            get { return Memory.ReadBool(Offset + 0x02); }
            set { Memory.WriteBool(Offset + 0x02, value); }
        }

        public override bool IsEditorFloor
        {
            get { return Memory.ReadBool(Offset + 0x05); }
            set { Memory.WriteBool(Offset + 0x05, value); }
        }

        public override bool IsFloor
        {
            get { return Memory.ReadBool(Offset + 0x06); }
            set { Memory.WriteBool(Offset + 0x06, value); }
        }

        public override bool IsPushable
        {
            get { return Memory.ReadBool(Offset + 0x03); }
            set { Memory.WriteBool(Offset + 0x03, value); }
        }

        public override int MenuIndex
        {
            get { return Memory.Read16(Offset + 0x16); }
            set { Memory.Write16(Offset + 0x16, value); }
        }

        public override int MenuKey
        {
            get { return Memory.Read8(Offset + 0x18); }
            set { Memory.Write8(Offset + 0x18, value); }
        }

        public override string Name
        {
            get { return Memory.ReadString(Offset + 0x19); }
            set { Memory.WriteString(Offset + 0x19, value); }
        }

        public override string P1EditText
        {
            get { return Memory.ReadString(Offset + 0x43); }
            set { Memory.WriteString(Offset + 0x43, value); }
        }

        public override string P2EditText
        {
            get { return Memory.ReadString(Offset + 0x58); }
            set { Memory.WriteString(Offset + 0x58, value); }
        }

        public override string P3EditText
        {
            get { return Memory.ReadString(Offset + 0x6D); }
            set { Memory.WriteString(Offset + 0x6D, value); }
        }

        public override int Points
        {
            get { return Memory.Read16(Offset + 0xC1); }
            set { Memory.Write16(Offset + 0xC1, value); }
        }

        public override string StepEditText
        {
            get { return Memory.ReadString(Offset + 0x97); }
            set { Memory.WriteString(Offset + 0x97, value); }
        }
    }
}