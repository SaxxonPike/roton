using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Original;

public sealed class OriginalElement : Element
{
    private readonly IMemory _memory;
    private readonly int _offset;

    public OriginalElement(IMemory memory, int index)
        : base(index)
    {
        _memory = memory;
        _offset = 0x4AD4 + index * 0x00C3;
    }

    public override string BoardEditText
    {
        get => _memory.ReadString(_offset + 0x82);
        set => _memory.WriteString(_offset + 0x82, value);
    }

    public override int Character
    {
        get => _memory.Read8(_offset + 0x00);
        set => _memory.Write8(_offset + 0x00, value);
    }

    public override string CodeEditText
    {
        get => _memory.ReadString(_offset + 0xAC);
        set => _memory.WriteString(_offset + 0xAC, value);
    }

    public override int Color
    {
        get => _memory.Read8(_offset + 0x01);
        set => _memory.Write8(_offset + 0x01, value);
    }

    public override int Cycle
    {
        get => _memory.Read16(_offset + 0x0C);
        set => _memory.Write16(_offset + 0x0C, value);
    }

    public override string EditorCategory
    {
        get => _memory.ReadString(_offset + 0x2E);
        set => _memory.WriteString(_offset + 0x2E, value);
    }

    public override bool HasDrawCode
    {
        get => _memory.ReadBool(_offset + 0x07);
        set => _memory.WriteBool(_offset + 0x07, value);
    }

    public override bool IsAlwaysVisible
    {
        get => _memory.ReadBool(_offset + 0x04);
        set => _memory.WriteBool(_offset + 0x04, value);
    }

    public override bool IsDestructible
    {
        get => _memory.ReadBool(_offset + 0x02);
        set => _memory.WriteBool(_offset + 0x02, value);
    }

    public override bool IsEditorFloor
    {
        get => _memory.ReadBool(_offset + 0x05);
        set => _memory.WriteBool(_offset + 0x05, value);
    }

    public override bool IsFloor
    {
        get => _memory.ReadBool(_offset + 0x06);
        set => _memory.WriteBool(_offset + 0x06, value);
    }

    public override bool IsPushable
    {
        get => _memory.ReadBool(_offset + 0x03);
        set => _memory.WriteBool(_offset + 0x03, value);
    }

    public override int MenuIndex
    {
        get => _memory.Read16(_offset + 0x16);
        set => _memory.Write16(_offset + 0x16, value);
    }

    public override int MenuKey
    {
        get => _memory.Read8(_offset + 0x18);
        set => _memory.Write8(_offset + 0x18, value);
    }

    public override string Name
    {
        get => _memory.ReadString(_offset + 0x19);
        set => _memory.WriteString(_offset + 0x19, value);
    }

    public override string P1EditText
    {
        get => _memory.ReadString(_offset + 0x43);
        set => _memory.WriteString(_offset + 0x43, value);
    }

    public override string P2EditText
    {
        get => _memory.ReadString(_offset + 0x58);
        set => _memory.WriteString(_offset + 0x58, value);
    }

    public override string P3EditText
    {
        get => _memory.ReadString(_offset + 0x6D);
        set => _memory.WriteString(_offset + 0x6D, value);
    }

    public override int Points
    {
        get => _memory.Read16(_offset + 0xC1);
        set => _memory.Write16(_offset + 0xC1, value);
    }

    public override string StepEditText
    {
        get => _memory.ReadString(_offset + 0x97);
        set => _memory.WriteString(_offset + 0x97, value);
    }
}