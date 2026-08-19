using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Super;

public sealed class SuperElement(IMemory memory, int index) : Element(index)
{
    private readonly int _offset = 0x7CAA + index * 0x00C2;
    private Bool _isAlwaysVisible;

    public override string BoardEditText
    {
        get => memory.ReadString(_offset + 0x81);
        set => memory.WriteString(_offset + 0x81, value);
    }

    public override ref HWord Character => ref memory.GetRef<HWord>(_offset + 0x00);

    public override string CodeEditText
    {
        get => memory.ReadString(_offset + 0xAB);
        set => memory.WriteString(_offset + 0xAB, value);
    }

    public override ref HWord Color => ref memory.GetRef<HWord>(_offset + 0x01);

    public override ref Word Cycle => ref memory.GetRef<Word>(_offset + 0x0B);

    public override string EditorCategory
    {
        get => memory.ReadString(_offset + 0x2D);
        set => memory.WriteString(_offset + 0x2D, value);
    }

    public override ref Bool HasDrawCode => ref memory.GetRef<Bool>(_offset + 0x06);

    public override ref Bool IsAlwaysVisible => ref _isAlwaysVisible;

    public override ref Bool IsDestructible => ref memory.GetRef<Bool>(_offset + 0x02);

    public override ref Bool IsEditorFloor => ref memory.GetRef<Bool>(_offset + 0x04);

    public override ref Bool IsFloor => ref memory.GetRef<Bool>(_offset + 0x05);

    public override ref Bool IsPushable => ref memory.GetRef<Bool>(_offset + 0x03);

    public override ref Word MenuIndex => ref memory.GetRef<Word>(_offset + 0x15);

    public override ref PChar MenuKey => ref memory.GetRef<PChar>(_offset + 0x17);

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

    public override ref Word Points => ref memory.GetRef<Word>(_offset + 0xC0);

    public override string StepEditText
    {
        get => memory.ReadString(_offset + 0x96);
        set => memory.WriteString(_offset + 0x96, value);
    }

    public override bool CanContainCode =>
        memory.Data[_offset + 0xAB] > 0;

    public override bool NameMatches(ReadOnlySpan<char> name) =>
        memory.ReadStringSpan(_offset + 0x18).CaseInsensitiveCharacterEqual(name);
}