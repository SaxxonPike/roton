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

    public override ref HWord Character => ref memory.GetRef<HWord>(_offset + 0x00);

    public override string CodeEditText
    {
        get => memory.ReadString(_offset + 0xAC);
        set => memory.WriteString(_offset + 0xAC, value);
    }

    public override ref HWord Color => ref memory.GetRef<HWord>(_offset + 0x01);

    public override ref Word Cycle => ref memory.GetRef<Word>(_offset + 0x0C);

    public override string EditorCategory
    {
        get => memory.ReadString(_offset + 0x2E);
        set => memory.WriteString(_offset + 0x2E, value);
    }

    public override ref Bool HasDrawCode => ref memory.GetRef<Bool>(_offset + 0x07);

    public override ref Bool IsAlwaysVisible => ref memory.GetRef<Bool>(_offset + 0x04);

    public override ref Bool IsDestructible => ref memory.GetRef<Bool>(_offset + 0x02);

    public override ref Bool IsEditorFloor => ref memory.GetRef<Bool>(_offset + 0x05);

    public override ref Bool IsFloor => ref memory.GetRef<Bool>(_offset + 0x06);

    public override ref Bool IsPushable => ref memory.GetRef<Bool>(_offset + 0x03);

    public override ref Word MenuIndex => ref memory.GetRef<Word>(_offset + 0x16);

    public override ref PChar MenuKey => ref memory.GetRef<PChar>(_offset + 0x18);

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

    public override ref Word Points => ref memory.GetRef<Word>(_offset + 0xC1);

    public override string StepEditText
    {
        get => memory.ReadString(_offset + 0x97);
        set => memory.WriteString(_offset + 0x97, value);
    }

    public override bool CanContainCode =>
        memory.Data[_offset + 0xAC] > 0;

    public override bool NameMatches(ReadOnlySpan<char> name) =>
        memory.ReadStringSpan(_offset + 0x19).CaseInsensitiveCharacterEqual(name);
}