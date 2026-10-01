using System;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Original;

internal sealed class OriginalElement(IMemory memory, int index) : IElement
{
    private readonly int _offset = 0x4AD8 + index * 0x00C3;

    public string BoardEditText
    {
        get => memory.ReadString(_offset + 0x82);
        set => memory.WriteString(_offset + 0x82, value);
    }

    public ref HWord Character => 
        ref memory.GetRef<HWord>(_offset + 0x00);

    public string CodeEditText
    {
        get => memory.ReadString(_offset + 0xAC);
        set => memory.WriteString(_offset + 0xAC, value);
    }

    public ref HWord Color => 
        ref memory.GetRef<HWord>(_offset + 0x01);

    public ref Word Cycle => 
        ref memory.GetRef<Word>(_offset + 0x0C);

    public string EditorCategory
    {
        get => memory.ReadString(_offset + 0x2E);
        set => memory.WriteString(_offset + 0x2E, value);
    }

    public ref Bool HasDrawCode => 
        ref memory.GetRef<Bool>(_offset + 0x07);

    public int Id => index;

    public ref Bool IsAlwaysVisible => 
        ref memory.GetRef<Bool>(_offset + 0x04);

    public ref Bool IsDestructible => 
        ref memory.GetRef<Bool>(_offset + 0x02);

    public ref Bool IsEditorFloor => 
        ref memory.GetRef<Bool>(_offset + 0x05);

    public ref Bool IsFloor => 
        ref memory.GetRef<Bool>(_offset + 0x06);

    public ref Bool IsPushable => 
        ref memory.GetRef<Bool>(_offset + 0x03);

    public ref Word MenuIndex => 
        ref memory.GetRef<Word>(_offset + 0x16);

    public ref PChar MenuKey => 
        ref memory.GetRef<PChar>(_offset + 0x18);

    public string Name
    {
        get => memory.ReadString(_offset + 0x19);
        set => memory.WriteString(_offset + 0x19, value);
    }

    public string P1EditText
    {
        get => memory.ReadString(_offset + 0x43);
        set => memory.WriteString(_offset + 0x43, value);
    }

    public string P2EditText
    {
        get => memory.ReadString(_offset + 0x58);
        set => memory.WriteString(_offset + 0x58, value);
    }

    public string P3EditText
    {
        get => memory.ReadString(_offset + 0x6D);
        set => memory.WriteString(_offset + 0x6D, value);
    }

    public ref Word Points => 
        ref memory.GetRef<Word>(_offset + 0xC1);

    public string StepEditText
    {
        get => memory.ReadString(_offset + 0x97);
        set => memory.WriteString(_offset + 0x97, value);
    }

    public bool CanContainCode =>
        memory.Data[_offset + 0xAC] > 0;

    public bool NameMatches(ReadOnlySpan<char> name) =>
        memory.ReadStringSpan(_offset + 0x19).CaseInsensitiveCharacterEqual(name);
}