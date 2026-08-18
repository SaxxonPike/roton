using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Original;

public sealed class OriginalElement(IMemory memory, int index) : Element(index)
{
    private readonly int _offset = 0x4AD4 + index * 0x00C3;

    public override string BoardEditText
    {
        get => memory.ReadString(_offset + 0x82);
        set => memory.WriteString(_offset + 0x82, value);
    }

    public override int Character
    {
        get => memory.Read8(_offset + 0x00);
        set => memory.Write8(_offset + 0x00, value);
    }

    public override string CodeEditText
    {
        get => memory.ReadString(_offset + 0xAC);
        set => memory.WriteString(_offset + 0xAC, value);
    }

    public override int Color
    {
        get => memory.Read8(_offset + 0x01);
        set => memory.Write8(_offset + 0x01, value);
    }

    public override int Cycle
    {
        get => memory.Read16(_offset + 0x0C);
        set => memory.Write16(_offset + 0x0C, value);
    }

    public override string EditorCategory
    {
        get => memory.ReadString(_offset + 0x2E);
        set => memory.WriteString(_offset + 0x2E, value);
    }

    public override bool HasDrawCode
    {
        get => memory.ReadBool(_offset + 0x07);
        set => memory.WriteBool(_offset + 0x07, value);
    }

    public override bool IsAlwaysVisible
    {
        get => memory.ReadBool(_offset + 0x04);
        set => memory.WriteBool(_offset + 0x04, value);
    }

    public override bool IsDestructible
    {
        get => memory.ReadBool(_offset + 0x02);
        set => memory.WriteBool(_offset + 0x02, value);
    }

    public override bool IsEditorFloor
    {
        get => memory.ReadBool(_offset + 0x05);
        set => memory.WriteBool(_offset + 0x05, value);
    }

    public override bool IsFloor
    {
        get => memory.ReadBool(_offset + 0x06);
        set => memory.WriteBool(_offset + 0x06, value);
    }

    public override bool IsPushable
    {
        get => memory.ReadBool(_offset + 0x03);
        set => memory.WriteBool(_offset + 0x03, value);
    }

    public override int MenuIndex
    {
        get => memory.Read16(_offset + 0x16);
        set => memory.Write16(_offset + 0x16, value);
    }

    public override int MenuKey
    {
        get => memory.Read8(_offset + 0x18);
        set => memory.Write8(_offset + 0x18, value);
    }

    public override string Name
    {
        get => memory.ReadString(_offset + 0x19);
        set => memory.WriteString(_offset + 0x19, value);
    }

    public override string P1EditText
    {
        get => memory.ReadString(_offset + 0x43);
        set => memory.WriteString(_offset + 0x43, value);
    }

    public override string P2EditText
    {
        get => memory.ReadString(_offset + 0x58);
        set => memory.WriteString(_offset + 0x58, value);
    }

    public override string P3EditText
    {
        get => memory.ReadString(_offset + 0x6D);
        set => memory.WriteString(_offset + 0x6D, value);
    }

    public override int Points
    {
        get => memory.Read16(_offset + 0xC1);
        set => memory.Write16(_offset + 0xC1, value);
    }

    public override string StepEditText
    {
        get => memory.ReadString(_offset + 0x97);
        set => memory.WriteString(_offset + 0x97, value);
    }

    public override bool NameMatches(ReadOnlySpan<char> name) =>
        memory.ReadStringSpan(_offset + 0x19).CaseInsensitiveCharacterEqual(name);
}