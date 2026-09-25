using System;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Super;

internal sealed class SuperElement(IMemory memory, int index) : IElement
{
    private readonly int _offset = 0x7CAA + index * 0x00C2;
    private Bool _isAlwaysVisible;

    public string BoardEditText
    {
        get => memory.ReadString(_offset + 0x81);
        set => memory.WriteString(_offset + 0x81, value);
    }

    public ref HWord Character => 
        ref memory.GetRef<HWord>(_offset + 0x00);

    public string CodeEditText
    {
        get => memory.ReadString(_offset + 0xAB);
        set => memory.WriteString(_offset + 0xAB, value);
    }

    public ref HWord Color => 
        ref memory.GetRef<HWord>(_offset + 0x01);

    public ref Word Cycle => 
        ref memory.GetRef<Word>(_offset + 0x0B);

    public string EditorCategory
    {
        get => memory.ReadString(_offset + 0x2D);
        set => memory.WriteString(_offset + 0x2D, value);
    }

    public ref Bool HasDrawCode => 
        ref memory.GetRef<Bool>(_offset + 0x06);

    public int Id => index;

    public ref Bool IsAlwaysVisible => 
        ref _isAlwaysVisible;

    public ref Bool IsDestructible => 
        ref memory.GetRef<Bool>(_offset + 0x02);

    public ref Bool IsEditorFloor => 
        ref memory.GetRef<Bool>(_offset + 0x04);

    public ref Bool IsFloor => 
        ref memory.GetRef<Bool>(_offset + 0x05);

    public ref Bool IsPushable => 
        ref memory.GetRef<Bool>(_offset + 0x03);

    public ref Word MenuIndex => 
        ref memory.GetRef<Word>(_offset + 0x15);

    public ref PChar MenuKey => 
        ref memory.GetRef<PChar>(_offset + 0x17);

    public string Name
    {
        get => memory.ReadString(_offset + 0x18);
        set => memory.WriteString(_offset + 0x18, value);
    }

    public string P1EditText
    {
        get => memory.ReadString(_offset + 0x42);
        set => memory.WriteString(_offset + 0x42, value);
    }

    public string P2EditText
    {
        get => memory.ReadString(_offset + 0x57);
        set => memory.WriteString(_offset + 0x57, value);
    }

    public string P3EditText
    {
        get => memory.ReadString(_offset + 0x6C);
        set => memory.WriteString(_offset + 0x6C, value);
    }

    public ref Word Points => 
        ref memory.GetRef<Word>(_offset + 0xC0);

    public string StepEditText
    {
        get => memory.ReadString(_offset + 0x96);
        set => memory.WriteString(_offset + 0x96, value);
    }

    public bool CanContainCode =>
        memory.Data[_offset + 0xAB] > 0;

    public bool NameMatches(ReadOnlySpan<char> name) =>
        memory.ReadStringSpan(_offset + 0x18).CaseInsensitiveCharacterEqual(name);
}