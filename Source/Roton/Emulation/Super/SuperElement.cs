using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Super;

public sealed class SuperElement(IMemory memory, int index) : Element(index)
{
    private readonly int _offset = 0x7CAA + index * 0x00C2;

    public override string BoardEditText
    {
        get => memory.ReadString(_offset + 0x81);
        set => memory.WriteString(_offset + 0x81, value);
    }

    public override int Character
    {
        get => memory.Read8(_offset + 0x00);
        set => memory.Write8(_offset + 0x00, value);
    }

    public override string CodeEditText
    {
        get => memory.ReadString(_offset + 0xAB);
        set => memory.WriteString(_offset + 0xAB, value);
    }

    public override int Color
    {
        get => memory.Read8(_offset + 0x01);
        set => memory.Write8(_offset + 0x01, value);
    }

    public override int Cycle
    {
        get => memory.Read16(_offset + 0x0B);
        set => memory.Write16(_offset + 0x0B, value);
    }

    public override string EditorCategory
    {
        get => memory.ReadString(_offset + 0x2D);
        set => memory.WriteString(_offset + 0x2D, value);
    }

    public override bool HasDrawCode
    {
        get => memory.ReadBool(_offset + 0x06);
        set => memory.WriteBool(_offset + 0x06, value);
    }

    public override bool IsAlwaysVisible { get; set; }

    public override bool IsDestructible
    {
        get => memory.ReadBool(_offset + 0x02);
        set => memory.WriteBool(_offset + 0x02, value);
    }

    public override bool IsEditorFloor
    {
        get => memory.ReadBool(_offset + 0x04);
        set => memory.WriteBool(_offset + 0x04, value);
    }

    public override bool IsFloor
    {
        get => memory.ReadBool(_offset + 0x05);
        set => memory.WriteBool(_offset + 0x05, value);
    }

    public override bool IsPushable
    {
        get => memory.ReadBool(_offset + 0x03);
        set => memory.WriteBool(_offset + 0x03, value);
    }

    public override int MenuIndex
    {
        get => memory.Read16(_offset + 0x15);
        set => memory.Write16(_offset + 0x15, value);
    }

    public override int MenuKey
    {
        get => memory.Read8(_offset + 0x17);
        set => memory.Write8(_offset + 0x17, value);
    }

    public override string Name
    {
        get => memory.ReadString(_offset + 0x18);
        set => memory.WriteString(_offset + 0x18, value);
    }

    public override string P1EditText
    {
        get => memory.ReadString(_offset + 0x42);
        set => memory.WriteString(_offset + 0x42, value);
    }

    public override string P2EditText
    {
        get => memory.ReadString(_offset + 0x57);
        set => memory.WriteString(_offset + 0x57, value);
    }

    public override string P3EditText
    {
        get => memory.ReadString(_offset + 0x6C);
        set => memory.WriteString(_offset + 0x6C, value);
    }

    public override int Points
    {
        get => memory.Read16(_offset + 0xC0);
        set => memory.Write16(_offset + 0xC0, value);
    }

    public override string StepEditText
    {
        get => memory.ReadString(_offset + 0x96);
        set => memory.WriteString(_offset + 0x96, value);
    }

    public override bool NameMatches(ReadOnlySpan<char> name) =>
        memory.ReadStringSpan(_offset + 0x18).CaseInsensitiveCharacterEqual(name);
}